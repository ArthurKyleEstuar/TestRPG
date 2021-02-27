using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiBattle : MonoBehaviour
{
    [SerializeField] private BattleCharController   aiData;
    [SerializeField] private BattleOptionUI         battleUi;

    private void Start()
    {
        aiData = GetComponent<BattleCharController>();
        battleUi = FindObjectOfType<BattleOptionUI>();
    }

    public void ProcessTurn()
    {
        if (BattleOptionUI.Instance != null)
            BattleOptionUI.LogAction(aiData.CharName + " is thinking...");

        StartCoroutine(DelayedEndTurnCR());
    }

    private IEnumerator DelayedEndTurnCR()
    {
        yield return new WaitForSeconds(1);

        if (BattleOptionUI.Instance != null)
            BattleOptionUI.LogAction("");

        BattleController.EndTurn();
    }
}
