using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject core;
    [SerializeField] OptionMenu optionMenu;
    [SerializeField] EndGameWindow endGameWindow;
    [SerializeField] private GameObject UpgradeWindowPrefab;
    private GameObject AllEnemies;
    public AudioManager audioManager;
    public GameObject UpgradeWindowCanvas;
    public static bool isPlaying {get; private set;}
    public static CameraBase currentCam;
    public static bool isChangingScene;
    
    private int currentLevel = 0;
    void Awake() {
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (optionMenu.gameObject.activeSelf) {
                optionMenu.Hide();
            }
            else {
                //optionMenu.Show();
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }

        if(AllEnemies!=null && AllEnemies.GetComponent<EnemyManager>().GetAliveNum() <= 0) {
            if(UpgradeWindowCanvas == null) {
                UpgradeWindowCanvas = Instantiate(UpgradeWindowPrefab);
            }
            if(currentLevel< 10){
                Debug.Log("Showing upgrade window");
                UpgradeWindowCanvas.GetComponent<UpgradeWindow>().Show();
                //currentLevel++;
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
        if(sceneName == "Level0" ) yield return new WaitForSeconds(1f);
        Scene currentScene = SceneManager.GetActiveScene();
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        while (!asyncLoad.isDone) {
            yield return null;
        }

        SceneManager.MoveGameObjectToScene(core, SceneManager.GetSceneByName(sceneName));
        AllEnemies = GameObject.Find("AllEnemy");
        SceneManager.UnloadSceneAsync(currentScene);
        isChangingScene = false;
    }

    public static void QuitApp() {   
        Application.Quit();
    }

}
