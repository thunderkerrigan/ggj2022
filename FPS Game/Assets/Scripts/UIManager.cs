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
        // StartCoroutine(test());

    }

    private void OnDestroy()
    {
        if ((_player))
        {
            _player.OnCoolDownUpdate -= OnCoolDownUpdate;
        }
    }

    IEnumerator test()
    {
        while (true)
        {
            for (float i = 0; i < 1f; i += 0.1f)
            {
                timerDiaper.SetProgress(i);
                yield return new WaitForFixedUpdate();
            }
        }
    }

    private void OnCoolDownUpdate(int position, float value)
    {
        StartCoroutine(coolDownProgress(position, value));
    }

    // Update is called once per frame
    IEnumerator coolDownProgress(int position, float value)
    {
        var tick = 0.025f;
        for (float i = 0; i < value; i += tick)
        {
            if (position == 0)
            {
                timerDiaper.SetProgress(i / value);
            }

            if (position == 1)
            {
                timerMine.SetProgress(i / value);
            }

            yield return new WaitForSeconds(tick);
        }
    }
}
