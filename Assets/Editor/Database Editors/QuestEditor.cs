using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UI;
using System;

/*
 * Todo
 * - (option 1) make system undoable, but remove undo on confirmation
 * - (option 2) don't make the system undoable
 */

public class QuestEditor : BaseDatabaseEditor
{
    private QuestDatabase questDB = null;

    private Vector2 objScrollPos;

    // entire workable canvas

    private QuestData curQuest;


    [MenuItem("Database/Quest Editor")]
    public static void OpenQuestWindow()
    {
        GetWindow(typeof(QuestEditor));
    }

    protected override void OnEnable()
    {
        window = GetWindow(typeof(QuestEditor));
        if (questDB == null) questDB = AssetDatabase.LoadAssetAtPath<QuestDatabase>("Assets/Database/questDB.asset");
    }

    #region General_Functions
    protected override void DrawHeader()
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Quests", GUILayout.Width(itemListArea.width));
        EditorGUILayout.LabelField("Information", GUILayout.Width(itemInfoArea.width));
        EditorGUILayout.EndHorizontal();
    }
    protected override void DrawItemListButtons()
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
    protected override void DrawMenuItems()
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
    // draw info panel
    protected override void DrawItemInfo()
    {
        DrawQuestInfo();
    }
    #endregion


    #region Quest_GUI
    private void DrawQuestInfo()
    {
        if (curQuest == null) return;
        float width = itemInfoArea.width - 10;

        EditorGUI.BeginChangeCheck();

        QuestData newQuest = new QuestData();

        //GUILayout.BeginArea(infoArea);

        EditorGUILayout.BeginVertical(GUILayout.Width(itemInfoArea.width));

        DrawQuestID(newQuest, width);
        DrawQuestTitle(newQuest, width);
        DrawQuestDescription(newQuest, width);
        DrawQuestObjectives(newQuest, width);
        EditorGUILayout.Space(20);
        DrawQuestRewards(newQuest, width);

        EditorGUILayout.EndVertical();
        //GUILayout.EndArea();

        if (EditorGUI.EndChangeCheck())
        {
            questDB.EditQuest(curQuest.ID, newQuest);
        }
    }
    private void DrawQuestID(QuestData newQuest, float width)
    {
        newQuest.SetID(EditorGUILayout.TextField(
            "ID:",
            curQuest.ID,
            GUILayout.MaxWidth(width)));
    }
    private void DrawQuestTitle(QuestData newQuest, float width)
    {
        // adding tooltips to editor https://answers.unity.com/questions/914081/tooltips-in-editor-windows.html
        newQuest.Title = EditorGUILayout.TextField(
            new GUIContent("Title: ", "Name to appear in quest lists"),
            curQuest.Title,
            GUILayout.Width(width));
    }
    private void DrawQuestDescription(QuestData newQuest, float width)
    {
        EditorGUILayout.LabelField(new GUIContent("Description: ", "Information about the quest"));
        GUIStyle style = new GUIStyle(EditorStyles.textArea)
        {
            wordWrap = true
        };
        newQuest.Description = EditorGUILayout.TextArea(
            curQuest.Description,
            style,
            GUILayout.Width(width),
            GUILayout.Height(50));
    }
    private void DrawQuestObjectives(QuestData newQuest, float width)
    {
        objScrollPos = EditorGUILayout.BeginScrollView(objScrollPos, GUILayout.Height(100));

        EditorGUILayout.LabelField("Objectives: ");

        newQuest.Objectives = new List<Objective>(curQuest.Objectives);

        for (int x = 0; x < curQuest.ObjectiveCount; x++)
        {
            newQuest.Objectives[x] = (Objective)EditorGUILayout.ObjectField(
                curQuest.Objectives[x],
                typeof(Objective),
                allowSceneObjects: false,
                GUILayout.Width(width));
        }

        EditorGUILayout.EndScrollView();

        DrawQuestObjectiveButtons(newQuest, width);
    }
    private void DrawQuestObjectiveButtons(QuestData newQuest, float width)
    {
        EditorGUILayout.BeginHorizontal(GUILayout.Width(width));

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
    private void DrawQuestRewards(QuestData newQuest, float width)
    {
        // Draw rewards
        newQuest.Reward = EditorGUILayout.TextField(
            "Reward: ",
            curQuest.Reward,
            GUILayout.Width(width));
    }
    #endregion
}
