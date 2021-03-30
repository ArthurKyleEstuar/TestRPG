using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class SkillButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI skillName;
    [SerializeField] private TextMeshProUGUI skillCost;

    private BattleOptionUI  battleUI;
    private SkillData       currSkillData;
    private SkillObject     skillRef;

    public void Initialize(SkillObject newSkillObj, BattleOptionUI newUI)
    {
        battleUI = newUI;
        skillRef = newSkillObj;
        skillRef.ClearTarget();

        currSkillData = skillRef.CurrSkillData;

        if (skillName != null)
            skillName.text = currSkillData.SkillName;

        if (skillCost != null)
            skillCost.text = currSkillData.SkillCost.ToString();
    }

    public void UseSkill()
    {
        string target = "";

        switch(currSkillData.SkillTargetType)
        {
            case TargetType.Ally:
                target = "player";
                break;

            case TargetType.Enemy:
                target = "bad";
                break;
        }

        PlayerBattle.isUsingSkill = true;
        PlayerBattle.activeSkill = skillRef;

        battleUI.ShowValidTargets(BattleController.GetValidTargets(target));
        battleUI.DisableSkillList();
    }
}
