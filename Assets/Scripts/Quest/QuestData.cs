using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public enum QuestState
{
    NotAccepted = 0,
    Ongoing     = 1,
    Completed   = 2,
    Failed      = 3,
}

[System.Serializable]
public class QuestData : BaseData
{
    [SerializeField] private string             questTitle;
    [TextArea(3,4)]
    [SerializeField] private string             description;
    [SerializeField] private List<Objective>    objectives;
    [SerializeField] private string             rewards;
    private QuestState state;

    public QuestData()
    {
        id = "";
        questTitle = "";
        description = "";
        objectives = new List<Objective>();
        rewards = "";
        state = QuestState.NotAccepted;
    }

    public QuestData(QuestData copy)
    {
        id = copy.id;
        questTitle = copy.questTitle;
        description = copy.description;
        // Generate list of objectives
        List<Objective> newList = new List<Objective>();
        for (int x = 0; x < copy.ObjectiveCount; x++)
        {
            // Create new instance of objective SO
            Objective obj = Object.Instantiate(copy.objectives[x]);
            newList.Add(obj);
        }
        objectives = newList;
        rewards = copy.rewards;
        state = copy.state;
    }

    public bool IsCompleted { get; private set; } = false;

    public event System.Action OnQuestUpdated;
    public event System.Action<string> OnQuestCompleted;

    public string           Title { get { return questTitle; } set { questTitle = value; } }
    public string           Description { get { return description; } set { description = value; } }
    public string           Reward { get { return rewards; } set { rewards = value; } }
    public QuestState       State { get { return state; } set { state = value; } }
    public List<Objective>  Objectives { get { return objectives; } set { objectives = value; } }
    public int              ObjectiveCount =>  objectives.Count;

    public void StartQuest()
    {
        state = QuestState.Ongoing;
        objectives.ForEach(_ => _.Init());
        objectives.ForEach(_ => _.OnProgressUpdated += UpdateProgress);
        objectives.ForEach(_ => _.OnObjectiveCompleted += Evaluate);
    }

    // Check if all objectives have been completed
    void Evaluate() 
    {
        UpdateProgress();
        if (objectives.All(obj => obj.IsCompleted == true)) OnQuestCompleted?.Invoke(id);
    }

    void UpdateProgress()
    {
        OnQuestUpdated?.Invoke();
    }

    public void CompleteQuest()
    {
        state = QuestState.Completed;
        objectives.ForEach(obj => obj.OnObjectiveCompleted -= Evaluate);
    }
}
