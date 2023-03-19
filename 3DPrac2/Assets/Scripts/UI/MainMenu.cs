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

    [SerializeField] GameObject playButton;
    [SerializeField] GameObject optionButton;
    [SerializeField] GameObject quitButton;

    private bool onShrink = false;

    void Start() {
        core = GameObject.Find("Core(Clone)");
        if (!core) core = Instantiate(corePrefab);

        gameManager  = core.transform.Find("GameManager").GetComponent<GameManager>();
        audioManager = core.transform.Find("AudioManager").GetComponent<AudioManager>();
        optionMenu   = core.transform.Find("OptionMenuCanvas").GetComponent<OptionMenu>();

        audioManager.PlayMainMenuMusic();
                    

        playButton.GetComponent<Button>().onClick.AddListener(delegate {
            audioManager.PlayClickButtonSFX();
            StartCoroutine(shrinkAllButtons());
            gameManager.playGame("Level0");
            
        });

        optionButton.GetComponent<Button>().onClick.AddListener(delegate {
            audioManager.PlayClickButtonSFX();
            // StartCoroutine(shrinkAllButtons());
            //Invoke("showOptionMenu", 1f);
            optionMenu.Show();
        });

        quitButton.GetComponent<Button>().onClick.AddListener(delegate {
            audioManager.PlayClickButtonSFX();
            Application.Quit();
        });
    }

    void Update() {
        
    }

    void showOptionMenu() {
        optionMenu.Show();
    }

    private IEnumerator shrinkAllButtons() {
        Debug.Log("Shrink all buttons");
        if(!onShrink){
            shrinkButtonTween(playButton);
            shrinkButtonTween(optionButton);
            shrinkButtonTween(quitButton);
            onShrink = true;
        }
        yield return new WaitForSeconds (1);
        onShrink = false;
    }

    void shrinkButtonTween(GameObject button) {
        LeanTween.scale(button, Vector3.zero, 1f).setEase(LeanTweenType.easeInBack);
    }
}
