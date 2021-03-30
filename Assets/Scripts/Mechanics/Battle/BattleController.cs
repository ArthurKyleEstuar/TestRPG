using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleController : MonoBehaviour
{
    [SerializeField] private static List<BattleCharController> charControllers = new List<BattleCharController>();
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
        charControllers.Clear();

        charControllers.AddRange(FindObjectsOfType<BattleCharController>());

        SortBattlerSpeed();

        HandleTurn();

        //TODO 
        //1. Load battle data
        //2. Sort by stats
        //3. Turn order
    }

    public static void SortBattlerSpeed()
    {
        charControllers.Sort((b1, b2) => b2.Speed.CompareTo(b1.Speed));
    }

    private static void HandleTurn()
    {
        if (charControllers[currBattleIndex].TeamId == "player" 
            && Instance.playerController != null)
        {
            Instance.playerController.OnTurnStart(charControllers[currBattleIndex]);
        }
        else
        {
            AiBattle ai = charControllers[currBattleIndex]
                .GetComponent<AiBattle>();

            if (ai == null) EndTurn();

            ai.ProcessTurn();
        }
    }

    public static void EndTurn()
    {
        currBattleIndex++;

        if (currBattleIndex >= charControllers.Count)
            currBattleIndex = 0;

        HandleTurn();
    }

    public static List<BattleCharController> GetValidTargets(string targetTeamId)
    {
        List<BattleCharController> validTargets = new List<BattleCharController>();

        foreach(BattleCharController bcc in charControllers)
        {
            if (bcc.TeamId == targetTeamId)
                validTargets.Add(bcc);
        }

        return validTargets;
    }
}
