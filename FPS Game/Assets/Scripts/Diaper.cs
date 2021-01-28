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
        if (other.gameObject.tag == "Player")
        {
            PV.RPC("RPC_Shoot", RpcTarget.All, other.GetContact(0).point, other.GetContact(0).normal);
        }
        PhotonNetwork.Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
