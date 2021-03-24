using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI title;

    [SerializeField] private Transform objectivesRoot;
    [SerializeField] private ObjectiveUI objectivesPrefab;

    private List<ObjectiveUI> objectivesUI = new List<ObjectiveUI>();

    public void UpdateUI(QuestData quest)
    {
        ClearUI();
        title.text = quest.Title;

        SetupObjectivesList(quest);
    }

    private void SetupObjectivesList(QuestData quest)
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
        title.text = "";
        if (objectivesUI.Count == 0) return;
        objectivesUI.ForEach(ui => ui.gameObject.SetActive(false));
    }
}

