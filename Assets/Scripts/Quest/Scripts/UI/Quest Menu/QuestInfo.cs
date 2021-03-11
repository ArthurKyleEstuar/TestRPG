using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestInfo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private ObjectiveUI objectivesPrefab;
    [SerializeField] private Transform objectivesRoot;
    [SerializeField] private TextMeshProUGUI rewards;

    private List<ObjectiveUI> objectivesUI = new List<ObjectiveUI>();

    private void OnEnable()
    {
        ClearUI();
    }

    public void UpdateUI(QuestData quest)
    {
        ClearUI();

        description.text = quest.Description;
        SetupObjectiveList(quest);
        rewards.text = quest.Reward;
    }

    private void SetupObjectiveList(QuestData quest)
    {
        for (int x = 0, y = quest.ObjectiveCount; x < y; x++)
        {
            ObjectiveUI objective;

            if (objectivesUI.Count > x)
            {
                objective = objectivesUI[x];
            }
            else
            {
                objective = Instantiate(objectivesPrefab, objectivesRoot);
                objectivesUI.Add(objective);
            }

            objective.UpdateUI(quest.Objectives[x]);
            objective.gameObject.SetActive(true);
        }
    }

    private void ClearUI()
    {
        description.text = "";
        rewards.text = "";
        if (objectivesUI.Count == 0) return;
        objectivesUI.ForEach(ui => ui.gameObject.SetActive(false));
    }
}
