using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public enum GardenType
{
    Leek,
    Onion,
    Beet,
    Carrot
}

public enum GardenRow
{
    top,
    middle,
    bottom
}

/// Potager
public delegate void PotagerDeathHandler();
public delegate void PotagerDamageHandler(int lifePoints);


public class Garden : MonoBehaviour
{
    [SerializeField] private int hitPoints;
    private List<GardenTile> gardenTiles;
    [SerializeField] private List<StateGarden> stateGardens;
    [SerializeField] private GardenType gardenType;
    public event PotagerDeathHandler OnPotagerdeath;
    public event PotagerDamageHandler OnPotagerdamage;

    private void Awake()
    {
        gardenTiles = GetComponentsInChildren<GardenTile>().ToList();
        var gardenTypes = stateGardens.FindAll(garden => garden.type == gardenType);
        gardenTiles.ForEach(tile =>
        {
            var top = gardenTypes.Find(garden => garden.row == GardenRow.top);
            var middle = gardenTypes.Find(garden => garden.row == GardenRow.middle);
            var bottom = gardenTypes.Find(garden => garden.row == GardenRow.bottom);
            tile.SetGarden(this, hitPoints, top, middle, bottom);
        });
    }

    public bool isAlive()
    {
        return (hitPoints > 0);
    }

    public void TakeDamage(int damage)
    {
        this.hitPoints -= damage;
        if (OnPotagerdamage != null)
        {
            OnPotagerdamage(hitPoints);
        }
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