using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager_Baby : AudioManager{

    [SerializeField] public List<AudioClip> complaints_short;
    [SerializeField] public List<AudioClip> complaints_long;
    [SerializeField] public List<AudioClip> grunt;
    [SerializeField] public List<AudioClip> happy;
    [SerializeField] public List<AudioClip> laugh;
    [SerializeField] public List<AudioClip> playful;
    [SerializeField] public List<AudioClip> relieved;
    [SerializeField] public List<AudioClip> sad;
    [SerializeField] public List<AudioClip> surprised;

    public int playAudioClip (string audioType) {
        return this.playAudioClip(audioType, this.babyAudioSource);
    }

    public int playAudioClip (string audioType, int givenIndex) {
        return this.playAudioClip(audioType, this.babyAudioSource, givenIndex);
    }

    public int playAudioClip (string audioType, AudioSource audioSource, int givenIndex = -1) {
        int result = -1;
        switch (audioType) {
            case "complaints_short":
                result = this.playClip(this.complaints_short, audioSource, givenIndex);
                break;
                
            case "complaints_long":
                result = this.playClip(this.complaints_long, audioSource, givenIndex);
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
}
