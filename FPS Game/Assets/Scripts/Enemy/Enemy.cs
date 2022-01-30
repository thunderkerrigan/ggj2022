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

enum EnemyMode
{
    Move,
    Attack
}
public class Enemy : MonoBehaviourPunCallbacks, IDamageable
{

    [Tooltip("Aribitrary value for enemy speed")]
    [SerializeField] private int speed;
    [SerializeField] private int damage;
[SerializeField] private Collider attackCollider;
    [Tooltip("Attack cooldown in seconds")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private SpriteRenderer bloodPool;
    [SerializeField] private Sprite[]  bloodSprites;

    private Transform destinationPoint;
    private Garden destinationGarden;

    private NavMeshAgent navMeshAgent;
    private NavMeshObstacle navMeshObstacle;

    private bool isAlive = true;

    private EnemyMode mode = EnemyMode.Move;

    private PhotonView PView;

    private Animator animator;

    private void Start()
    {
        navMeshAgent = this.GetComponent<NavMeshAgent>();
        navMeshAgent.speed = this.speed;

        navMeshObstacle = this.GetComponent<NavMeshObstacle>();

        animator = this.GetComponentInChildren<Animator>();

        if (this.PView == null)
        {
            this.PView = GetComponent<PhotonView>();

            if (this.PView == null)
            {
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
        animator.SetFloat("x", Random.Range(0, 100));

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
        if (closestGarden != null)
        {
            this.destinationPoint = closestGarden.transform;
            navMeshAgent.SetDestination(closestGarden.transform.position);
        }

    }

    private void FixedUpdate()
    {
        if (!PView.IsMine)
        {
            return;
        }

        if (this.isAlive == false)
        {
            return;
        }
         if (this.mode == EnemyMode.Move && this.isAlive == true) {
            FindClosestDestination();

            var direction = this.destinationPoint.transform.position - this.transform.position;

            var look = Quaternion.LookRotation(direction, Vector3.up).eulerAngles;

            if (look.y < 45 && look.y > 0 || look.y <= 360 && look.y >= 315)
            {
                animator.Play("Rabbit_walking_back");
            }
            else if (look.y >= 45 && look.y < 135)
            {
                animator.Play("Rabbit_walking_right");
            }
            else if (look.y >= 135 && look.y < 225)
            {
                animator.Play("Rabbit_walking_front");
            }
            else if (look.y >= 225 && look.y < 315)
            {
                animator.Play("Rabbit_walking_left");
            }
        }

        this.gameObject.transform.eulerAngles = new Vector3(0, 0, 0);
    }

    private void Attack(Garden garden)
    {
        garden.TakeDamage(damage: this.damage);

        if (garden.isAlive() == false)
        {
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

    private void OnTriggerEnter(Collider other)
    {
        var garden = other.transform.gameObject.GetComponent<Garden>();
        if (garden != null && garden.isAlive() == true && garden == this.destinationGarden)
        {
            this.mode = EnemyMode.Attack;
            //navMeshAgent.enabled = false;
            navMeshObstacle.enabled = true;

            StartCoroutine(Attack());
        }
    }

    public void TakeDamage(float damage)
    {
        photonView.RPC("RPC_TakeDamage", RpcTarget.All, damage);
       
    }
   
    [PunRPC]
    void RPC_TakeDamage(float damage)
    {
        //if (!photonView.IsMine)
           // return;
        
        animator.SetFloat("x", Random.Range(0, 100));

        Debug.Log("Enemy " + this.gameObject.name + " took " + damage + " damage");
        this.isAlive = false;
        navMeshAgent.enabled = false;
        StopCoroutine(Attack());
        animator.Play("Rabbit_dead");
        bloodPool.sprite = bloodSprites[Random.Range(0, bloodSprites.Length)];
        this.gameObject.GetComponent<Rigidbody>().isKinematic = true;
        attackCollider.enabled = false;
        //this.gameObject.GetComponent<Collider>().enabled = false;
        StartCoroutine(destroyAfter(60));
    }
    
    private IEnumerator destroyAfter(float time)
    {
        yield return new WaitForSeconds(time);
        PhotonNetwork.Destroy(this.gameObject);
    }
}