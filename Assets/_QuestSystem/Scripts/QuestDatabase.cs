using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(menuName = "QuestDB")]
public class QuestDatabase : BaseDatabase<QuestData>
{
    [SerializeField] private List<QuestData> quests = new List<QuestData>();

    /// <summary>
    /// Get a specific quest
    /// </summary>
    /// <param name="id">ID of quest to find</param>
    /// <returns></returns>
    public QuestData GetQuest(string id)
    {
        if (quests.Exists(q => q.GetID() == id))
            return quests.Find(q => q.GetID() == id);

        Debug.LogErrorFormat("{0} does not exist");
        return null;
    }

    /// <summary>
    /// Gets all quests in the DB
    /// </summary>
    /// <returns></returns>
    public List<QuestData> GetAllQuests()
    {
        return quests;
    }
    /// <summary>
    /// Gets all quests that have not been accepted
    /// </summary>
    /// <returns></returns>
    public List<QuestData> GetNotAcceptedQuests()
    {
        return quests.FindAll(quest => quest.GetState() == QuestState.NotAccepted);
    }
    /// <summary>
    /// Gets all quests that have been accepted.  Quests may be ongoing, completed, or failed
    /// </summary>
    /// <returns></returns>
    public List<QuestData> GetAcceptedQuests()
    {
        return quests.FindAll(quest => quest.GetState() != QuestState.NotAccepted);
    }
    /// <summary>
    /// Gets all quests that have been accepted but not completed
    /// </summary>
    /// <returns></returns>
    public List<QuestData> GetOngoingQuests()
    {

        return quests.FindAll(quest => quest.GetState() == QuestState.Ongoing);
    }
    /// <summary>
    /// Gets all quests that have been completed
    /// </summary>
    /// <returns></returns>
    public List<QuestData> GetCompletedQuests()
    {
        return quests.FindAll(quest => quest.GetState() == QuestState.Completed);
    }
    /// <summary>
    /// Gets all quests that have been failed
    /// </summary>
    /// <returns></returns>
    public List<QuestData> GetFailedQuests()
    {
        return quests.FindAll(quest => quest.GetState() == QuestState.Failed);
    }
    
    public void AddQuest(QuestData toAdd)
    {
        if (quests.Exists(quest => quest.GetID() == toAdd.GetID())) return;

        // Create new instance of quest
        QuestData newQuest = new QuestData(toAdd);
        quests.Add(newQuest);
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
        if (GetQuest(id) == null) return;

        quests.RemoveAll(q => q.GetID() == id);
    }
}
