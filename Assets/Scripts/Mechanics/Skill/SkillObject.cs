using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillObject : MonoBehaviour
{
    protected SkillData                     currSkillData;
    protected int                           currSkillCooldown;
    protected BattleCharController          currCharacter;
    protected List<BattleCharController>    currTargets          = new List<BattleCharController>();

    public SkillData CurrSkillData => currSkillData;

    public virtual void Initialize(SkillData newData, BattleCharController skillOwner)
    {
        currSkillData = newData;
        currCharacter = skillOwner;
    }

    public abstract void UseSkill();

    public virtual void SetTarget(BattleCharController newTarget)
    {
        currTargets.Clear();
        currTargets.Add(newTarget);
    }

    public virtual void SetTarget(List<BattleCharController> newTargets)
    {
        currTargets.Clear();
        currTargets.AddRange(newTargets);
    }

    public virtual bool CanUseSkill(int availActionPoints)
    {
        if (currSkillData == null)
            return currSkillCooldown > 0;

        return currSkillData.SkillCost <= availActionPoints 
            && currSkillCooldown <= 0;
    }
    
}
