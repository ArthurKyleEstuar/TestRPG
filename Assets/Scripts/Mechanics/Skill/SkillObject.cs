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

    private int currSkillRemainDuration;

    public virtual void Initialize(SkillData newData, BattleCharController skillOwner)
    {
        currSkillData = newData;
        currCharacter = skillOwner;
    }

    public void CountdownSkillDuration()
    {
        if (currSkillRemainDuration <= 0) return;

        currSkillRemainDuration--;

        if (currSkillRemainDuration <= 0)
            ClearSkillEffect();
    }

    public virtual void UseSkill()
    {
        currSkillRemainDuration = CurrSkillData.SkillDuration;

        StartCoroutine(SkillUseCR());
    }

    public abstract void ClearSkillEffect();

    protected abstract IEnumerator SkillUseCR();

    public void AddTarget(BattleCharController newTarget)
    {
        currTargets.Add(newTarget);
    }

    public void ClearTarget()
    {
        currTargets.Clear();
    }

    public bool CanSelectTarget()
    {
        return currTargets.Count < CurrSkillData.MaxTargetCount;
    }

    public virtual bool CanUseSkill(int availActionPoints)
    {
        if (currSkillData == null)
            return currSkillCooldown > 0;

        return currSkillData.SkillCost <= availActionPoints 
            && currSkillCooldown <= 0;
    }
    
}
