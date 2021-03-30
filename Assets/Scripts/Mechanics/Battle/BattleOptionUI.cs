using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class BattleOptionUI : MonoBehaviour
{
    [Header("Target Data")]
    [SerializeField] private Transform  targetList;
    [SerializeField] private GameObject targetButton;

    [Header("Skill Data")]
    [SerializeField] private Transform  skillList;
    [SerializeField] private GameObject skillButton;

    [Header("Log")]
    [SerializeField] private TextMeshProUGUI enemyLog;

    private List<TargetSelect>  targetObjects = new List<TargetSelect>();
    private List<SkillButton>   skillButtons = new List<SkillButton>();

    private static BattleOptionUI instance;
    public static BattleOptionUI Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<BattleOptionUI>();

            if(instance == null)
            {
                Debug.LogError("NO BATTLE OPTION UI");
                return null;
            }

            return instance;
        }
    }

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
                if (i >= targetObjects.Count - 1)
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

    public void DisableSkillList()
    {
        skillButtons.ForEach(obj => obj.gameObject.SetActive(false));
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
        if (skillButtons == null || skillButtons.Count <= 0)
        {
            foreach (SkillObject skill in skills)
            {
                GameObject obj = Instantiate(skillButton, skillList);

                SkillButton sb = obj.GetComponent<SkillButton>();

                sb.Initialize(skill, this);

                skillButtons.Add(sb);
            }
        }
        else
        {
            for(int i = 0; i < skillButtons.Count; i++)
            {
                skillButtons[i].gameObject.SetActive(true);
                skillButtons[i].Initialize(skills[i], this);
            }
        }
    }

    public static void LogAction(string msg)
    {
        if (Instance.enemyLog == null) return;

        Instance.enemyLog.text = msg;
    }
}
