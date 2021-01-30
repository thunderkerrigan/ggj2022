﻿using System;
using System.Collections;
using NUnit.Framework.Constraints;
using Photon.Pun;
using UnityEngine;

namespace DefaultNamespace
{
    public class MilkPool : MonoBehaviour
    {
        PhotonView PV;
        private bool fusing = false;
        private bool enable = false;

        /*
        private void OnCollisionEnter(Collision other)
        {
            if (enable)
            {
                other.gameObject.GetComponent<IDamageable>()?.TakeDamage(3000);
                if (other.gameObject.tag == "Player" || other.gameObject.tag == "ThrowingWeapon")
                {
                    PhotonNetwork.Destroy(gameObject);
                }
            }
        }
*/
        private void OnTriggerEnter(Collider other)
        {
            print("OnTriggerEnter");

            if (enable)
            {
                other.gameObject.GetComponent<IDamageable>()?.TakeDamage(3000);
                if (other.gameObject.tag == "Player" || other.gameObject.tag == "ThrowingWeapon")
                {
                    PhotonNetwork.Destroy(gameObject);
                }
            }
            else
            {
                StartCoroutine(BuildUp());
            }

            //GetComponent<Collider>().isTrigger = false;
            // GetComponent<Rigidbody>().useGravity = false;
            //GetComponent<Rigidbody>().isKinematic = true;
        }

        public void SetGroundedState(bool _state)
        {
        }

        // Start is called before the first frame update
        void Awake()
        {
            PV = GetComponent<PhotonView>();
            StartCoroutine(BuildUp());
            //GetComponent<Rigidbody>().useGravity = true;
        }

        IEnumerator BuildUp()
        {
            yield return new WaitForSeconds(1);
            //GetComponent<Collider>().isTrigger = false;
            enable = true;
            print("isEnable");
        }
    }
}