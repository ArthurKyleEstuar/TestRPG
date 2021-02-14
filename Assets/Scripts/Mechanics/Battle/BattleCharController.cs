using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleCharController : MonoBehaviour
{
    [Header("ID")]
    [SerializeField] private string teamId;
    [SerializeField] private string charName;

    [Header("Stats")]
    [SerializeField] private float maxHp = 10;
    [SerializeField] private float atk = 2;

    [Header("Skills")]
    [SerializeField] private SkillDatabase skillDb;
    [SerializeField] private List<string> skillIds = new List<string>();

    [SerializeField] private List<SkillObject> skillObjects = new List<SkillObject>();

    private float currHp;

    public string   TeamId => teamId;
    public string   CharName => charName;
    public float MaxHp => maxHp;
    public int CurrHp => (int)currHp;
    public float Atk => atk;
    public List<SkillObject> Skills => skillObjects;

    private void Start()
    {
        currHp = maxHp;

        foreach(string id in skillIds)
        {
            SkillData sd = skillDb.GetFile(id);

            GameObject go = Instantiate(sd.SkillObject, this.transform);

            SkillObject so = go.GetComponent<SkillObject>();

            skillObjects.Add(so);
        }
    }

    public float TakeDamage(float initDamage)
    {
        float finalDamage = initDamage;

        currHp -= finalDamage;

        Debug.LogError(CharName + " " + currHp);

        return finalDamage;
    }

    public float GetAttackDamage()
    {
        return atk;
    }
}
