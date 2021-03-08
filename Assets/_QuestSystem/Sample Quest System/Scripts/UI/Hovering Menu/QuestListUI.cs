using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class QuestListUI : MonoBehaviour
{
    [SerializeField] private QuestUI questPrefab;

    private QuestManager manager;

    private List<QuestUI> questUIs = new List<QuestUI>();
    // Start is called before the first frame update
    void Start()
    {
        manager = QuestManager.Instance;
        manager.OnQuestListUpdated += UpdateUI;
        manager.OnQuestProgressChanged += UpdateUI;
    }

    private void UpdateUI()
    {
        ClearUI();

        SetupQuestList();
    }

    private void SetupQuestList()
    {
        for (int x = 0, y = manager.OngoingQuests.Count(); x < y; x++)
        {
            QuestUI itemUI;

            // Check if there are more quests than UI elements
            if (questUIs.Count > x)
            {
                itemUI = questUIs[x];
            }
            else
            {
                itemUI = Instantiate(questPrefab, transform);
                questUIs.Add(itemUI);
            }

            itemUI.UpdateUI(manager.OngoingQuests[x]);
            questUIs[x].gameObject.SetActive(true);
        }
    }

    private void ClearUI()
    {
        if (questUIs.Count == 0) return;
        questUIs.ForEach(ui => ui.gameObject.SetActive(false));
    }
}

