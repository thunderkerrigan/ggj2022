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

    private void OnCoolDownUpdate(int position, float value)
    {
        if (position == 0)
        {
            timerDiaper.InstantSetProgress(0);
            timerDiaper.AnimationDuration = value;
            timerDiaper.SetProgress(1);
        }

        if (position == 1)
        {
            timerMine.InstantSetProgress(0);
            timerMine.AnimationDuration = value;
            timerMine.SetProgress(1);
        }
    }

}