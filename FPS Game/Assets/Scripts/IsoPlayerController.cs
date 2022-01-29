using System;
using System.Collections;
using System.Collections.Generic;
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
   Rigidbody rb;

   
   void Awake()
   {
      rb = GetComponent<Rigidbody>();
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
      Debug.Log(value.Get<Vector2>());
      attackVal = value.Get<Vector2>();
      Debug.Log("Light Attack");
      triggerAttack();
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
      var spawnManager = GameObject.Find("PlayerSpawnManager").GetComponent<SpawnManager>();
      transform.position = spawnManager.GetSpawnpoint(0).position;
   }
   
   // CUSTOM FUNCTION
   
   public void SetGroundedState(bool _grounded, float _y)
   {
      Debug.Log("SetGroundedState: " + _grounded);
      currentFloorY = _y;
      grounded = _grounded;
   }

   private void triggerAttack()
   {
      Vector3 directionVector3 = weapon.transform.position + new Vector3(attackVal.x, 0, attackVal.y);
      
      Debug.Log("directionVector3: " + directionVector3);
      Debug.Log("Attack val : " + attackVal);
      Debug.Log("transform val : " + weapon.transform.position);

      weapon.transform.rotation = Quaternion.LookRotation(new Vector3(attackVal.x, 0, attackVal.y), Vector3.up);
   }
   

}
