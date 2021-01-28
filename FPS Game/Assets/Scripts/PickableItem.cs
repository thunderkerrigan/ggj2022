using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PickableItem : MonoBehaviour
{
    
    PhotonView PV;
    // Start is called before the first frame update
    void Awake()
    {
        PV = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
