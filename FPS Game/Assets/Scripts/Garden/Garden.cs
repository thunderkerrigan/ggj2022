using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

/// Potager
public class Garden : MonoBehaviour
{

    [SerializeField] private int hitPoints;


    private void Start() {
        this.GetComponentInChildren<MeshRenderer>().enabled = false;
    }

    public bool isAlive()
    {
        return (hitPoints > 0);
    }

    public void TakeDamage(int damage)
    {
        this.hitPoints -= damage;
    }

    private void OnTriggerEnter(Collider other)
    {
        var enemy = other.gameObject.GetComponentInParent<Enemy>();
        if (enemy != null) {
            // TODO: here garden is in Danger
        }
    }
}