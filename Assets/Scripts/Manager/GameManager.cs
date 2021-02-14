using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : BaseManager<GameManager>
{
    protected override void Start()
    {
        base.Start();

        SceneController.Instance.OpenScene("GameScene");
    }
}
