using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using Hashtable = ExitGames.Client.Photon.Hashtable;


public class EnemyMove : MonoBehaviourPunCallbacks
{

    [Tooltip("Aribitrary value for enemy speed")]
    [SerializeField] private int speed;

    private Transform destinationPoint;
    private Garden destinationGarden;

    private NavMeshAgent navMeshAgent;

    private bool stop = false;

    private PhotonView PView;

    private void Start()
    {
        navMeshAgent = this.GetComponent<NavMeshAgent>();
        navMeshAgent.speed = this.speed;

        if (this.PView == null)
        {
            this.PView = GetComponent<PhotonView>();

            if (this.PView == null) {
                Debug.LogError("No PhotonView for " + gameObject.name);
            }

            // if (!this.PView.IsMine)
            // {
            //     return;
            // }
        }

        if (navMeshAgent == null)
        {
            Debug.LogError("No navMeshAgent for " + gameObject.name);
        }
        else
        {
            FindClosestDestination();
        }
    }

    private void FindClosestDestination()
    {
        if (navMeshAgent.enabled == false) { return; }

        Garden[] gardens = (Garden[])GameObject.FindObjectsOfType(typeof(Garden));
        Garden closestGarden = null;
        float minDistance = 10000;
        foreach (Garden garden in gardens)
        {
            // Don't go to dead gardens
            if (garden.isAlive() == false) { continue; }

            float distance = Vector3.Distance(garden.transform.position, this.transform.position);
            if (closestGarden == null)
            {
                closestGarden = garden;
                minDistance = distance;
            }
            else if (distance < minDistance)
            {
                closestGarden = garden;
                minDistance = distance;
            }
        }

        this.destinationGarden = closestGarden;
        this.destinationPoint = closestGarden.transform;
        navMeshAgent.SetDestination(closestGarden.transform.position);
    }

    private void Update()
    {
        // if (PView.IsMine == false) { return; }
        if (stop == false) {
            FindClosestDestination();
        } else {
            navMeshAgent.SetDestination(this.transform.position);
        }
        
    }

    private void OnTriggerEnter(Collider other) {
        Debug.Log("TRIGGER ENTER " + other.transform.root.gameObject.GetType());
        var garden = other.transform.root.gameObject.GetComponent<Garden>();
        if (garden != null && garden.isAlive() == true) {
            Debug.Log("ET C EST PARTI!");
            stop = true;
        }
    }

    /// <summary>
    /// OnCollisionEnter is called when this collider/rigidbody has begun
    /// touching another rigidbody/collider.
    /// </summary>
    /// <param name="other">The Collision data associated with this collision.</param>
    void OnCollisionEnter(Collision other)
    {
        Debug.Log("COLLISION ENTER " + other.gameObject.name);
    }
}