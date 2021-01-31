using System;
using System.Collections;
using NUnit.Framework.Constraints;
using Photon.Pun;
using UnityEngine;

namespace DefaultNamespace
{
    public class MilkPool : MonoBehaviour
    {
        [SerializeField] Animator _animator;
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
        private void Start()
        {
            

        }

        private void OnTriggerEnter(Collider other)
        {
            print("OnTriggerEnter");

            if (enable)
            {
                
     
                if (other.gameObject.tag == "Player" || other.gameObject.tag == "ThrowingWeapon")
                {
                    other.gameObject.GetComponent<IDamageable>()?.GetStunned(5);
                    _animator.enabled = true;
                    _animator.SetTrigger("trapped");
                    IEnumerator death()
                    {
                        yield return new WaitForSeconds(1);
                        PhotonNetwork.Destroy(gameObject);
                    }

                    StartCoroutine(death());
                }
            }
            else
            {
                StartCoroutine(BuildUp());
            }
        }

        public void SetGroundedState(bool _state)
        {
        }

        // Start is called before the first frame update
        void Awake()
        {
            PV = GetComponent<PhotonView>();
            StartCoroutine(BuildUp());
            IEnumerator Wallah()
            {
                yield return new WaitForSeconds(1);
            }

            StartCoroutine(Wallah());
        }

        IEnumerator BuildUp()
        {
            yield return new WaitForSeconds(1);
            //GetComponent<Collider>().isTrigger = false;
            enable = true;
            
        }
    }
}