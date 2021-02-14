using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFollow : MonoBehaviour
{
    [SerializeField] private Transform  objectToFollow;
    [SerializeField] private Vector3    offset;
    [SerializeField] private bool       followX;
    [SerializeField] private bool       followY;
    [SerializeField] private bool       followZ;
   
    private void Update()
    {
        Vector3 newPos = this.transform.position;

        if (followX)
            newPos.x = objectToFollow.position.x + offset.x;

        if(followY)
            newPos.y = objectToFollow.position.y + offset.y;

        if(followZ)
            newPos.z = objectToFollow.position.z + offset.z;

        this.transform.position = newPos;
    }

}
