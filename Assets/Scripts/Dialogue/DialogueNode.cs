using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

public enum NodeColor
{
    Gray = 0,
    Blue = 1,
    LightBlue = 2,
    Green = 3,
    Yellow = 4,
    Orange = 5,
    Red = 6
}

public class DialogueNode : ScriptableObject
{
    [SerializeField] private NodeColor      nodeColor = NodeColor.Gray;
    [SerializeField] private string         speaker;
    [TextArea(3, 4)]
    [SerializeField] private string         text;
    [SerializeField] private AudioClip      audioClip;
    [SerializeField] private List<string>   children        = new List<string>();
    [SerializeField] private Rect           rect            = new Rect(0, 0, 300, 200);

    [SerializeField] private string onEnterActions;
    [SerializeField] private string onExitActions;

    public NodeColor    GetNodeColor()      => nodeColor;
    public string       GetSpeaker()        => speaker;
    public string       GetText()           => text;
    public AudioClip    GetAudioClip()      => audioClip;
    public List<string> GetChildren()       => children;
    public Rect         GetRect()           => rect;
    public string       GetEnterActions()   => onEnterActions;
    public string       GetExitActions()    => onExitActions;

#if UNITY_EDITOR
    public void SetNodeColor(NodeColor color)
    {
        if (nodeColor == color) return;

        RecordUndo("Edit Node Color");
        nodeColor = color;
        EditorUtility.SetDirty(this);
    }

    public void SetSpeaker(string speaker)
    {
        if (this.speaker == speaker) return;

        RecordUndo("Edit Node Speaker");
        this.speaker = speaker;
        EditorUtility.SetDirty(this);
    }
    public void SetText(string text)
    {
        if (this.text == text) return;

        RecordUndo("Edit Node Text");
        this.text = text;
        EditorUtility.SetDirty(this);
    }
    public void SetAudio(AudioClip clip)
    {
        if (audioClip == clip) return;

        RecordUndo("Edit Node Audio");
        audioClip = clip;
        EditorUtility.SetDirty(this);
    }

    public void SetEnterAction(string enterActions)
    {
        if (onEnterActions == enterActions) return;

        RecordUndo("Edit Node Enter Actions");
        onEnterActions = enterActions;
        EditorUtility.SetDirty(this);
    }
    public void SetExitAction(string exitActions)
    {
        if (onExitActions== exitActions) return;

        RecordUndo("Edit Node Exit Actions");
        onExitActions = exitActions;
        EditorUtility.SetDirty(this);
    }

    public void SetPosition(Vector2 pos)
    {
        if (rect.position == pos) return;

        RecordUndo("Move Dialogue Node");
        rect.position = pos;
        EditorUtility.SetDirty(this);
    }
    public void SetSize(Vector2 size)
    {
        if (rect.size == size) return;

        RecordUndo("Set Node Size");
        rect.size = size;
        EditorUtility.SetDirty(this);
    }

    public void AddChild(string childID)
    {
        RecordUndo("Add Child Node");
        children.Add(childID);
        EditorUtility.SetDirty(this);
    }
    public void RemoveChild(string childID)
    {
        RecordUndo("Remove Child Node");
        children.Remove(childID);
        EditorUtility.SetDirty(this);
    }

    void RecordUndo(string undoMessage = "Edit Dialogue Node") 
    { 
        Undo.RecordObject(this, undoMessage);
    }
#endif
}

