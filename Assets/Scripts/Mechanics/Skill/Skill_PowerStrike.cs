using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_PowerStrike : SkillObject
{
    public override void UseSkill()
    {
        StartCoroutine(SkillUseCR());
    }

    private IEnumerator SkillUseCR()
    {
        float poweredAtk = currCharacter.Atk * currSkillData.SkillValue;

        foreach (BattleCharController target in currTargets)
        {
            StringBuilder sb = new StringBuilder("");

            sb.AppendFormat("Used {0} on {1} dealing {2} damage", currSkillData.SkillName, target.CharName, poweredAtk);

            BattleOptionUI.LogAction(sb.ToString());

            target.TakeDamage(poweredAtk);

            yield return new WaitForSeconds(1);
        }

        BattleController.EndTurn();
    }
}
