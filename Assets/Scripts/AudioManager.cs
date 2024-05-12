using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public Sound[] musicSounds, sfxSounds;
    public AudioSource musicSource, sfxSource;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    public void Start() {
        PlayMusic("Cymatics - Interstellar");
    }


    public void PlayMusic(string name) {
        Sound mySound = Array.Find(musicSounds, x => x.name == name);

        if (mySound == null) {
            Debug.Log("Som não encontrado!");
        } else {
            musicSource.clip = mySound.clip;
            musicSource.Play();
        }
    }


    public void PlaySFX(string name) {
        Sound mySound = Array.Find(sfxSounds, x => x.name == name);

        if (mySound == null) {
            Debug.Log("Som não encontrado!");
        } else {
            sfxSource.PlayOneShot(mySound.clip);
        }
    }


}
