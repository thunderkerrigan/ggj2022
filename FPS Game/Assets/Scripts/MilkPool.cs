using System;
using System.Collections;
using NUnit.Framework.Constraints;
using Photon.Pun;
using UnityEngine;

namespace DefaultNamespace
{
    public class MilkPool : MonoBehaviour
    {
        public PlayerController launcherPlayerController;
	    public string launcherAudioType;
	    public int launcherAudioClipIndex = -1;
	    public string targetAudioType;
	    public int targetAudioClipIndex = -1;
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
                    other.gameObject.GetComponent<IDamageable>()?.GetStunned(3);

                    if(other.gameObject.GetComponent<IDamageable>() != null) {
                        PlayerController targetPlayerController =  other.gameObject.GetComponent<PlayerController>();
                        // Play sound on launcher player
                        if (targetPlayerController != launcherPlayerController) { launcherPlayerController.playAudioClip(launcherAudioType, true, launcherAudioClipIndex); }
            
                        // Play sound on target player
                        targetPlayerController?.playAudioClip(targetAudioType, true, targetAudioClipIndex);
					}

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
