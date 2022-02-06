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
public delegate void PotagerDeathHandler();

public class Garden : MonoBehaviour
{
    [SerializeField] private int hitPoints;
    public event PotagerDeathHandler OnPotagerdeath;

    private void Start()
    {
        this.GetComponentInChildren<MeshRenderer>().enabled = false;
    }

    public bool isAlive()
    {
        return (hitPoints > 0);
    }

    public void TakeDamage(int damage)
    {
        this.hitPoints -= damage;
        if (this.hitPoints <= 0 && OnPotagerdeath != null)
        {
            OnPotagerdeath();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var enemy = other.gameObject.GetComponentInParent<Enemy>();
        if (enemy != null)
        {
            // TODO: here garden is in Danger
        }
    }
}