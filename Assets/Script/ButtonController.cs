using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonController : MonoBehaviour
{
    [Header("Boolean")]
    public static bool newGame;
    public static bool loadGame;
    public static bool gameStart;
    private bool reGameOpen;
    private bool settingOpen;
    private bool exitOpen;
    private bool isBlurOn;

    [Header("Audio")]
    public AudioClip ButtonTouch;
    public AudioClip GmOver;
    AudioSource AudioSource;

    [Header("GameObject")]
    public GameObject Result;
    public GameObject ReStart;
    public GameObject Blur;
    [SerializeField]
    private GameObject setting;
    [SerializeField]
    private GameObject exit;
    [SerializeField]
    private GameObject reGame;

    int count;

    void Awake()
    {
        gameStart = false;
        newGame = false;
        loadGame = true;
        settingOpen = false;
        exitOpen = false;
        reGameOpen = false;

        AudioSource = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioSource>();
    }

    void Update()
    {
        if (ReStart != null)
            ReStart.SetActive(reGameOpen);
        if(setting != null)
            setting.SetActive(settingOpen);
        if(Blur != null)
            Blur.SetActive(isBlurOn);

        if (GameOver.gameoverSceneChk)
        {
            gameOver();
        }
    }
    public void StartGame()
    {
            newGame = true;
            loadGame = false;
            AudioSource.clip = ButtonTouch;
            AudioSource.Play();
            PlayerPrefs.SetString("newGame", newGame.ToString());
            PlayerPrefs.SetString("loadGame", loadGame.ToString());
            SceneManager.LoadScene("InGameScene");
    }

    public void LoadGame()
    {
        newGame = false;
        loadGame = true;
        PlayerPrefs.SetString("newGame", newGame.ToString());
        PlayerPrefs.SetString("loadGame", loadGame.ToString());
        SceneManager.LoadScene("InGameScene");
    }

    public void Setting()
    {
        AudioSource.clip = ButtonTouch;
        AudioPlay();
        settingOpen = !settingOpen;
        isBlurOn = !isBlurOn;
        LevelController.DontTouch = !LevelController.DontTouch;
    }

    public void Exit()
    {
        exitOpen = !exitOpen;
    }
    public void _reGame()
    {
        AudioSource.clip = ButtonTouch;
        AudioPlay();
        settingOpen = !settingOpen;
        reGameOpen = !reGameOpen;
    }
    public void GameExit()
    {
        Application.Quit();
    }

    public void ToHome()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void ReGameScene()
    {
        SceneManager.LoadScene("InGameScene");
    }
    public void AskReGame()
    {
        ReStart.SetActive(true);
    }
    public void gameOver()
    {
        if (count == 0)
        {
            AudioSource.clip = GmOver;
            AudioPlay();
            count++;
        }
        GameOver.gameoverSceneChk = true;
        reGameOpen = false;
        settingOpen = false;
        ReStart.SetActive(false);
        Result.SetActive(true);
        setting.SetActive(false);
        isBlurOn = true;
        LevelController.DontTouch = true;
        newGame = true;
        loadGame = false;
        PlayerPrefs.SetString("newGame", newGame.ToString());
        PlayerPrefs.SetString("loadGame", loadGame.ToString());
    }
    public void ReGame()
    {
        newGame = true;
        loadGame = false;
        AudioSource.clip = ButtonTouch;
        AudioPlay();
        GameOver.gameoverSceneChk = false;
        LevelController.DontTouch = false;
        Result.SetActive(false);
        PlayerPrefs.SetString("newGame", newGame.ToString());
        PlayerPrefs.SetString("loadGame", loadGame.ToString());
        SceneManager.LoadScene("InGameScene");
    }
    void AudioPlay()
    {
        AudioSource.Play();
    }
}
