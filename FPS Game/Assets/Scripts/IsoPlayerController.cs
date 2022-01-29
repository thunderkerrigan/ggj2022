using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class IsoPlayerController : MonoBehaviour
{
   
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

   private void OnJoin(InputAction.CallbackContext context)
   {
      transform.position = new Vector3(Random.Range(-75, 75), 0.5f, Random.Range(-75, 75));
      moveSpeed = 30;
   }
   
   // CUSTOM FUNCTION
   
   public void SetGroundedState(bool _grounded, float _y)
   {
      Debug.Log("SetGroundedState: " + _grounded);
      currentFloorY = _y;
      grounded = _grounded;
   }
}
