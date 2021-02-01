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

    public int PlaySound (string category) {
        return this.PlaySound(category, transform);
    }

    public int PlaySound (string category, int givenIndex) {
        return this.PlaySound(category, transform, givenIndex);
    }

    public int PlaySound (string audioType, Transform position, int givenIndex = -1) {
        int result = -1;
        switch (audioType) {
            case "complaints_short":
                result = this.playClip(this.complaints_short, position, givenIndex);
                break;
                
            case "complaints_long":
                result = this.playClip(this.complaints_long, position, givenIndex);
                break;

            case "grunt":
                result = this.playClip(this.grunt, position, givenIndex);
                break;

            case "happy":
                result = this.playClip(this.happy, position, givenIndex);
                break;

            case "laugh":
                result = this.playClip(this.laugh, position, givenIndex);
                break;

            case "playful":
                result = this.playClip(this.playful, position, givenIndex);
                break;

            case "relieved":
                result = this.playClip(this.relieved, position, givenIndex);
                break;
            
            case "sad":
                result = this.playClip(this.sad, position, givenIndex);
                break;

            case "surprised":
                result = this.playClip(this.surprised, position, givenIndex);
                break;
        }
        return result;
    }
}
