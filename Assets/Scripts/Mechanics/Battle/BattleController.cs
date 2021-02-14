using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleController : MonoBehaviour
{
    [SerializeField] private List<BattleCharController> battleControllers = new List<BattleCharController>();
    [SerializeField] private PlayerBattle               playerController;

    private int currBattleIndex;

    private void Start()
    {
        battleControllers.Clear();

        battleControllers.AddRange(FindObjectsOfType<BattleCharController>());

        HandleTurn();
        //TODO 
        //1. Load battle data
        //2. Sort by stats
        //3. Turn order
    }

    private void HandleTurn()
    {
        if (battleControllers[currBattleIndex].TeamId == "player" && playerController != null)
        {
            playerController.OnTurnStart(battleControllers[currBattleIndex]);
        }
        else
        {
            AiBattle ai = battleControllers[currBattleIndex].GetComponent<AiBattle>();

            if (ai == null) EndTurn();

            ai.ProcessTurn();
        }
    }

    public void EndTurn()
    {
        currBattleIndex++;

        if (currBattleIndex >= battleControllers.Count)
            currBattleIndex = 0;

        HandleTurn();
    }

    public List<BattleCharController> GetValidTargets(string currTeamId)
    {
        List<BattleCharController> validTargets = new List<BattleCharController>();

        foreach(BattleCharController bcc in battleControllers)
        {
            if (bcc.TeamId != currTeamId)
                validTargets.Add(bcc);
        }

        return validTargets;
    }
}
