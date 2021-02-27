using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class TargetSelect : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI    targetName;
    [SerializeField] private TextMeshProUGUI    targetHpState;
    [SerializeField] private Image              targetHpFill;

    [SerializeField] private RectTransform      counters;
    [SerializeField] private GameObject         counterPrefab;

    private List<GameObject> counterObjects = new List<GameObject>();

    private PlayerBattle            playerBattle;
    private BattleCharController    targetController;
    private int currTargettedCount;

    public void Initialize(BattleCharController newTargetData)
    {
        targetController = newTargetData;

        if(targetName != null)
            targetName.text = targetController.CharName;

        if (targetHpState != null)
            targetHpState.text = targetController.CurrHp + "/" + targetController.MaxHp;

        if(targetHpFill != null)
            targetHpFill.fillAmount = (float)targetController.CurrHp / targetController.MaxHp;

        currTargettedCount = 0;

        for (int i = 0; i < counterObjects.Count; i++)
            Destroy(counterObjects[i]);

        counterObjects.Clear();
    }

    public void SelectTarget()
    {
        if (playerBattle == null)
            playerBattle = FindObjectOfType<PlayerBattle>();

        if (playerBattle == null) return;

        if (PlayerBattle.isUsingSkill)
        {
            PlayerBattle.activeSkill.AddTarget(targetController);
            currTargettedCount++;

            if(counterPrefab != null)
            {
                GameObject obj = Instantiate(counterPrefab, counters);

                TargetCounter tc = obj.GetComponent<TargetCounter>();

                if(tc != null) 
                    tc.Initialize(currTargettedCount);

                counterObjects.Add(obj);
            }

            if (!PlayerBattle.activeSkill.CanSelectTarget())
            {
                PlayerBattle.activeSkill.UseSkill();

                playerBattle.OnAttack();
            }
        }
        else
        {
            playerBattle.AttackTarget(targetController);
        }
    }
}
