using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DiaperWeapon : Weapon
{
    [SerializeField] Camera cam;
    private bool enable = true;

    PhotonView PV;

    void Awake()
    {
        PV = GetComponent<PhotonView>();
    }

    public override void Use()
    {
        Spawn();
    }

    void Spawn()
    {
        if (enable)
        {
            StartCoroutine(OnCooldown());
            var playerPosition = cam.transform.position;
            var frontPosition = cam.transform.TransformPoint(Vector3.forward * 2);
            var direction = (frontPosition - playerPosition).normalized;
            var diaper = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Diaper"), frontPosition, Quaternion.identity);
            diaper.GetComponent<Rigidbody>().AddForceAtPosition(playerPosition, diaper.transform.position);
            diaper.GetComponent<Rigidbody>().velocity = direction * 30;
        }
        
    }

    IEnumerator OnCooldown()
    {
        enable = false; 
        yield return new WaitForSeconds(3);
        enable = true;
    }
    [PunRPC]
    void RPC_Shoot(Vector3 hitPosition, Vector3 hitNormal)
    {
        // Collider[] colliders = Physics.OverlapSphere(hitPosition, 0.3f);
        // if(colliders.Length != 0)
        // {
        //     GameObject bulletImpactObj = Instantiate(bulletImpactPrefab, hitPosition + hitNormal * 0.001f, Quaternion.LookRotation(hitNormal, Vector3.up) * bulletImpactPrefab.transform.rotation);
        //     Destroy(bulletImpactObj, 10f);
        //     bulletImpactObj.transform.SetParent(colliders[0].transform);
        // }
    }
}
