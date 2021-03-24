using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public abstract class BaseDatabaseEditor : EditorWindow
{
    private Vector2 itemListScrollPos;

    #region Draw_Area
    // entire workable canvas
    protected readonly Rect canvasArea = new Rect(10, 10, 1000, 610);
    protected readonly Rect headerArea = new Rect(0, 0, 1000, 20);
    // section for list items
    protected readonly Rect itemListArea = new Rect(00, 20, 150, 580);
    // section for the database info
    protected readonly Rect itemInfoArea = new Rect(150, 20, 600, 580);
    #endregion

    //protected EditorWindow window;
    //protected Vector2 windowSize = new Vector2();

    // figure out how to make this more generic
    protected virtual void OnEnable() { }

    private void OnGUI()
    {
        //DrawBorder(canvasArea);
        GUILayout.BeginArea(canvasArea);

        // unneeded function for now
        //if (windowSize != window.position.size)
        //{
        //    windowSize = window.position.size;
        //}

        // draw some borders to define areas
        DrawBorder(itemListArea);
        DrawBorder(itemInfoArea);

        DrawHeader();

        EditorGUILayout.BeginHorizontal();

        DrawItemList();
        DrawItemInfo();

        EditorGUILayout.EndHorizontal();
        GUILayout.EndArea();

        // deselect current input field
        if (Event.current.type == EventType.MouseDown)
        {
            EditorGUI.FocusTextInControl(null);
            Repaint();
        }
    }

    // draw area labels
    protected virtual void DrawHeader()
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("List", GUILayout.Width(itemListArea.width));
        EditorGUILayout.LabelField("Information", GUILayout.Width(itemInfoArea.width));
        EditorGUILayout.EndHorizontal();
    }

    // draw items in the db
    protected void DrawItemList()
    {
        EditorGUILayout.BeginVertical(
            GUILayout.Height(itemListArea.height - 5),
            GUILayout.Width(itemListArea.width));

        itemListScrollPos = EditorGUILayout.BeginScrollView(itemListScrollPos);

        DrawMenuItems();

        EditorGUILayout.EndScrollView();

        DrawItemListButtons();

        EditorGUILayout.EndVertical();
    }
    // override for drawing menu items
    protected virtual void DrawMenuItems()
    {
        throw new NotImplementedException();
    }
    // override for setting up add and remove buttons
    // find out how to convert this to generic editing
    protected virtual void DrawItemListButtons()
    {
        //// Remove last item on list
        //if (GUILayout.Button("x")
        //    && curDB.data.Count == 0)
        //{
        //    curDB.data.RemoveAt(curDB.data.Count - 1);
        //}

        //// Add new item to the list
        //if (GUILayout.Button("+"))
        //{
        //    curDB.data.Add(new QuestData(id: (curDB.data.Count + 1).ToString()));
        //}
    }
    // override for drawing item info
    protected virtual void DrawItemInfo()
    {
        throw new NotImplementedException();
    }

    // draw border around defined areas
    private void DrawBorder(Rect area)
    {
        //area.width += 10;
        //area.height += 10;
        Vector3[] positions = new Vector3[8];
        Vector2 topLeft = area.position;
        Vector2 topRight = new Vector2(area.x + area.width, area.y);
        Vector2 bottomRight = area.size + area.position;
        Vector2 bottomLeft = new Vector2(area.x, area.y + area.height);

        positions[0] = topLeft;
        positions[1] = topRight;
        positions[2] = topRight;
        positions[3] = bottomRight;
        positions[4] = bottomRight;
        positions[5] = bottomLeft;
        positions[6] = bottomLeft;
        positions[7] = topLeft;

        Handles.DrawLines(positions);
    }
}
