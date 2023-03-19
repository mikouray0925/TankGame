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
    public static bool isPlaying { get; private set; }
    public static CameraBase currentCam;
    public static bool isChangingScene;
    public static bool isUpgrading;
    public static int currentLevel = 0;
    public bool debugMode = false;
    void Awake()
    {
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (optionMenu.gameObject.activeSelf)
            {
                optionMenu.Hide();
            }
            else
            {
                optionMenu.Show();

            }
        }
        if(debugMode){
            if(Input.GetKeyDown(KeyCode.X)){
                GameObject ultimate = GameObject.Find("AllEnemy");
                foreach(Transform child in ultimate.transform){
                    child.GetComponent<Health>().TakeDamage(100, Vector3.up);
                }
            }
        } 

        if (AllEnemies != null && AllEnemies.GetComponent<EnemyManager>().GetAliveNum() <= 0)
        {
            if (UpgradeWindowCanvas == null)
            {
                UpgradeWindowCanvas = Instantiate(UpgradeWindowPrefab);
            }

            if (currentLevel <= 2 && !isUpgrading)
            {
                currentLevel++;
                if (currentLevel == 3)
                {
                    EndGameWindow.Pop("EndGameCanvas", "You Win");
                }
                else
                {
                    Debug.Log("Showing upgrade window");
                    UpgradeWindowCanvas.GetComponent<UpgradeWindow>().Show();
                    isUpgrading = true;
                }


            }
            if (isUpgrading)
            {
                if (UpgradeWindowCanvas.activeSelf == false)
                {
                    RespawnEnemies();
                    isUpgrading = false;

                }
            }
        }
    }

    void FixedUpdate()
    {

    }

    public static CameraBase currentCamera
    {
        get
        {
            return currentCam;
        }
        set
        {
            if (currentCam && currentCam != value)
            {
                currentCam.SetActive(false);
            }
            currentCam = value;
            currentCam.SetActive(true);
        }
    }

    public void playGame(string gameSceneName)
    {
        ChangeScene(gameSceneName);
        isPlaying = true;
        Time.timeScale = 1f;
        audioManager.PlayBattleMusic();
        endGameWindow.Hide();
    }

    public void BackToMainMenu()
    {
        ChangeScene("MainMenu");
        isPlaying = false;
    }

    public void ChangeScene(string sceneName)
    {
        if (isChangingScene) return;
        if (SceneManager.GetActiveScene().name == sceneName) return;
        StartCoroutine(ChangingSceneCoroutine(sceneName));
    }

    IEnumerator ChangingSceneCoroutine(string sceneName)
    {
        isChangingScene = true;
        if (sceneName == "Level0") {
            yield return new WaitForSeconds(1f);
            currentLevel = 0;
            if(UpgradeWindowCanvas != null){
                Destroy(UpgradeWindowCanvas);
            }
        }
        Scene currentScene = SceneManager.GetActiveScene();
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        SceneManager.MoveGameObjectToScene(core, SceneManager.GetSceneByName(sceneName));
        AllEnemies = GameObject.Find("AllEnemy");
        SceneManager.UnloadSceneAsync(currentScene);
        isChangingScene = false;
    }

    public static void QuitApp()
    {
        Application.Quit();
    }

    public void RespawnEnemies()
    {
        AllEnemies.GetComponent<EnemyManager>().RespawnEnemies();
    }
}
