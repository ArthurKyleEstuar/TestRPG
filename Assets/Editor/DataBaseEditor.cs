using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UI;

public class DataBaseEditor : EditorWindow
{
    private QuestDatabase questDB = null;

    private Vector2 itemListScrollPos;
    private Vector2 objScrollPos;

    // entire workable canvas
    private readonly Rect canvasArea = new Rect(10, 10, 1000, 1000);
    // section for list items
    private readonly Rect itemListArea = new Rect(00, 00, 150, 600);
    // section for the database info
    private readonly Rect infoArea = new Rect(110, 00, 600, 600);

    private Vector2 windowSize = new Vector2();
    private QuestData curQuest;

    EditorWindow window;

    [MenuItem("Window/DataBase Editor")]
    public static void OpenWindow()
    {
        GetWindow(typeof(DataBaseEditor));
    }

    private void OnGUI()
    {
        DrawBorder(canvasArea);
        GUILayout.BeginArea(canvasArea);

        if (windowSize != window.position.size)
        { 
            windowSize = window.position.size;
        }

        //DrawBorder(itemListArea);
        //DrawBorder(infoArea);
        EditorGUILayout.BeginHorizontal();
        DrawMenuItems();
        DrawQuestInfo();

        EditorGUILayout.EndHorizontal();
        GUILayout.EndArea();

        if (Event.current.type == EventType.MouseDown)
        {
            EditorGUI.FocusTextInControl(null);
            Repaint();
            //Debug.Log(Selection.activeObject.name);
        }
    }

    private void OnEnable()
    {
        window = GetWindow(typeof(DataBaseEditor));
        if (questDB == null) questDB = AssetDatabase.LoadAssetAtPath<QuestDatabase>("Assets/Database/questDB.asset");
        Undo.undoRedoPerformed += OnUndo;
    }

    void OnUndo()
    {

    }

    // draw list of items in DB
    void DrawMenuItems()
    {

        //GUILayout.BeginArea(itemListArea);
        EditorGUILayout.BeginVertical(GUILayout.Width(itemListArea.width));
        DrawQuestMenuItems();
        if (GUILayout.Button("x")
            && questDB.data.Count > 0)
        {
            questDB.data.RemoveAt(questDB.data.Count - 1);
        }
        if (GUILayout.Button("+"))
        {
            questDB.data.Add(new QuestData(id: (questDB.data.Count + 1).ToString()));
        }
        EditorGUILayout.EndVertical();
        //GUILayout.EndArea();
    }

    void DrawQuestMenuItems()
    {
        itemListScrollPos = EditorGUILayout.BeginScrollView(itemListScrollPos,GUILayout.Height(itemListArea.height));
        for (int x = 0; x < questDB.GetQuestCount(); x++)
        {
            QuestData quest = questDB.GetAllQuests()[x];
            if (GUILayout.Button(quest.ID))
            {
                Undo.RecordObject(this, "Select quest");
                curQuest = quest;
            }
        }
        EditorGUILayout.EndScrollView();
    }

    void DrawQuestInfo()
    {
        if (curQuest == null) return;
        EditorGUI.BeginChangeCheck();

        QuestData newQuest = new QuestData();

        //GUILayout.BeginArea(infoArea);
        EditorGUILayout.BeginVertical(GUILayout.Width(infoArea.width));

        // Draw quest title
        // adding tooltips to editor https://answers.unity.com/questions/914081/tooltips-in-editor-windows.html
        newQuest.Title = EditorGUILayout.TextField(new GUIContent("Title: ", "Name to appear in quest lists"), curQuest.Title);

        DrawQuestDescription(newQuest);


        DrawObjectives(newQuest);

        EditorGUILayout.Space(20);

        // Draw rewards
        newQuest.Reward = EditorGUILayout.TextField("Reward: ", curQuest.Reward);

        //GUILayout.EndArea();
        EditorGUILayout.EndVertical();
        if (EditorGUI.EndChangeCheck())
        {
            questDB.EditQuest(curQuest.ID, newQuest);
        }

    }

    private void DrawQuestDescription(QuestData newQuest)
    {
        // Draw description
        EditorGUILayout.LabelField(new GUIContent("Description: ", "Information about the quest"));
        GUIStyle style = new GUIStyle(EditorStyles.textArea)
        {
            wordWrap = true
        };
        newQuest.Description = EditorGUILayout.TextArea(curQuest.Description, style, GUILayout.Width(infoArea.width), GUILayout.Height(50));
    }

    private void DrawObjectives(QuestData newQuest)
    {
        // Draw objectives
        objScrollPos = EditorGUILayout.BeginScrollView(objScrollPos, GUILayout.Height(100));

        EditorGUILayout.LabelField("Objectives: ");

        newQuest.Objectives = new List<Objective>(curQuest.Objectives);

        for (int x = 0; x < curQuest.ObjectiveCount; x++)
        {
            newQuest.Objectives[x] = (Objective)EditorGUILayout.ObjectField(
                curQuest.Objectives[x],
                typeof(Objective),
                allowSceneObjects: false);
        }

        EditorGUILayout.EndScrollView();

        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Remove last objective")
            && newQuest.ObjectiveCount > 0)
        {
            newQuest.Objectives.RemoveAt(newQuest.ObjectiveCount - 1);
        }
        if (GUILayout.Button("Add new objective"))
        {
            newQuest.Objectives.Add(null);
        }

        EditorGUILayout.EndHorizontal();
    }

    void DrawBorder(Rect area) 
    {
        area.width += 10;
        area.height += 10;
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
