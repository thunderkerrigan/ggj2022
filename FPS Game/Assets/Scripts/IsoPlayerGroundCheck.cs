using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsoPlayerGroundCheck : MonoBehaviour
{
    IsoPlayerController playerController;
    private float closestY = -300;
    void Awake()
    {
        playerController = GetComponentInParent<IsoPlayerController>();
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == playerController.gameObject)
            return;
        closestY = other.ClosestPoint(transform.position).y;
        playerController.SetGroundedState(true, closestY);
    }

    void OnTriggerExit(Collider other)
    {
        if(other.gameObject == playerController.gameObject)
            return;

        playerController.SetGroundedState(false, -300);
    }

    void OnTriggerStay(Collider other)
    {
        if(other.gameObject == playerController.gameObject)
            return;
        playerController.SetGroundedState(true, closestY);
    }
}