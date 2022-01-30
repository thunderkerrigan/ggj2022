using System;
using System.Collections;
using System.Collections.Generic;
using Doozy.Engine.Soundy;
using Photon.Pun;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class IsoPlayerController : MonoBehaviour
{
   public Animator characterAnimator;
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

   public bool isAlive = true;

   
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
             Destroy(GetComponent<AudioListener>());
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
      characterAnimator.SetFloat("x", moveVal.x);

	  
	  if(moveVal.x == 1 && moveVal.y == 0){
	  	GetComponentInChildren<Animator>().Play("Bear_walking_right");
	  }
	  
	  else if(moveVal.x == 0 && moveVal.y == -1){
	  	GetComponentInChildren<Animator>().Play("Bear_walking_front");
	  }
	  
	  else if(moveVal.x == -1 && moveVal.y == 0){
	  	GetComponentInChildren<Animator>().Play("Bear_walking_left");
	  }
	  
	  else if(moveVal.x == 0 && moveVal.y == 1){
	  	GetComponentInChildren<Animator>().Play("Bear_walking_back");
	  }
	  else{
	  	GetComponentInChildren<Animator>().Play("Bear_idle");
	  }
	  
   }

   private void OnLightAttack(InputValue value)
   {
      var newVal = value.Get<Vector2>();
      characterAnimator.SetFloat("x", newVal.x);
      if (!newVal.Equals(Vector2.zero) && canAttack)
      {
		  
		  
         attackVal = newVal;
         StartCoroutine(triggerAttack());
		 
   	  if(newVal.x == 1 && newVal.y == 0){
   	  	GetComponentInChildren<Animator>().Play("Bear_attacking_right");
   	  }
	  
   	  else if(newVal.x == 0 && newVal.y == -1){
   	  	GetComponentInChildren<Animator>().Play("Bear_attacking_front");
   	  }
	  
   	  else if(newVal.x == -1 && newVal.y == 0){
   	  	GetComponentInChildren<Animator>().Play("Bear_attacking_left");
   	  }
	  
   	  else if(newVal.x == 0 && newVal.y == 1){
   	  	GetComponentInChildren<Animator>().Play("Bear_attacking_back");
   	  }
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
   SoundyManager.Play(attackSound, transform);
      yield return new WaitForSeconds(0.5f);
      canAttack = true;
   }

   private void OnTriggerEnter(Collider other) {
      Debug.Log("PLAYER TRIGGER");
      WeaponHandler otherWeapon = other.GetComponentInParent<WeaponHandler>();
      if (otherWeapon != null && weapon != otherWeapon.transform.parent && canTakeDamage == true) {
         this.TakeDamageFromPlayer(weaponHandler: otherWeapon);
      }
   }
   
   private void TakeDamageFromPlayer(WeaponHandler weaponHandler) {
      healthPoints -= weaponHandler.damage;
      // TODO: BLOOD
      if (healthPoints <= 0 && isAlive == true) {
         // EXPLODE

         this.isAlive = false;
         Debug.Log("DEAD");
      }
   }

   public bool isCurrentPlayer() {
      return PV.IsMine;
   }

}
