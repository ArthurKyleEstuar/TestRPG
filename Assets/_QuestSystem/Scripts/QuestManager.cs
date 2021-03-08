﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class QuestManager : BaseManager<QuestManager>
{
    [SerializeField] private QuestDatabase questMasterDB;
    private QuestDatabase curDB;

    public event System.Action OnQuestListUpdated;
    public event System.Action OnQuestProgressChanged;
    public event System.Action<string> OnQuestCompleted;

    public List<QuestData> AcceptedQuests => curDB.GetAcceptedQuests();
    public List<QuestData> OngoingQuests => curDB.GetOngoingQuests();
    public List<QuestData> CompletedQuests => curDB.GetCompletedQuests();
    public List<QuestData> FailedQuests => curDB.GetFailedQuests();

    protected override void Start()
    {
        base.Start();

        curDB = Instantiate(new QuestDatabase());
        if (questMasterDB != null)
            curDB.AddQuests(questMasterDB.GetAllQuests());
    }

    public void AcceptQuest(string id)
    {
        QuestData quest = curDB.GetQuest(id);
        if (quest == null) return;
        if (quest.GetState() != QuestState.NotAccepted) return;

        quest.StartQuest();
        quest.OnQuestCompleted += CompleteQuest;
        quest.OnQuestUpdated += UpdateQuests;
        OnQuestListUpdated?.Invoke();
    }

    public QuestData GetQuest(string id)
    {
        return curDB.GetQuest(id);
    }

    void UpdateQuests()
    {
        OnQuestProgressChanged?.Invoke();
    }

    public void CompleteQuest(string id)
    {
        QuestData toFinish = curDB.GetQuest(id);
        if (toFinish == null) return;
        if (toFinish.GetState() != QuestState.Ongoing) return;

        toFinish.CompleteQuest();

        OnQuestListUpdated?.Invoke();
        //OnQuestCompleted(id);
    }

    void SaveQuests()
    {

    }
}