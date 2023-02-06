using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {  

    [SerializeField] GameObject corePrefab;
    [SerializeField] GameObject core;

    [SerializeField] GameManager gameManager;
    [SerializeField] AudioManager audioManager;
    [SerializeField] OptionMenu optionMenu;

    [SerializeField] Button playButton;
    [SerializeField] Button optionButton;
    [SerializeField] Button quitButton;

    void Start() {
        core = GameObject.Find("Core(Clone)");
        if (!core) core = Instantiate(corePrefab);

        gameManager  = core.transform.Find("GameManager").GetComponent<GameManager>();
        audioManager = core.transform.Find("AudioManager").GetComponent<AudioManager>();
        optionMenu   = core.transform.Find("OptionMenuCanvas").GetComponent<OptionMenu>();

        audioManager.PlayMainMenuMusic();

        playButton.onClick.AddListener(delegate {
            audioManager.PlayClickButtonSFX();
            gameManager.playGame("Level0");
        });

        optionButton.onClick.AddListener(delegate {
            audioManager.PlayClickButtonSFX();
            optionMenu.Show();
        });

        quitButton.onClick.AddListener(delegate {
            audioManager.PlayClickButtonSFX();
            Application.Quit();
        });
    }

    void Update() {
        
    }
}
