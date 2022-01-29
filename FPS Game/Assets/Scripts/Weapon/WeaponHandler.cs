using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class WeaponHandler : MonoBehaviour
{
    public CapsuleCollider weaponCollider;
    public SpriteRenderer blowSprite;
    public float enableColliderTime;
    public float disableColliderTime;
    // Start is called before the first frame update
    public void TriggerWeapon()
    {
        StartCoroutine(TriggerWeaponCoroutine());
    }
    
    private IEnumerator TriggerWeaponCoroutine()
    {
        yield return new WaitForSeconds(enableColliderTime);
        weaponCollider.enabled = true;
        blowSprite.enabled = true;
        yield return new WaitForSeconds(disableColliderTime);
        weaponCollider.enabled = false;
        blowSprite.enabled = false;
    }

}
