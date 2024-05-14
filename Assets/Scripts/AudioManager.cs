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
        PlayMusic("Tema"); //Caso ache uma música com este nome, ela começa a tocar assim que o jogo carregar.
    }


    public void PlayMusic(string name) {
        Sound mySound = Array.Find(musicSounds, x => x.name == name);

        if (mySound == null) {
            Debug.Log("Som não encontrado!");
        } else {
            musicSource.clip = mySound.clip;
            musicSource.Play();
        }
        //Caso a música esteja no músicSounds (do tipo Sounds[] que pode ser acessado diretamente pela Unity), ela começará a tocar. 
        //Caso não esteja, uma mensagem de 'Som não encontrado' será exibida no console.
    }


    public void PlaySFX(string name) {
        Sound mySound = Array.Find(sfxSounds, x => x.name == name);

        if (mySound == null) {
            Debug.Log("Som não encontrado!");
        } else {
            sfxSource.PlayOneShot(mySound.clip);
        }
        //A mesma lógica do PlayMusic() é aplicada aqui, mas para os SFX.
    }


}
