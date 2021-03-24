using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public abstract class BaseDatabaseEditor : EditorWindow
{
    private Vector2 itemListScrollPos;

    // entire workable canvas
    protected readonly Rect canvasArea = new Rect(10, 10, 1000, 610);
    // section for list items
    protected readonly Rect itemListArea = new Rect(00, 00, 150, 600);
    // section for the database info
    protected readonly Rect itemInfoArea = new Rect(150, 00, 600, 600);

    protected EditorWindow window;
    protected Vector2 windowSize = new Vector2();

    // figure out how to do this
    protected virtual void OnEnable() { }

    private void OnGUI()
    {
        //DrawBorder(canvasArea);
        GUILayout.BeginArea(canvasArea);

        if (windowSize != window.position.size)
        {
            windowSize = window.position.size;
        }

        // draw some borders to define areas
        DrawBorder(itemListArea);
        DrawBorder(itemInfoArea);

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
    // modifiable override for menu items
    protected virtual void DrawMenuItems()
    {
        throw new NotImplementedException();
    }
    // find out how to do this
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

    protected virtual void DrawItemInfo()
    {
        throw new NotImplementedException();
    }


    protected void DrawBorder(Rect area)
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
