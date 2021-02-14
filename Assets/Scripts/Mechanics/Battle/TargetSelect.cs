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

    private PlayerBattle            playerController;
    private BattleCharController    targetController;

    public void Initialize(BattleCharController newTargetData)
    {
        targetController = newTargetData;

        if(targetName != null)
            targetName.text = targetController.CharName;

        if (targetHpState != null)
            targetHpState.text = targetController.CurrHp + "/" + targetController.MaxHp;

        if(targetHpFill != null)
            targetHpFill.fillAmount = (float)targetController.CurrHp / targetController.MaxHp;
    }

    public void SelectTarget()
    {
        if (playerController == null)
            playerController = FindObjectOfType<PlayerBattle>();

        if (playerController == null) return;

        playerController.AttackTarget(targetController);
    }
}
