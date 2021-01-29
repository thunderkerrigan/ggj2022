using System;
using System.Collections;
using Doozy.Engine.Progress;
using System.Collections.Generic;
using static System.Net.Mime.MediaTypeNames;
using Doozy.Engine;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private PlayerController _player;

    public Progressor timerDiaper;
    public Progressor timerMine;
   
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        _player.OnCoolDownUpdate += OnCoolDownUpdate; //Abonnement
        
    }

    private void OnDestroy()
    {
       if ((_player))
        {
            _player.OnCoolDownUpdate -= OnCoolDownUpdate;
        }
    }

    private void OnCoolDownUpdate(float[] value)
    {
        timerDiaper.SetValue(value[0]);
        timerDiaper.UpdateProgressTargets();
        timerDiaper.SetValue(value[1]);
        timerDiaper.UpdateProgressTargets();
        print("cooldown update " + value[0]);
       // print("ta mere le cd" + value[1]);

    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
