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

enum EnemyMode {
    Move,
    Attack
}
public class Enemy : MonoBehaviourPunCallbacks
{

    [Tooltip("Aribitrary value for enemy speed")]
    [SerializeField] private int speed;
    [SerializeField] private int damage;

    [Tooltip("Attack cooldown in seconds")]
    [SerializeField] private float attackCooldown;

    private Transform destinationPoint;
    private Garden destinationGarden;

    private NavMeshAgent navMeshAgent;
    private NavMeshObstacle navMeshObstacle;

    private bool isAlive = true;

    private EnemyMode mode = EnemyMode.Move;

    private PhotonView PView;

    private void Start()
    {
        navMeshAgent = this.GetComponent<NavMeshAgent>();
        navMeshAgent.speed = this.speed;

        navMeshObstacle = this.GetComponent<NavMeshObstacle>();

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
        navMeshAgent.enabled = true;
        navMeshObstacle.enabled = false;

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
        if (this.mode == EnemyMode.Move) {
            FindClosestDestination();
        } else {
            // TODO: this should be done only once
          //  Attack(this.destinationGarden);
        }
        
    }

    private void Attack(Garden garden) {
        Debug.Log("ATTACK!");
        garden.Attack(damage: this.damage);

        if (garden.isAlive() == false) {
            this.mode = EnemyMode.Move;
        }
    }

    IEnumerator Attack()

    {
        yield return new WaitForSeconds(this.attackCooldown);

        if (this.isAlive == false) { StopCoroutine(Attack()); yield break; }
        if (this.mode != EnemyMode.Attack) { StopCoroutine(Attack()); yield break; }

        this.Attack(garden: this.destinationGarden);

        StartCoroutine(Attack());
    }

    private void OnTriggerEnter(Collider other) {
        Debug.Log("TRIGGER ENTER " + other.transform.root.gameObject.GetType());
        var garden = other.transform.root.gameObject.GetComponent<Garden>();
        if (garden != null && garden.isAlive() == true && garden == this.destinationGarden) {
            Debug.Log("ET C EST PARTI!");
            this.mode = EnemyMode.Attack;
            navMeshAgent.enabled = false;
            navMeshObstacle.enabled = true;

            StartCoroutine(Attack());
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