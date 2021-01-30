using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DiaperWeapon : Weapon
{
    [SerializeField] Camera cam;
    PhotonView PV;

    void Awake()
    {
        PV = GetComponent<PhotonView>();
        nextAttack = Time.time;
        cooldown = .5f;
        enable = true;
    }

    public void setCooldown(float newCooldown) {
        print("newCooldown" + newCooldown);
        nextAttack = Time.time;
        cooldown = newCooldown;
        enable = true;
    }

    public void resetCooldown()
    {
        nextAttack = Time.time;
        cooldown = 1.5f;
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
            StartCoroutine(OnCooldown());
            nextAttack = Time.time + cooldown * 1000;
            var playerPosition = cam.transform.position;
            var frontPosition = cam.transform.TransformPoint(Vector3.forward * 2);
            var direction = (frontPosition - playerPosition).normalized;
            var diaper = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Diaper"), frontPosition, Quaternion.identity);
            diaper.GetComponent<Rigidbody>().AddForceAtPosition(playerPosition, diaper.transform.position);
            diaper.GetComponent<Rigidbody>().velocity = direction * 30;
            var randomTorque = Random.insideUnitSphere;
            diaper.GetComponent<Rigidbody>().AddTorque(randomTorque*75);
            return cooldown;
        }
        else
        {
            return -1;
        }
    }
}
