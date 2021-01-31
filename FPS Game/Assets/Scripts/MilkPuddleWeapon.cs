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
            cooldown = 2f;
            enable = true;
        }

        public override float Use()
        {
           return Spawn();
        }

        float Spawn()
        {
            if (enable)
            {
                nextAttack = Time.time + cooldown * 1000;
                var playerPosition = cam.transform.position;
                var frontPosition = cam.transform.TransformPoint(Vector3.forward * 3);
                
                var direction = (frontPosition - playerPosition).normalized;
                var puddle = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "MilkPool"), frontPosition, Random.rotation);
                puddle.GetComponent<Rigidbody>().AddForceAtPosition(playerPosition, puddle.transform.position + Vector3.up);
                puddle.GetComponent<Rigidbody>().velocity = direction * 3;
                var randomTorque = Random.insideUnitSphere;
                puddle.GetComponent<Rigidbody>().AddTorque(randomTorque*7599999);
                StartCoroutine(OnCooldown());
                return cooldown;
            }
            else
            {
                return -1;
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