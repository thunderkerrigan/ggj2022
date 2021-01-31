using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager_Baby : MonoBehaviour{

    private AudioSource babyAudioSource;
    [SerializeField] public List<AudioClip> complaints;
    [SerializeField] public List<AudioClip> grunt;
    [SerializeField] public List<AudioClip> happy;
    [SerializeField] public List<AudioClip> laugh;
    [SerializeField] public List<AudioClip> playful;
    [SerializeField] public List<AudioClip> relieved;
    [SerializeField] public List<AudioClip> sad;
    [SerializeField] public List<AudioClip> surprised;

	private void Awake () {
        babyAudioSource = this.gameObject.GetComponent<AudioSource>();
	}

    public int playAudioClip (string audioType) {
        return this.playAudioClip(audioType, this.babyAudioSource);
    }

    public int playAudioClip (string audioType, int givenIndex) {
        return this.playAudioClip(audioType, this.babyAudioSource, givenIndex);
    }

    public int playAudioClip (string audioType, AudioSource audioSource, int givenIndex = -1) {
        int result = -1;
        switch (audioType) {
            case "complaints":
                result = this.playClip(this.complaints, audioSource, givenIndex);
                break;

            case "grunt":
                result = this.playClip(this.grunt, audioSource, givenIndex);
                break;

            case "happy":
                result = this.playClip(this.happy, audioSource, givenIndex);
                break;

            case "laugh":
                result = this.playClip(this.laugh, audioSource, givenIndex);
                break;

            case "playful":
                result = this.playClip(this.playful, audioSource, givenIndex);
                break;

            case "relieved":
                result = this.playClip(this.relieved, audioSource, givenIndex);
                break;
            
            case "sad":
                result = this.playClip(this.sad, audioSource, givenIndex);
                break;

            case "surprised":
                result = this.playClip(this.surprised, audioSource, givenIndex);
                break;
        }
        return result;
    }

    private int playClip (List<AudioClip> clipList, AudioSource audioSource, int givenIndex = -1) {
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
