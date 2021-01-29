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
            nextAttack = Time.time;
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
                nextAttack = Time.time + cooldown * 1000;
                PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "MilkPool"), transform.position, Quaternion.identity);
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