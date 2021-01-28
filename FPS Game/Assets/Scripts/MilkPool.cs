using Photon.Pun;
using UnityEngine;

namespace DefaultNamespace
{
    public class MilkPool : MonoBehaviour
    {
        PhotonView PV;
        private void OnCollisionEnter(Collision other)
        {
            other.gameObject.GetComponent<IDamageable>()?.TakeDamage(3000);
            PhotonNetwork.Destroy(gameObject);
        }

        // Start is called before the first frame update
        void Awake()
        {
            PV = GetComponent<PhotonView>();
        }
    }
}