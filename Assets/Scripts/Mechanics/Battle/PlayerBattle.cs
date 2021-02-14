using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBattle : MonoBehaviour
{
    [SerializeField] private GameObject         playerUi;
    [SerializeField] private BattleOptionUI     battleUi;
    [SerializeField] private BattleController   battleController;

    private BattleCharController target;
    private BattleCharController currChar;

    public void OnTurnStart(BattleCharController newChar)
    {
        currChar = newChar;
        playerUi.gameObject.SetActive(true);
    }

    public void ShowAttack()
    {
        if (battleUi == null || battleController == null) return;

        battleUi.ShowValidTargets(battleController.GetValidTargets("player"));
    }

    public void ShowSkills()
    {
        battleUi.ShowSkills(target.Skills);
    }

    public void AttackTarget(BattleCharController newTarget)
    {
        target = newTarget;

        float totalDamage = target.TakeDamage(currChar.GetAttackDamage());
        string action = target.CharName + " takes " + (int)totalDamage + " damage!";

        if (playerUi != null)
            playerUi.gameObject.SetActive(false);

        StartCoroutine(LogAndDelayEndCR(action));
    }

    private IEnumerator LogAndDelayEndCR(string logMsg)
    {
        battleUi.LogAction(logMsg);
        battleUi.DisableTargetList();

        yield return new WaitForSeconds(1);

        battleController.EndTurn();
    }
}
