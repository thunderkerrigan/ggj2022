using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    private float timeRemaining;
    private bool _timerIsRunning = false;

    private bool _timerFinished = false;

   // public Text timeText;

    public bool isTimerFinished() {
        return this._timerFinished;
    }

    public bool isTimerRunning() {
        return this._timerIsRunning;
    }

    public void startTimer(float duration) {
        this.timeRemaining = duration;
        this._timerIsRunning = true;
        this._timerFinished = false;
    }

    public void stop() {
        this._timerIsRunning = false;
        this._timerFinished = true;
    }

    void Update()
    {
        if (_timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
            }
            else
            {
                Debug.Log("Time has run out!");
                timeRemaining = 0;
                _timerIsRunning = false;
                _timerFinished = true;
            }
        }
    }

    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;

        float minutes = Mathf.FloorToInt(timeToDisplay / 60); 
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

       // timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}