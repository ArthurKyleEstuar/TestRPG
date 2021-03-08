﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// for editor code
using UnityEditor;
using UnityEditor.Callbacks;
using System;

/*
* stuff to look up
* "UnityEditor"
* "MenuItem"
* "GetWindow"
* "EditorUtility"
* "EditorGUI"
* "Selection"
* "Undo"
* "EventType"
*/


public class DialogueEditor : EditorWindow
{
    Dialogue        selectedDialogue    = null;

    [NonSerialized]
    DialogueNode    draggedNode         = null;
    [NonSerialized]
    GUIStyle        nodeStyle           = null; 
    [NonSerialized]
    DialogueNode    creatingNode        = null;
    [NonSerialized]
    DialogueNode    deletingNode        = null;
    [NonSerialized]
    DialogueNode    linkingParentNode   = null;
    [NonSerialized]
    bool            isDraggingCanvas    = false;
    [NonSerialized]
    Vector2         scrollPosition;
    [NonSerialized]
    Vector2         mouseOffset;

    Vector2         draggingCanvasOffset;

    const float canvasSize = 4000;
    const float bgSize = 50;
    // create menu button for opening window
    [MenuItem("Window/Dialogue Editor")]
    public static void ShowEditorWindow()
    {
        {
            /* creates editor window
                * function(window type[use class name], is utility, window name);
                */
        }
        GetWindow(typeof(DialogueEditor), false, "Dialogue Editor");
    }

    // order of callbacks, when opening assets
    [OnOpenAsset(1)]
    public static bool OnOpenAsset(int instanceID, int line)
    {
        // if selected object is typeof dialogue, open/display dialogue editor window
        Dialogue dialogue = EditorUtility.InstanceIDToObject(instanceID) as Dialogue;
        if(dialogue == null)
            return false;

        ShowEditorWindow();
        return true;
    }

    private void OnEnable()
    {
        // event when selected item has changed
        Selection.selectionChanged += OnSelectionChanged;

        #region Normal Node Style
        // designing node look
        nodeStyle = new GUIStyle();
        // change node background               range of node0 to node6
        nodeStyle.normal.background = EditorGUIUtility.Load("node0") as Texture2D;
        //nodeStyle.normal.textColor = Color.white;
        // change offset of contents from borders
        nodeStyle.padding = new RectOffset(20, 20, 20, 20);
        // change background size
        nodeStyle.border = new RectOffset(12, 12, 12, 12);
        #endregion

    }

    // if selected object is of type dialogue, refresh dialogue window
    private void OnSelectionChanged()
    {
        Dialogue newDialogue = Selection.activeObject as Dialogue;

        if (newDialogue != null)
        {
            selectedDialogue = newDialogue;
            Repaint();
        }
    }

    private void OnGUI()
    {
        {
            // force redraw window
            //Repaint();

            /*  Manually type in position
            *                               in pixels,         label
            *   EditorGUI.LabelField(new Rect(10, 10, 200, 200), "Hello");
                
                //automatically positions GUI
                GUILayoutUtility
                */
        }

        if (selectedDialogue == null)
        {
            EditorGUILayout.LabelField("No dialogue selected");
        }
        else
        {
            ProcessEvents();

            // Scrolling the canvas
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
            // Canvas size
            Rect canvas = GUILayoutUtility.GetRect(canvasSize, canvasSize);
            Texture2D bgTex = Resources.Load("background") as Texture2D;

            Rect texCoords = new Rect(0, 0, canvasSize / bgSize, canvasSize / bgSize);

            GUI.DrawTextureWithTexCoords(canvas, bgTex, texCoords);

            // Draw connections between nodes first so they draw under the nodes
            foreach (DialogueNode node in selectedDialogue.GetAllNodes())
            {
                DrawConnections(node);
            }
            // Draw nodes
            foreach (DialogueNode node in selectedDialogue.GetAllNodes())
            {
                DrawNode(node);
            }

            DrawToolbar();

            EditorGUILayout.EndScrollView();

            if(creatingNode != null)
            {
                selectedDialogue.CreateNode(creatingNode);
                creatingNode = null;
            }
            if(deletingNode != null)
            {
                selectedDialogue.DeleteNode(deletingNode);
                deletingNode = null;
            }
        }
    }

    private void ProcessEvents()
    {
        if (Event.current.type == EventType.MouseDown && draggedNode == null)
        {
            draggedNode = GetNodeAtPoint(Event.current.mousePosition + scrollPosition);
            if (draggedNode != null)
            {
                mouseOffset = Event.current.mousePosition - draggedNode.GetRect().position;
                Selection.activeObject = draggedNode;
            }
            else
            {
                // Record dragOffset and dragging
                isDraggingCanvas = true;
                draggingCanvasOffset = Event.current.mousePosition + scrollPosition;
                Selection.activeObject = selectedDialogue;
            }
        }
        else if (Event.current.type == EventType.MouseDrag && isDraggingCanvas)
        {
            // Update scrollPos
            scrollPosition = draggingCanvasOffset - Event.current.mousePosition;
            GUI.changed = true;
        }
        else if (Event.current.type == EventType.MouseDrag && draggedNode != null)
        {
            draggedNode.SetPosition(Event.current.mousePosition - mouseOffset);

            GUI.changed = true;
        }
        else if (Event.current.type == EventType.MouseUp && draggedNode != null)
        {
            isDraggingCanvas = false;
            draggedNode = null;
            GUI.changed = false;
        }
        else if(Event.current.type == EventType.MouseUp && isDraggingCanvas)
        {
            isDraggingCanvas = false;
            GUI.changed = false;
        }
    }

    private void DrawToolbar()
    {
        //Rect rect = GetWindow<DialogueEditor>().position;
        //GUILayout.BeginHorizontal
        Rect rect = new Rect(scrollPosition.x,
            scrollPosition.y, 100, 100);
        GUILayout.BeginArea(rect);

        if (GUILayout.Button("Reset node size"))
        {
            foreach (DialogueNode node in selectedDialogue.GetAllNodes())
            {
                node.SetSize(selectedDialogue.DefaultNodeSize);
            }
        }

        GUILayout.EndArea();
    }

    private void DrawNode(DialogueNode node)
    {
        GUIStyle style = nodeStyle;
        // Set node color
        switch(node.GetNodeColor())
        {
            case NodeColor.Gray:
                style.normal.background 
                    = EditorGUIUtility.Load("node0") as Texture2D;
                break;
            case NodeColor.Blue:
                style.normal.background 
                    = EditorGUIUtility.Load("node1") as Texture2D;
                break;
            case NodeColor.LightBlue:
                style.normal.background 
                    = EditorGUIUtility.Load("node2") as Texture2D;
                break;
            case NodeColor.Green:
                style.normal.background 
                    = EditorGUIUtility.Load("node3") as Texture2D;
                break;
            case NodeColor.Yellow:
                style.normal.background 
                    = EditorGUIUtility.Load("node4") as Texture2D;
                break;
            case NodeColor.Orange:
                style.normal.background 
                    = EditorGUIUtility.Load("node5") as Texture2D;
                break;
            case NodeColor.Red:
                style.normal.background 
                    = EditorGUIUtility.Load("node6") as Texture2D;
                break;
        }

        // draws a box which represents the node
        GUILayout.BeginArea(node.GetRect(), nodeStyle);

        EditorGUIUtility.labelWidth = 100f;
        node.SetNodeColor((NodeColor)EditorGUILayout.EnumPopup("Node Color", node.GetNodeColor()));

        node.SetSpeaker(EditorGUILayout.TextField("Speaker", node.GetSpeaker()));
        // set up text field for variables
        //                          text content,  text color
        node.SetText(EditorGUILayout.TextField("Text", node.GetText()));

        node.SetAudio((AudioClip)EditorGUILayout.ObjectField("Audio Clip", node.GetAudioClip(), typeof(AudioClip), allowSceneObjects: false));

        node.SetEnterAction(EditorGUILayout.TextField("Enter Actions", node.GetEnterActions()));

        node.SetExitAction(EditorGUILayout.TextField("Exit Actions", node.GetExitActions()));

        GUILayout.BeginHorizontal();

        // delete button
        if (GUILayout.Button("x"))
        {
            deletingNode = node;
        }

        DrawLinkButtons(node);

        // create button
        if (GUILayout.Button("+"))
        {
            creatingNode = node;
        }
        GUILayout.EndHorizontal();

        GUILayout.EndArea();
    }

    private void DrawLinkButtons(DialogueNode node)
    {
        if (linkingParentNode == null)
        {
            if (GUILayout.Button("link"))
            {
                linkingParentNode = node;
            }
        }
        else if (node == linkingParentNode)
        {
            if (GUILayout.Button("cancel"))
            {
                linkingParentNode = null;
            }
        }
        else if (linkingParentNode.GetChildren().Contains(node.name))
        {
            if (GUILayout.Button("unlink"))
            {
                linkingParentNode.RemoveChild(node.name);
                linkingParentNode = null;
            }
        }
        else
        {
            if (GUILayout.Button("child"))
            {
                linkingParentNode.AddChild(node.name);
                linkingParentNode = null;
            }
        }
    }

    private void DrawConnections(DialogueNode node)
    {
        Vector3 startPos = new Vector2(node.GetRect().xMax, node.GetRect().center.y);
        foreach (DialogueNode childNode in selectedDialogue.GetAllChildren(node))
        {
            Vector3 endPos = new Vector2(childNode.GetRect().xMin, childNode.GetRect().center.y);
            Vector3 curveOffset = endPos - startPos;
            curveOffset.y = 0;
            curveOffset.x *= 0.8f;
            Handles.DrawBezier(startPos, endPos,
                startPos + curveOffset,
                endPos - curveOffset,
                Color.white, null, 3f);
        }
    }

    private DialogueNode GetNodeAtPoint(Vector2 point)
    {
        DialogueNode foundNode = null;
        foreach(DialogueNode node in selectedDialogue.GetAllNodes())
        {
            if (node.GetRect().Contains(point))
                foundNode = node;
        }
        return foundNode;
    }
}
