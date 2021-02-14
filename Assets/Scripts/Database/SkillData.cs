using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TargetType
{
    All         = 0,
    Enemy       = 1,
    Ally        = 2,
}

[System.Serializable]
public class SkillData : BaseData
{
    [SerializeField] private string     skillName;
    [SerializeField] private string     skillDesc;
    [SerializeField] private TargetType skillTargetType;
    [SerializeField] private int        skillCost;
    [SerializeField] private int        skillCooldown;
    [SerializeField] private float      skillValue;
    [SerializeField] private int        maxTargetCount;
    [SerializeField] private GameObject skillObject;

    public string       SkillName => skillName;
    public string       SkillDesc => skillDesc;
    public TargetType   SkillTargetType => skillTargetType;
    public int          SkillCost => skillCost;
    public int          SkillCooldown => skillCooldown;
    public float        SkillValue => skillValue;
    public int          MaxTargetCount => maxTargetCount;
    public GameObject   SkillObject => skillObject;
}
