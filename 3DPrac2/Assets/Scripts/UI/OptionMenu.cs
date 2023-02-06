using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionMenu : MonoBehaviour
{
    [SerializeField] AudioManager audioManager;
    
    [Header ("Sliders")]
    [SerializeField] Slider musicVolumeSlider;
    [SerializeField] Slider effectVolumeSlider;
    [SerializeField] Slider uiVolumeSlider;

    void Awake() {
    }

    void Update() {
        
    }

    public void Show() {
        gameObject.SetActive(true);
        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        musicVolumeSlider.value = audioManager.MusicVolume;
        effectVolumeSlider.value = AudioManager.EffectVolume;
        uiVolumeSlider.value = audioManager.UiVolume;
    }

    public void Hide() {
        gameObject.SetActive(false);
        Time.timeScale = 1f;

        if (GameManager.isPlaying) {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void UpdateMusicVolume() {
        audioManager.MusicVolume = musicVolumeSlider.value;
    }

    public void UpdateEffectVolume() {
        AudioManager.EffectVolume = effectVolumeSlider.value;
    }

    public void UpdateUiVolume() {
        audioManager.UiVolume = uiVolumeSlider.value;
    }
}
