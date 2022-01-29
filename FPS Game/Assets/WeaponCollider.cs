using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCollider : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("WeaponHandler: OnTriggerEnter");
        Debug.Log("WeaponHandler: other.tag: " + other.tag);
            if(other.gameObject.GetComponent<IDamageable>() != null) {
                Debug.Log("WeaponHandler: OnTriggerEnter: IDamageable");
                other.gameObject.GetComponent<IDamageable>().TakeDamage(1);
            }
            else if( other.gameObject.GetComponentInParent<IDamageable>() != null) {
                Debug.Log("WeaponHandler: OnTriggerEnter: IDamageable");
                other.gameObject.GetComponentInParent<IDamageable>().TakeDamage(1);
            }
           
    }
}
