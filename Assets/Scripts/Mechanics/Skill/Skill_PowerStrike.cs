using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_PowerStrike : SkillObject
{
    public override void UseSkill()
    {
        float poweredAtk = currCharacter.Atk * currSkillData.SkillValue;
        currTargets[0].TakeDamage(poweredAtk);
    }
}
