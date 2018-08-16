using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    public Sound[] sounds;

	// Use this for initialization
	void Awake () {
		foreach(Sound s in sounds) {
            s.source = gameObject.AddComponent<AudioSource>();

            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.playOnAwake = false;
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Play(string name) {
       Sound sound = Array.Find(sounds, sounds => sounds.name == name);
        sound.source.Play();
    }
}
