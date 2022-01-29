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
   public Vector2 moveVal;
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
      if (PV.IsMine)
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
   
   private void FixedUpdate()
   {
      if (!PV.IsMine)
         return;
      transform.Translate(new Vector3(moveVal.x, 0, moveVal.y) * moveSpeed * Time.deltaTime);
   }

   
   // CONTROL MAPPING
   private void OnMove(InputValue value)
   {
      moveVal = value.Get<Vector2>();
   }

   private void OnLightAttack(InputValue value)
   {
      Debug.Log("Light Attack");
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
      transform.position = new Vector3(Random.Range(-75, 75), 0.5f, Random.Range(-75, 75));
      var spawnManager = GameObject.Find("PlayerSpawnManager").GetComponent<SpawnManager>();
      spawnManager.GetSpawnpoint(0);
   }
   
   // CUSTOM FUNCTION
   
   public void SetGroundedState(bool _grounded, float _y)
   {
      Debug.Log("SetGroundedState: " + _grounded);
      currentFloorY = _y;
      grounded = _grounded;
   }
}
