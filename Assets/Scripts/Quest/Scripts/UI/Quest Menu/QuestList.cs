using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestList : MonoBehaviour
{
    [SerializeField] private Button selPrefab;
    [SerializeField] private Transform questSelectionRoot;
    [SerializeField] private QuestInfo infoPanel;

    private List<Button> questSelections = new List<Button>();

    public void UpdateUI(List<QuestData> quests)
    {
        ClearUI();

        SetupQuestButtonList(quests); 
    }

    private void SetupQuestButtonList(List<QuestData> quests)
    {
        for (int x = 0; x < quests.Count; x++)
        {
            Button selection;
            QuestData curQuest = quests[x];

            // Check if there are more quests than UI elements
            if (questSelections.Count > x)
            {
                selection = questSelections[x];
            }
            else
            {
                selection = Instantiate(selPrefab, questSelectionRoot);
                questSelections.Add(selection);
            }

            selection.GetComponentInChildren<TMPro.TextMeshProUGUI>().text =
                curQuest.Title;

            // Add listener to update the quest info display
            selection.onClick.AddListener(() =>
            {
                infoPanel.UpdateUI(curQuest);
            });

            selection.gameObject.SetActive(true);
        }
    }

    void ClearUI()
    {
        if (questSelections.Count == 0) return;

        questSelections.ForEach(quest => 
        {
            quest.onClick.RemoveAllListeners();
            quest.gameObject.SetActive(false);
        });
    }
}
