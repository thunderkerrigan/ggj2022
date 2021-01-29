using System.Collections;
using Photon.Pun;
using UnityEngine;

namespace DefaultNamespace
{
    public class MilkPool : MonoBehaviour
    {
        PhotonView PV;
        private bool enable = false;
        private void OnCollisionEnter(Collision other)
        {
            if (enable)
            {
                other.gameObject.GetComponent<IDamageable>()?.TakeDamage(3000);
                PhotonNetwork.Destroy(gameObject);
            }
            
        }

        // Start is called before the first frame update
        void Awake()
        {
            PV = GetComponent<PhotonView>();
            StartCoroutine(BuildUp());
        }

        IEnumerator BuildUp()
        {
            yield return new WaitForSeconds(3);
            enable = true;
        }
    }
}