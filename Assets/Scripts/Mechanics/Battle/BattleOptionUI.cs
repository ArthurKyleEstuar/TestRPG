using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class BattleOptionUI : MonoBehaviour
{
    [Header("Target Data")]
    [SerializeField] private Transform targetList;
    [SerializeField] private GameObject targetButton;

    [Header("Skill Data")]
    [SerializeField] private Transform skillList;
    [SerializeField] private GameObject skillButton;

    [Header("Log")]
    [SerializeField] private TextMeshProUGUI enemyLog;

    private List<TargetSelect> targetObjects = new List<TargetSelect>();
    private List<SkillButton> skillButtons = new List<SkillButton>();

    #region Target Selection
    public void ShowValidTargets(List<BattleCharController> targets)
    {
        if(targetObjects == null || targetObjects.Count <= 0)
        {
            foreach(BattleCharController bcc in targets)
            {
                SetTargetButton(bcc);
            }
        }
        else
        {
            DisableTargetList();

            for(int i = 0; i < targets.Count; i++)
            {
                if (targetObjects[i] == null)
                {
                    SetTargetButton(targets[i]);
                }
                else
                {
                    targetObjects[i].gameObject.SetActive(true);
                    targetObjects[i].Initialize(targets[i]);
                }
            }
        }
    }

    public void DisableTargetList()
    {
        targetObjects.ForEach(obj => obj.gameObject.SetActive(false));
    }

    private void SetTargetButton(BattleCharController newData)
    {
        GameObject go = Instantiate(targetButton, targetList);

        TargetSelect ts = go.GetComponent<TargetSelect>();

        if (ts == null) return;

        ts.Initialize(newData);

        targetObjects.Add(ts);
    }
    #endregion

    public void ShowSkills(List<SkillObject> skills)
    {
        foreach(SkillObject skill in skills)
        {

        }
    }

    private void SetSkillButton()
    {

    }

    public void LogAction(string msg)
    {
        if (enemyLog == null) return;

        enemyLog.text = msg;
    }
}
