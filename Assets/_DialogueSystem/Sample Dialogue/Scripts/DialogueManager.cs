using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DialogueManager : BaseManager<DialogueManager>
{
    private Dialogue curDialogue;
    public DialogueNode CurNode { get; private set; } = null;

    // Flag for when current node has more than 1 child
    public bool HasChoices { get; private set; } = false;

    // Fires action for any trigger present on node
    public event System.Action<string> OnAction;
    public event System.Action OnNextDialogue;
    public event System.Action OnDialogueEnd;

    public void StartDialogue(Dialogue newDialogue)
    {
        curDialogue = newDialogue;
        CurNode = newDialogue.GetRootNode();

        OnEnterActions();

        OnNextDialogue?.Invoke();
    }

    public void Quit()
    {
        curDialogue = null;

        OnExitActions();

        CurNode = null;
        HasChoices = false;
        OnDialogueEnd?.Invoke();
    }

    public string GetSpeaker()
    {
        if (CurNode == null) return "defText";
        return CurNode.GetSpeaker();
    }
    public string GetText()
    {
        if (CurNode == null) return "defText";
        return CurNode.GetText();
    }

    public void NextDialogue()
    {
        // Check if node has more than 1 child
        if (NodeHasChoices())
        {
            HasChoices = true;
            OnNextDialogue?.Invoke();
            return;
        }
        else
            HasChoices = false;

        // Check if node only has 1 child
        if (HasNextDialogue())
        {
            DialogueNode[] children = curDialogue.GetAllChildren(CurNode).ToArray();

            OnExitActions();

            CurNode = children[0];

            OnEnterActions();

            OnNextDialogue?.Invoke();
        }
        else
        {
            Quit();
        }
    }

    public void SelectChoice(DialogueNode node) 
    {
        OnExitActions();

        CurNode = node;

        OnEnterActions();

        HasChoices = false;
        NextDialogue();
    }

    // check if the current node has any children
    public bool HasNextDialogue()
    {
        return CurNode.GetChildren().Count > 0;
    }

    // Returns nodes based on current node's children
    public IEnumerable<DialogueNode> GetChoices()
    {
        return curDialogue.GetAllChildren(CurNode);
    }

    bool NodeHasChoices()
    {
        return CurNode.GetChildren().Count > 1;
    }

    void OnEnterActions()
    {
        if (CurNode.GetEnterActions() == "") return;
        OnAction?.Invoke(CurNode.GetEnterActions());
    }

    void OnExitActions()
    {
        if (CurNode.GetExitActions() == "") return;
        OnAction?.Invoke(CurNode.GetExitActions());
    }
}

