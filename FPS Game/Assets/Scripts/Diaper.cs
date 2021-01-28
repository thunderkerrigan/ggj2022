using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Diaper : MonoBehaviour
{
    PhotonView PV;
    private void OnCollisionEnter(Collision other)
    {
        other.gameObject.GetComponent<IDamageable>()?.TakeDamage(20);
        PhotonNetwork.Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Awake()
    {
        PV = GetComponent<PhotonView>();
    }

    [PunRPC]
    void RPC_Splash(Vector3 hitPosition, Vector3 hitNormal)
    {
        
    }
}
