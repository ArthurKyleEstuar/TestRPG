using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_SpeedUp : SkillObject
{

    protected override IEnumerator SkillUseCR()
    {
        foreach (BattleCharController target in currTargets)
        {
            target.Speed += CurrSkillData.SkillValue;

            StringBuilder sb = new StringBuilder("");

            sb.AppendFormat("Used {0} on {1}"
                , currSkillData.SkillName
                , target.CharName);

            BattleOptionUI.LogAction(sb.ToString());

            BattleController.SortBattlerSpeed();

            yield return new WaitForSeconds(1);
        }

        BattleController.EndTurn();
    }

    public override void ClearSkillEffect()
    {
        currTargets.ForEach(obj => obj.Speed -= CurrSkillData.SkillValue);
    }
}
