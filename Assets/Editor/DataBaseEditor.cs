using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

public class DataBaseEditor : EditorWindow
{
    private QuestDatabase questDB = null;

    private Vector2 listScrollPos;
    private Vector2 menuScrollPos;

    private readonly Rect canvasArea = new Rect(10, 10, 500, 500);
    private readonly Rect listArea = new Rect(10, 10, 100, 100);
    private readonly Rect infoArea = new Rect(120, 10, 300, 400);

    private QuestData curQuest;

    [MenuItem("Window/DataBase Editor")]
    public static void OpenWindow()
    {
        GetWindow(typeof(DataBaseEditor));
    }

    private void OnGUI()
    {
        if (questDB == null) questDB = AssetDatabase.LoadAssetAtPath<QuestDatabase>("Assets/Database/questDB.asset");
        GUILayout.BeginArea(canvasArea);
        //menuScrollPos = EditorGUILayout.BeginScrollView(menuScrollPos);
        DrawBorder(listArea);
        DrawBorder(infoArea);
        //EditorGUILayout.BeginHorizontal();
        DrawMenuItems();
        DrawQuestInfo();
        //EditorGUILayout.EndHorizontal();
        GUILayout.EndArea();
        //EditorGUILayout.EndScrollView();
    }

    private void OnEnable()
    {
    }

    // draw list of databases
    void DrawMenuList()
    {

    }

    // draw list of items in DB
    void DrawMenuItems()
    {
        GUILayout.BeginArea(listArea);
        listScrollPos = EditorGUILayout.BeginScrollView(listScrollPos);
        DrawQuestMenuItems();
        GUILayout.EndScrollView();
        if (GUILayout.Button("x"))
        {
            
        }
        GUILayout.EndArea();
    }

    void DrawQuestMenuItems()
    {
        for (int x = 0; x < questDB.GetQuestCount(); x++)
        {
            string id = questDB.GetAllQuests()[x].ID;
            if (GUILayout.Button(id))
            {
                curQuest = questDB.GetFile(id);
            }
        }
    }

    // draw item info
    void DrawItemInfo()
    {
        GUILayout.BeginArea(infoArea);
        
        EditorGUILayout.LabelField("Quest Title: ");
        EditorGUILayout.TextField("text");

        GUILayout.EndArea();
    }

    void DrawQuestInfo()
    {
        if (curQuest == null) return;
        EditorGUI.BeginChangeCheck();

        QuestData newQuest = new QuestData();

        GUILayout.BeginArea(infoArea);

        newQuest.Title = EditorGUILayout.TextField("Quest Title: ", curQuest.Title);


        EditorGUILayout.LabelField("Description:");
        GUIStyle style = new GUIStyle(EditorStyles.textArea)
        {
            wordWrap = true
        };
        newQuest.Description = EditorGUILayout.TextArea(curQuest.Description, style, GUILayout.Width(infoArea.width), GUILayout.Height(50));

        EditorGUILayout.LabelField("Objectives: ");
        newQuest.Objectives = new List<Objective>(curQuest.Objectives);
        for (int x = 0; x < curQuest.ObjectiveCount; x++)
        {
            newQuest.Objectives[x] = (Objective)EditorGUILayout.ObjectField(curQuest.Objectives[x], typeof(Objective),allowSceneObjects: false);
        }
        newQuest.Reward = EditorGUILayout.TextField("Reward: ", curQuest.Reward);

        GUILayout.EndArea();
        if(EditorGUI.EndChangeCheck())
        {
            questDB.EditQuest(curQuest.ID, newQuest);
        }
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
