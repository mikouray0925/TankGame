using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header ("Settings")]
    static float effectVolume = 1.0f;

    [Header ("Components")]
    [SerializeField] AudioSource musicPlayer;
    [SerializeField] AudioSource uiPlayer;

    [Header ("Music")]
    [SerializeField] AudioClip mainMenuMusic;
    [SerializeField] AudioClip battleMusic;

    [Header ("UI")]
    [SerializeField] AudioClip clickButtonSFX;

    void Awake() {

    }

    void Update() {
        
    }

    public void PlayMainMenuMusic() {
        musicPlayer.clip = mainMenuMusic;
        musicPlayer.Play();
    }

    public void PlayClickButtonSFX() {
        uiPlayer.PlayOneShot(clickButtonSFX);
    }

    public static float EffectVolume {
        get {
            return effectVolume;
        }
        set {
            effectVolume = Mathf.Clamp(value, 0f, 1f);
        }
    }

    public float MusicVolume {
        get {
            return musicPlayer.volume;
        }
        set {
            musicPlayer.volume = Mathf.Clamp(value, 0f, 1f);
        }
    }

    public float UiVolume {
        get {
            return uiPlayer.volume;
        }
        set {
            uiPlayer.volume = Mathf.Clamp(value, 0f, 1f);
        }
    }
}
