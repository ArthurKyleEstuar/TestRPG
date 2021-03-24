using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class QuestManager : BaseManager<QuestManager>
{
    [SerializeField] private QuestDatabase questMasterDB;
    private QuestDatabase curDB;

    public event System.Action OnQuestListUpdated;
    public event System.Action OnQuestProgressChanged;
    public event System.Action<string> OnQuestAccepted;
    public event System.Action<string> OnQuestCompleted;

    // unnaccepted quests
    public List<QuestData> AvailableQuests => curDB.GetAvailableQuests();
    // accepted quests (ongoing, completed, failed)
    public List<QuestData> AcceptedQuests => curDB.GetAcceptedQuests();
    public List<QuestData> OngoingQuests => curDB.GetOngoingQuests();
    public List<QuestData> CompletedQuests => curDB.GetCompletedQuests();
    public List<QuestData> FailedQuests => curDB.GetFailedQuests();

    protected override void Start()
    {
        base.Start();

        curDB = Instantiate(new QuestDatabase());
        if (questMasterDB != null)
            curDB.AddQuestsCopy(questMasterDB.GetAllQuests());
    }

    public void AcceptQuest(string id)
    {
        QuestData quest = curDB.GetFile(id);
        if (quest == null) return;
        if (quest.State != QuestState.Available) return;

        quest.StartQuest();
        quest.OnQuestCompleted += CompleteQuest;
        quest.OnQuestUpdated += UpdateQuests;
        OnQuestListUpdated?.Invoke();
        OnQuestAccepted?.Invoke(id);
    }

    public QuestData GetQuest(string id)
    {
        return curDB.GetFile(id);
    }

    void UpdateQuests()
    {
        OnQuestProgressChanged?.Invoke();
    }

    public void CompleteQuest(string id)
    {
        QuestData toFinish = curDB.GetFile(id);
        if (toFinish == null) return;
        if (toFinish.State != QuestState.Ongoing) return;

        toFinish.CompleteQuest();

        OnQuestListUpdated?.Invoke();
        //OnQuestCompleted(id);
    }

    public bool IsQuestAvailable(string id)
    {
        // changed to check available/not accepted quests
        return AvailableQuests.Exists(obj => obj.ID == id);
    }

    void SaveQuests()
    {

    }
}
