using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class IsoPlayerController : MonoBehaviour
{
   
   public Vector2 moveVal;
   public float moveSpeed;

   private void Update()
   {
      transform.Translate(new Vector3(moveVal.x, 0, moveVal.y) * moveSpeed * Time.deltaTime);
   }

   private void OnMove(InputValue value)
   {
      moveVal = value.Get<Vector2>();
   }

   private void OnLightAttack(InputAction.CallbackContext context)
   {
      
   }

   private void OnHeavyAttack(InputAction.CallbackContext context)
   {
      
   }

   private void OnDash(InputAction.CallbackContext context)
   {
      
   }

   private void OnJoin(InputAction.CallbackContext context)
   {
      transform.position = new Vector3(Random.Range(-75, 75), 0.5f, Random.Range(-75, 75));
      moveSpeed = 30;
   }
}
