using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestListSelection : MonoBehaviour
{
    private enum ListType
    {
        Accepted = 0,
        Ongoing = 1,
        Completed = 2,
    }

    [SerializeField] private Button acceptedQuestsButton;
    [SerializeField] private Button ongoingQuestsButton;
    [SerializeField] private Button completedQuestsButton;

    [SerializeField] private QuestList questList;

    private List<QuestData> curList = new List<QuestData>();

    QuestManager manager;
    private void OnEnable()
    {
        if (manager == null) return;
        AddListeners();

        UpdateQuestList(ListType.Accepted);
    }


    private void OnDisable()
    {
        if (manager == null) return;
        RemoveListeners();
    }

    private void Start()
    {
        manager = QuestManager.Instance;
        if (manager == null) return;
        AddListeners();
    }

    void UpdateQuestList(ListType type)
    {
        if (manager == null) return;

        switch (type)
        {
            case ListType.Accepted:
                curList = manager.AcceptedQuests;
                break;
            case ListType.Ongoing:
                curList = manager.OngoingQuests;
                break;
            case ListType.Completed:
                curList = manager.CompletedQuests;
                break;
        }

        questList.UpdateUI(curList);
    }

    private void AddListeners()
    {
        // Add listeners to buttons for selecting list type
        acceptedQuestsButton.onClick.AddListener(() =>
        {
            UpdateQuestList(ListType.Accepted);
        });
        ongoingQuestsButton.onClick.AddListener(() =>
        {
            UpdateQuestList(ListType.Ongoing);
        });
        completedQuestsButton.onClick.AddListener(() =>
        {
            UpdateQuestList(ListType.Completed);
        });
    }
    private void RemoveListeners()
    {
        acceptedQuestsButton.onClick.RemoveAllListeners();
        ongoingQuestsButton.onClick.RemoveAllListeners();
        completedQuestsButton.onClick.RemoveAllListeners();
    }
}
