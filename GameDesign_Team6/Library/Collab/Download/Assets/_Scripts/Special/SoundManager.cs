using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** Touched by: Matt Bryan **/

public class SoundManager : MonoBehaviour {
	public static SoundManager instance = null;

	public AudioSource musicSource;
	public AudioClip[] clips = new AudioClip[4];

	// Use this for initialization
	void Start () {
		if (instance == null) {
            instance = this;
		}
        else if (instance != this) {
            Destroy (gameObject);
            DontDestroyOnLoad (gameObject);
        }

        musicSource.clip = randomClip();
        musicSource.Play();
	}

	//This seems to be throwing an error once in a while? - Allison
	private AudioClip randomClip() {
		int randomNum = Random.Range(0, 4);
		return clips[randomNum];
	}
}
