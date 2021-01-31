using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public abstract class AudioManager : MonoBehaviour{

    protected AudioSource babyAudioSource;

	private void Awake () {
        babyAudioSource = this.gameObject.GetComponent<AudioSource>();
	}

    protected int playClip (List<AudioClip> clipList, int givenIndex = -1) {
        return this.playClip(clipList, babyAudioSource, givenIndex);
    }

    protected int playClip (List<AudioClip> clipList, AudioSource audioSource, int givenIndex = -1) {
        int clipIndex = -1;
        if(clipList != null && clipList.Count > 0 && audioSource != null) {
            clipIndex = 0;
            if(givenIndex == -1 && clipList.Count > 1) { clipIndex = Random.Range(0, clipList.Count); } // /!\ max range is exclusive
            else if (givenIndex != -1) { clipIndex = givenIndex; }


            audioSource.PlayOneShot(clipList[clipIndex]);
		}
        return clipIndex;
    }
}
