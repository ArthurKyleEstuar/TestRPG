using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(menuName = "Dialogue")]
public class Dialogue : ScriptableObject, ISerializationCallbackReceiver
{
    [SerializeField]
    List<DialogueNode> Nodes = new List<DialogueNode>();
    [SerializeField]
    Vector2 newNodeOffset = new Vector2(250, 0);
    [SerializeField]
    Vector2 defaultNodeSize = new Vector2(300, 200);

    public Vector2 DefaultNodeSize => defaultNodeSize;

    Dictionary<string, DialogueNode> nodeLookup = new Dictionary<string, DialogueNode>();
    private void Awake()
    {
#if UNITY_STANDALONE
        OnValidate();            
#endif
    }

    // called when something is changed in inspector or obj is loaded
    // does not get called automatically in build, should be called
    private void OnValidate()
    {
        nodeLookup.Clear();
        foreach(DialogueNode node in GetAllNodes())
        {
            nodeLookup[node.name] = node;
        }
    }

    // for any collection that can be for-looped
    public IEnumerable<DialogueNode> GetAllNodes()
    {
        return Nodes;
    }

    public DialogueNode GetRootNode() => Nodes[0];
    public int GetNodeCount() => Nodes.Count;

    public IEnumerable<DialogueNode> GetAllChildren(DialogueNode node)
    {
        foreach(string cUID in node.GetChildren())
        {
            if (nodeLookup.ContainsKey(cUID))
            {
                // generates an IEnumerable so no need to create a list
                yield return nodeLookup[cUID];
            }
        }
    }

#if UNITY_EDITOR
    public void CreateNode(DialogueNode parent)
    {
        DialogueNode newNode = MakeNode(parent);
        Undo.RegisterCreatedObjectUndo(newNode, "Create Node");
        Undo.RecordObject(this, "Added Dialogue Node");
        AddNode(newNode);
    }

    public void DeleteNode(DialogueNode toDelete)
    {
        Undo.RecordObject(this, "Deleted Dialogue Node");

        Nodes.Remove(toDelete);
        OnValidate();
        CleanChildren(toDelete);
        Undo.DestroyObjectImmediate(toDelete);
    }

    private DialogueNode MakeNode(DialogueNode parent)
    {
        // creates instance of scriptable object, does not make an asset file yet
        DialogueNode newNode = CreateInstance<DialogueNode>();
        newNode.name = Guid.NewGuid().ToString();
        if (parent != null)
        {
            parent.AddChild(newNode.name);
            newNode.SetPosition(parent.GetRect().position + newNodeOffset);
            newNode.SetSize(DefaultNodeSize);

        }
        return newNode;
    }

    private void AddNode(DialogueNode newNode)
    {
        Nodes.Add(newNode);
        OnValidate();
    }

    private void CleanChildren(DialogueNode toDelete)
    {
        foreach (DialogueNode node in GetAllNodes())
        {
            node.RemoveChild(toDelete.name);
        }
    }
#endif
    public void OnBeforeSerialize()
    {
#if UNITY_EDITOR
        // can also be put in OnValidate
        if (Nodes.Count == 0)
        {
            DialogueNode newNode = MakeNode(null);
            AddNode(newNode);
        }

        if (AssetDatabase.GetAssetPath(this) != "")
        {
            foreach (DialogueNode node in GetAllNodes())
            {
                if (AssetDatabase.GetAssetPath(node) == "")
                {
                    AssetDatabase.AddObjectToAsset(node, this);
                }
            }
        }
#endif
    }

    public void OnAfterDeserialize()
    {
    }
}

