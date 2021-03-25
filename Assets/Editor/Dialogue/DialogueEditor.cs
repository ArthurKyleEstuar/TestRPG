using System.Collections;
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
    Dialogue selectedDialogue = null;

    #region NonSerialized_Fields
    [NonSerialized]
    DialogueNode draggedNode        = null;
    [NonSerialized]
    GUIStyle nodeStyle              = null;
    [NonSerialized]
    DialogueNode creatingNode       = null;
    [NonSerialized]
    DialogueNode deletingNode       = null;
    [NonSerialized]
    DialogueNode linkingParentNode  = null;
    [NonSerialized]
    bool isDraggingCanvas           = false;
    [NonSerialized]
    Vector2 scrollPosition;
    [NonSerialized]
    Vector2 mouseOffset;
    #endregion

    Vector2 draggingCanvasOffset;

    //#region Zoom_Properties
    //private const float kZoomMin = 0.1f;
    //private const float kZoomMax = 10.0f;

    //private readonly Rect _zoomArea = new Rect(0.0f, 75.0f, 600.0f, 300.0f - 100.0f);
    //private float _zoom = 1.0f;
    //private Vector2 _zoomCoordsOrigin = Vector2.zero;
    //#endregion

    const float canvasSize = 4000;
    const float bgSize = 50;

    private EditorWindow window;

    // create menu button for opening window
    [MenuItem("Window/Dialogue Editor")]
    private static void ShowEditorWindow()
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
    private static bool OnOpenAsset(int instanceID, int line)
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
        window = GetWindow<DialogueEditor>();
        SetNodeStyle();
    }

    private void SetNodeStyle()
    {
        // designing node look
        nodeStyle = new GUIStyle();
        // change node background               range of node0 to node6
        nodeStyle.normal.background = EditorGUIUtility.Load("node0") as Texture2D;
        //nodeStyle.normal.textColor = Color.white;
        // change offset of contents from borders
        nodeStyle.padding = new RectOffset(20, 20, 20, 20);
        // change background size
        nodeStyle.border = new RectOffset(12, 12, 12, 12);
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

            //if (canvasArea.width != window.position.width
            //    || canvasArea.height != window.position.height)
            //{
            //    window.
            //    canvasArea.width = window.position.width;
            //    canvasArea.height = window.position.height;
            //}

            /*
             * Draw Order: top to bottom
             * BG - very back
             * Connections - goes under nodes
             * Nodes
             * Toolbar - should be very top
             */

            // Scrolling the canvas
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);


            DrawBackground();
            DrawZoomedArea(); 

            EditorGUILayout.EndScrollView();

            DrawToolbar();

            if (creatingNode != null)
            {
                selectedDialogue.CreateNode(creatingNode);
                creatingNode = null;
            }
            if (deletingNode != null)
            {
                selectedDialogue.DeleteNode(deletingNode);
                deletingNode = null;
            }
        }
    }

    private void DrawZoomedArea()
    {
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
    }

    private static void DrawBackground()
    {
        // Canvas size
        Rect canvas = GUILayoutUtility.GetRect(canvasSize, canvasSize);
        Texture2D bgTex = Resources.Load("background") as Texture2D;

        Rect texCoords = new Rect(0, 0, canvasSize / bgSize, canvasSize / bgSize);

        // draw window background
        GUI.DrawTextureWithTexCoords(canvas, bgTex, texCoords);
    }

    // handle mouse events
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

        //if (Event.current.type == EventType.ScrollWheel)
        //{
        //    Vector2 screenCoordsMousePos = Event.current.mousePosition;
        //    Vector2 delta = Event.current.delta;
        //    Vector2 zoomCoordsMousePos = ConvertScreenCoordsToZoomCoords(screenCoordsMousePos);
        //    float zoomDelta = -delta.y / 150.0f;
        //    float oldZoom = _zoom;
        //    _zoom += zoomDelta;
        //    _zoom = Mathf.Clamp(_zoom, kZoomMin, kZoomMax);
        //    _zoomCoordsOrigin += (zoomCoordsMousePos - _zoomCoordsOrigin) - (oldZoom / _zoom) * (zoomCoordsMousePos - _zoomCoordsOrigin);

        //    Event.current.Use();
        //}
    }

    //private Vector2 ConvertScreenCoordsToZoomCoords(Vector2 screenCoords)
    //{
    //    return (screenCoords - _zoomArea.TopLeft()) / _zoom + _zoomCoordsOrigin;
    //}

    private void DrawToolbar()
    {
        //Rect rect = GetWindow<DialogueEditor>().position;
        //GUILayout.BeginHorizontal
        Rect rect = new Rect(0,
            0, 100, 100);
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
        EditNodeColor(node, style);

        // draws a box which represents the node
        GUILayout.BeginArea(node.GetRect(), nodeStyle);

        // draw input field
        DrawInputFields(node);
        DrawNodeButtons(node);

        GUILayout.EndArea();
    }
    private void EditNodeColor(DialogueNode node, GUIStyle style)
    {
        switch (node.GetNodeColor())
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
    }
    private void DrawInputFields(DialogueNode node)
    {
        node.SetNodeColor((NodeColor)EditorGUILayout.EnumPopup("Node Color", node.GetNodeColor()));
        node.SetSpeaker(EditorGUILayout.TextField("Speaker", node.GetSpeaker()));
        node.SetText(EditorGUILayout.TextField("Text", node.GetText()));
        node.SetAudio((AudioClip)EditorGUILayout.ObjectField("Audio Clip", node.GetAudioClip(), typeof(AudioClip), allowSceneObjects: false));
        
        node.SetCheckQuest(EditorGUILayout.Toggle("Check Quest", node.GetCheckQuestAvail()));
        if (node.GetCheckQuestAvail())
        {
            node.SetQuestId(EditorGUILayout.TextField("Quest ID", node.GetQuestId()));
        }

        node.SetEnterAction(EditorGUILayout.TextField("Enter Actions", node.GetEnterActions()));
        node.SetExitAction(EditorGUILayout.TextField("Exit Actions", node.GetExitActions()));
    }
    private void DrawNodeButtons(DialogueNode node)
    {
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

