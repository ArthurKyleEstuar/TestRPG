using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UI;

/*
 * Todo
 * - (option 1) make system undoable, but remove undo on confirmation
 * - (option 2) don't make the system undoable
 */



public class DataBaseEditor : EditorWindow
{
    private enum DatabaseTypes
    {
        Default = 0,
        Quest = 1,
        Skill = 2,
    }

    private DatabaseTypes dbType = DatabaseTypes.Default;

    private QuestDatabase questDB = null;
    private SkillDatabase skillDB = null;

    private List<BaseData> curList;

    private Vector2 dbListScrollPos;
    private Vector2 itemListScrollPos;
    private Vector2 objScrollPos;

    // entire workable canvas
    private readonly Rect canvasArea    = new Rect(10, 10, 1000, 600);
    // section for DB list
    private readonly Rect dbListArea    = new Rect(00, 00, 150, 600);
    // section for list items
    private readonly Rect itemListArea  = new Rect(150, 00, 150, 600);
    // section for the database info
    private readonly Rect itemInfoArea  = new Rect(300, 00, 600, 600);

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

        DrawBorder(dbListArea);
        DrawBorder(itemListArea);
        DrawBorder(itemInfoArea);
        EditorGUILayout.BeginHorizontal();
        DrawDatabaseList();
        if (dbType != DatabaseTypes.Default)
        {
            DrawItemList();
            DrawItemInfo();
        }

        EditorGUILayout.EndHorizontal();
        GUILayout.EndArea();

        // deselect current input field
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
        if (skillDB == null) skillDB = AssetDatabase.LoadAssetAtPath<SkillDatabase>("Assets/Database/SkillDB.asset");
    }

    #region General_Functions
    // draw list of databases
    private void DrawDatabaseList()
    {
        EditorGUILayout.BeginVertical(GUILayout.Width(dbListArea.width));
        dbListScrollPos = EditorGUILayout.BeginScrollView(dbListScrollPos);
        if (GUILayout.Button("Quest"))
        {
            dbType = DatabaseTypes.Quest;
        }
        if (GUILayout.Button("Skill"))
        {
            dbType = DatabaseTypes.Skill;
        }

        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();
    } 
    // draw list of items in current DB
    private void DrawItemList()
    {
        //GUILayout.BeginArea(itemListArea);
        EditorGUILayout.BeginVertical(GUILayout.Width(itemListArea.width));
        itemListScrollPos = EditorGUILayout.BeginScrollView(itemListScrollPos);

        switch (dbType)
        {
            case DatabaseTypes.Quest:
                DrawQuestMenuItems();
                break;
        }
        EditorGUILayout.EndScrollView();
        DrawItemListButtons();

        //EditorGUILayout.Space(10);
        EditorGUILayout.EndVertical();
        //GUILayout.EndArea();
    }
    // draw exra item list buttons
    private void DrawItemListButtons()
    {
        // Remove last item on list
        if (GUILayout.Button("x")
            && questDB.data.Count > 0)
        {
            questDB.data.RemoveAt(questDB.data.Count - 1);
        }

        // Add new item to the list
        if (GUILayout.Button("+"))
        {
            questDB.data.Add(new QuestData(id: (questDB.data.Count + 1).ToString()));
        }
    }
    // draw info panel
    private void DrawItemInfo()
    {
        switch(dbType)
        {
            case DatabaseTypes.Quest:
                DrawQuestInfo();
                break;
            case DatabaseTypes.Default:
                break;
        }
    }
    #endregion

    #region Quest_GUI
    private void DrawQuestMenuItems()
    {
        for (int x = 0; x < questDB.GetQuestCount(); x++)
        {
            QuestData quest = questDB.GetAllQuests()[x];
            if (GUILayout.Button(quest.ID))
            {
                curQuest = quest;
            }
        }
    }
    private void DrawQuestInfo()
    {
        if (curQuest == null) return;
        EditorGUI.BeginChangeCheck();

        QuestData newQuest = new QuestData();

        //GUILayout.BeginArea(infoArea);

        EditorGUILayout.BeginVertical(GUILayout.Width(itemInfoArea.width));
        DrawQuestTitle(newQuest);
        DrawQuestDescription(newQuest);
        DrawQuestObjectives(newQuest);
        EditorGUILayout.Space(20);
        DrawQuestRewards(newQuest);
        EditorGUILayout.EndVertical();
        //GUILayout.EndArea();

        if (EditorGUI.EndChangeCheck())
        {
            questDB.EditQuest(curQuest.ID, newQuest);
        }
    }
    private void DrawQuestTitle(QuestData newQuest)
    {
        // adding tooltips to editor https://answers.unity.com/questions/914081/tooltips-in-editor-windows.html
        newQuest.Title = EditorGUILayout.TextField(
            new GUIContent("Title: ", "Name to appear in quest lists"),
            curQuest.Title);
    }
    private void DrawQuestRewards(QuestData newQuest)
    {
        // Draw rewards
        newQuest.Reward = EditorGUILayout.TextField("Reward: ", curQuest.Reward);
    }
    private void DrawQuestDescription(QuestData newQuest)
    {
        EditorGUILayout.LabelField(new GUIContent("Description: ", "Information about the quest"));
        GUIStyle style = new GUIStyle(EditorStyles.textArea)
        {
            wordWrap = true
        };
        newQuest.Description = EditorGUILayout.TextArea(
            curQuest.Description,
            style,
            GUILayout.Width(itemInfoArea.width),
            GUILayout.Height(50));
    }
    private void DrawQuestObjectives(QuestData newQuest)
    {
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

        DrawQuestObjectiveButtons(newQuest);
    }
    private void DrawQuestObjectiveButtons(QuestData newQuest)
    {
        EditorGUILayout.BeginHorizontal();

        // Remove last objective slot
        if (GUILayout.Button("Remove last objective")
            && newQuest.ObjectiveCount > 0)
        {
            newQuest.Objectives.RemoveAt(newQuest.ObjectiveCount - 1);
        }

        // Add a new objective slot
        if (GUILayout.Button("Add new objective"))
        {
            newQuest.Objectives.Add(null);
        }

        EditorGUILayout.EndHorizontal();
    }
    #endregion

    void DrawBorder(Rect area) 
    {
        //area.width += 10;
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
