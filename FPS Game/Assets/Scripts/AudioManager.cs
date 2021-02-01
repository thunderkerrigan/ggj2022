using System.Collections;
using System.Collections.Generic;
using Doozy.Engine.Soundy;
using UnityEngine;

public abstract class AudioManager : MonoBehaviour{

    
    protected int playClip (List<AudioClip> clipList, int givenIndex = -1) {
        return this.playClip(clipList, transform.position, givenIndex);
    }

    protected int playClip (List<AudioClip> clipList, Vector3 position, int givenIndex = -1) {
        int clipIndex = -1;
        if(clipList != null && clipList.Count > 0) {
            clipIndex = 0;
            if(givenIndex == -1 && clipList.Count > 1) { clipIndex = Random.Range(0, clipList.Count); } // /!\ max range is exclusive
            else if (givenIndex != -1) { clipIndex = givenIndex; }

            SoundyManager.Play(clipList[clipIndex], position);

		}
        return clipIndex;
    }
}
