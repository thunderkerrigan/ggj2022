using System;
using System.Collections;
using System.Collections.Generic;
using Doozy.Engine.Soundy;
using Photon.Pun;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class IsoPlayerController : MonoBehaviour
{
   public PhotonView PV;
   public GameObject weapon;
   public Vector2 moveVal;
   public Vector2 attackVal;
   public float moveSpeed, dashSpeed, dashCooldownTimer;
   bool grounded, canDash = true;
   private float currentFloorY = -1;
   private bool canAttack = true;
   Rigidbody rb;
   public AudioClip dashSound;
   public AudioClip attackSound;

   public bool canTakeDamage = false;

   [SerializeField] private int healthPoints;

   
   void Awake()
   {
      rb = GetComponent<Rigidbody>();
      canAttack = true;
   }
   
   private void Start()
   {
      if (PhotonNetwork.IsConnected)
      {
         if ( PV.IsMine)
         {
// TODO
         }
         else
         {
            Destroy(GetComponentInChildren<Camera>().gameObject);
            Destroy(rb);
            Destroy(GetComponent<AudioListener>());
         }
      }
   }
   
   private void FixedUpdate()
   {
      if (PhotonNetwork.IsConnected && !PV.IsMine)
         return;
      
      rb.velocity = new Vector3(moveVal.x, 0, moveVal.y) * moveSpeed;
      //transform.Translate(new Vector3(moveVal.x, 0, moveVal.y) * moveSpeed * Time.deltaTime);
   }

   
   // CONTROL MAPPING
   private void OnMove(InputValue value)
   {
      moveVal = value.Get<Vector2>();
   }

   private void OnLightAttack(InputValue value)
   {
      var newVal = value.Get<Vector2>();
      if (!newVal.Equals(Vector2.zero) && canAttack)
      {
         Debug.Log("Light Attack: " + newVal);
         Debug.Log("Light Attack: " + canAttack);
         attackVal = newVal;
         StartCoroutine(triggerAttack());
      }
   }

   private void OnHeavyAttack(InputValue value)
   {
      Debug.Log("Heavy Attack");
   }

   private void OnDash(InputValue value)
   {
      Debug.Log("try to dash");
      if (canDash)
      {
         Debug.Log("dash");
         rb.AddForce(new Vector3(moveVal.x, 0.1f, moveVal.y) * dashSpeed, ForceMode.VelocityChange);
         StartCoroutine(DashCooldown());
      }
   }
   
   protected IEnumerator DashCooldown()
   {
      canDash = false; 
      yield return new WaitForSeconds(dashCooldownTimer);
      canDash = true;
   }

   private void OnJoin(InputValue value)
   {
//      var spawnManager = GameObject.Find("PlayerSpawnManager").GetComponent<SpawnManager>();
  //    transform.position = spawnManager.GetSpawnpoint(0).position;
   }
   
   // CUSTOM FUNCTION
   
   public void SetGroundedState(bool _grounded, float _y)
   {
      currentFloorY = _y;
      grounded = _grounded;
   }

   private IEnumerator triggerAttack()
   {
      canAttack = false;
      Vector3 directionVector3 = weapon.transform.position + new Vector3(attackVal.x, 0, attackVal.y);
      weapon.transform.rotation = Quaternion.LookRotation(new Vector3(attackVal.x, 0, attackVal.y), Vector3.up);
      weapon.GetComponent<WeaponHandler>().TriggerWeapon();
      SoundyManager.Play(audioClip: attackSound , pitch: Random.Range(0.9f, 1.1f));
      yield return new WaitForSeconds(0.5f);
      canAttack = true;
   }

   private void OnTriggerEnter(Collider other) {
      //Debug.Log("PLAYER TRIGGER");
      WeaponHandler otherWeapon = other.GetComponent<WeaponHandler>();
      if (otherWeapon != null && weapon != otherWeapon.transform.parent && canTakeDamage == true) {
         //Debug.Log("DAMAGE !");
      }
   }
   

}
