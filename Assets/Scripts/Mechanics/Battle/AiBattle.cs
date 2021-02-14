using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiBattle : MonoBehaviour
{
    [SerializeField] private BattleController       battleController;
    [SerializeField] private BattleCharController   aiData;
    [SerializeField] private BattleOptionUI         battleUi;

    private void Start()
    {
        battleController = FindObjectOfType<BattleController>();
        aiData = GetComponent<BattleCharController>();
        battleUi = FindObjectOfType<BattleOptionUI>();
    }

    public void ProcessTurn()
    {
        if (battleUi != null)
            battleUi.LogAction(aiData.CharName + " is thinking...");

        StartCoroutine(DelayedEndTurnCR());
    }

    private IEnumerator DelayedEndTurnCR()
    {
        yield return new WaitForSeconds(1);

        if (battleUi != null)
            battleUi.LogAction("");

        battleController.EndTurn();
    }
}
