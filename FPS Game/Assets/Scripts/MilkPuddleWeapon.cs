using System.Collections;
using System.IO;
using Photon.Pun;
using UnityEngine;

namespace DefaultNamespace
{
    public class MilkPuddleWeapon : Weapon
    {
        [SerializeField] Camera cam;
        PhotonView PV;

        void Awake()
        {
            PV = GetComponent<PhotonView>();
            nextAttack = Time.time + cooldown;
            cooldown = 15f;
            enable = true;
        }

        public override void Use()
        {
            Spawn();
        }

        void Spawn()
        {
            if (enable)
            {
                var groundPosition = new Vector3(transform.position.x, -1, transform.position.z);
                PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "MilkPool"), groundPosition, Quaternion.identity);
                StartCoroutine(OnCooldown());
            }
            
        }

        IEnumerator OnCooldown()
        {
            enable = false; 
            yield return new WaitForSeconds(cooldown);
            enable = true;
        }
    }
}