using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject core;
    [SerializeField] OptionMenu optionMenu;
    [SerializeField] EndGameWindow endGameWindow;
    public AudioManager audioManager;

    public static bool isPlaying {get; private set;}
    public static CameraBase currentCam;
    public static bool isChangingScene;
    
    void Awake() {
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (optionMenu.gameObject.activeSelf) {
                optionMenu.Hide();
            }
            else {
                optionMenu.Show();
            }
        }
    }

    void FixedUpdate() {

    }

    public static CameraBase currentCamera {
        get {
            return currentCam;
        }
        set {
            if (currentCam && currentCam != value) {
                currentCam.SetActive(false);
            } 
            currentCam = value;
            currentCam.SetActive(true);
        }
    }

    public void playGame(string gameSceneName) {
        ChangeScene(gameSceneName);
        isPlaying = true;
        Time.timeScale = 1f;
        audioManager.PlayBattleMusic();
        endGameWindow.Hide();
    }   

    public void BackToMainMenu() {
        ChangeScene("MainMenu");
        isPlaying = false;
    }  

    public void ChangeScene(string sceneName) {
        if (isChangingScene) return;
        if (SceneManager.GetActiveScene().name == sceneName) return;
        StartCoroutine(ChangingSceneCoroutine(sceneName));
    }

    IEnumerator ChangingSceneCoroutine(string sceneName) {
        isChangingScene = true;
        yield return new WaitForSeconds(1f);
        Scene currentScene = SceneManager.GetActiveScene();
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        while (!asyncLoad.isDone) {
            yield return null;
        }

        SceneManager.MoveGameObjectToScene(core, SceneManager.GetSceneByName(sceneName));
        SceneManager.UnloadSceneAsync(currentScene);
        isChangingScene = false;
    }

    public static void QuitApp() {   
        Application.Quit();
    }
}
