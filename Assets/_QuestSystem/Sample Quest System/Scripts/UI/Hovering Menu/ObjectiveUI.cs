using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ObjectiveUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private TextMeshProUGUI progress;
    [SerializeField] private GameObject progressCheckMark;

    public void UpdateUI(Objective objective)
    {
        if (description != null)
            description.text = objective.Description;

        if (progressCheckMark != null)
            progressCheckMark.SetActive(objective.IsCompleted);

        if (progress != null)
            UpdateProgress(objective);
    }

    private void UpdateProgress(Objective objective)
    {
        // Check if objective type is a counter
        if (objective.GetType() == typeof(CounterObjective))
        {
            progress.gameObject.SetActive(true);
            CounterObjective obj = objective as CounterObjective;

            progress.text = string.Format("{0}/{1}", obj.CurGoal, obj.MaxGoal);
        }
        else
        {
            progress.gameObject.SetActive(false);
        }
    }

    public void SetText(string text)
    {
        description.text = text;
    }
}
