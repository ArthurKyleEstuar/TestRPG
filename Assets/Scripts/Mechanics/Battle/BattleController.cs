using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleController : MonoBehaviour
{
    [SerializeField] private static List<BattleCharController> battleControllers = new List<BattleCharController>();
    [SerializeField] private PlayerBattle               playerController;

    private static int currBattleIndex;

    private static BattleController instance;
    private static BattleController Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<BattleController>();

            if (instance == null)
            {
                Debug.LogError("NO BATTLE CONTROLLER");
                return null;
            }

            return instance;
        }
    }

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

    private static void HandleTurn()
    {
        if (battleControllers[currBattleIndex].TeamId == "player" && Instance.playerController != null)
        {
            Instance.playerController.OnTurnStart(battleControllers[currBattleIndex]);
        }
        else
        {
            AiBattle ai = battleControllers[currBattleIndex].GetComponent<AiBattle>();

            if (ai == null) EndTurn();

            ai.ProcessTurn();
        }
    }

    public static void EndTurn()
    {
        currBattleIndex++;

        if (currBattleIndex >= battleControllers.Count)
            currBattleIndex = 0;

        HandleTurn();
    }

    public static List<BattleCharController> GetValidTargets(string currTeamId)
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
