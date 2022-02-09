using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioOptionsManager : MonoBehaviour
{
    private void Start()
    {
        GetComponent<Slider>().value = AudioListener.volume;
    }

    // Start is called before the first frame update
   public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
    }
}
