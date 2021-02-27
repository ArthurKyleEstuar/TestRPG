using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class PlayerBattle : MonoBehaviour
{
    [SerializeField] private GameObject         playerUi;
    [SerializeField] private BattleOptionUI     battleUi;
    [SerializeField] private UnityEvent         onTurnStart;

    private BattleCharController currChar;

    public static bool          isUsingSkill;
    public static SkillObject   activeSkill;

    public void OnTurnStart(BattleCharController newChar)
    {
        currChar = newChar;

        if(playerUi != null)
            playerUi.gameObject.SetActive(true);

        isUsingSkill = false;
        activeSkill = null;

        onTurnStart?.Invoke();
    }

    public void ShowAttack()
    {
        if (battleUi == null) return;

        battleUi.ShowValidTargets(BattleController.GetValidTargets("player"));
    }

    public void ShowSkills()
    {
        battleUi.ShowSkills(currChar.Skills);
    }

    public void AttackTarget(BattleCharController newTarget)
    {
        float totalDamage = newTarget.TakeDamage(currChar.GetAttackDamage());
        string action = newTarget.CharName + " takes " + (int)totalDamage + " damage!";

        if (playerUi != null)
            playerUi.gameObject.SetActive(false);

        StartCoroutine(LogAndDelayEndCR(action));
    }

    public void OnAttack()
    {
        isUsingSkill = false;
        activeSkill = null;

        if (battleUi == null) return;

        battleUi.DisableTargetList();
        battleUi.DisableSkillList();
    }

    private IEnumerator LogAndDelayEndCR(string logMsg)
    {
        BattleOptionUI.LogAction(logMsg);
        OnAttack();

        yield return new WaitForSeconds(1);

        BattleController.EndTurn();
    }
}
