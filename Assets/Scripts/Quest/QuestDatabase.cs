using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEditor;

[CreateAssetMenu(menuName = "QuestDB")]
public class QuestDatabase : BaseDatabase<QuestData>
{
    /// <summary>
    /// Gets all quests in the DB
    /// </summary>
    /// <returns></returns>
    public List<QuestData> GetAllQuests() => data;
    
    public int GetQuestCount() => data.Count;

    /// <summary>
    /// Gets all quests that have not been accepted
    /// </summary>
    /// <returns></returns>
    public List<QuestData> GetNotAcceptedQuests()
    {
        return data.FindAll(quest => quest.State == QuestState.NotAccepted);
    }
    /// <summary>
    /// Gets all quests that have been accepted.  Quests may be ongoing, completed, or failed
    /// </summary>
    /// <returns></returns>
    public List<QuestData> GetAcceptedQuests()
    {
        return data.FindAll(quest => quest.State != QuestState.NotAccepted);
    }
    /// <summary>
    /// Gets all quests that have been accepted but not completed
    /// </summary>
    /// <returns></returns>
    public List<QuestData> GetOngoingQuests()
    {
        return data.FindAll(quest => quest.State == QuestState.Ongoing);
    }
    /// <summary>
    /// Gets all quests that have been completed
    /// </summary>
    /// <returns></returns>
    public List<QuestData> GetCompletedQuests()
    {
        return data.FindAll(quest => quest.State == QuestState.Completed);
    }
    /// <summary>
    /// Gets all quests that have been failed
    /// </summary>
    /// <returns></returns>
    public List<QuestData> GetFailedQuests()
    {
        return data.FindAll(quest => quest.State == QuestState.Failed);
    }
    
    public void EditQuest(string id, QuestData newQuest)
    {
        if (GetFile(id) == null) return;
        QuestData quest = GetFile(id);
        if (quest == newQuest) return;
        Undo.RecordObject(this, "Edit Quest");

        SetQuest(quest, newQuest);
        EditorUtility.SetDirty(this);
    }

    void SetQuest(QuestData oldQuest, QuestData newQuest)
    {
        oldQuest.Title = newQuest.Title;
        oldQuest.Description = newQuest.Description;
        oldQuest.Objectives = newQuest.Objectives;
        oldQuest.Reward = newQuest.Reward;
    }

    public void AddQuest(QuestData toAdd)
    {
        if (data.Exists(quest => quest.ID == toAdd.ID)) return;

        // Create new instance of quest
        QuestData newQuest = new QuestData(toAdd);
        data.Add(newQuest);
    }
    public void AddQuests(IEnumerable<QuestData> rangeToAdd)
    {
        for (int x = 0; x < rangeToAdd.Count(); x++)
        {
            AddQuest(rangeToAdd.ElementAt(x));
        }
    }
    public void RemoveQuest(string id)
    {
        if (GetFile(id) == null) return;
        
        data.RemoveAll(q => q.ID == id);
    }
}
