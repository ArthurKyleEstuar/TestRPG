using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private Rigidbody  playerRb;
    [SerializeField] private float      moveSpeed = 5.0f;

    private void Update()
    {
        Vector3 newPos = this.transform.position;

        //TODO Use better input system
        if(Input.GetKey(KeyCode.W))
            newPos = AdjustAxisPoint('y', newPos);

        if (Input.GetKey(KeyCode.S))
            newPos = AdjustAxisPoint('y', newPos, false);

        if (Input.GetKey(KeyCode.A))
            newPos = AdjustAxisPoint('x', newPos, false);

        if (Input.GetKey(KeyCode.D))
            newPos = AdjustAxisPoint('x', newPos);

        playerRb.MovePosition(newPos);
        playerRb.velocity = Vector3.zero;
    }

    private Vector3 AdjustAxisPoint(char axis, Vector3 posToAdjust, bool isPositive = true)
    {
        if (playerRb == null) return Vector3.zero;

        int dir = (isPositive) ? 1 : -1;

        switch(axis)
        {
            case 'x':
                posToAdjust.x += moveSpeed * dir * Time.deltaTime;
                break;

            case 'y':
                posToAdjust.y += moveSpeed * dir * Time.deltaTime;
                break;
        }

        return posToAdjust;
    }
}
