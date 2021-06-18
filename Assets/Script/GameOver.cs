using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    private int finalScore;
    private int finalLevel;

    [SerializeField]
    private Text scoreTxt;
    [SerializeField]
    private Text levelTxt;

    [SerializeField]
    private GameObject gameoverScene;
    public static bool gameoverSceneChk;

    void Start()
    {
        gameoverSceneChk = false;
    }

    void Update()
    {
        if (gameoverSceneChk)
        {
            finalScore = (int)GameManager.tempScore;
            finalLevel = LevelController.BestLevel;
            scoreTxt.text = finalScore.ToString();
            levelTxt.text = finalLevel.ToString();
        }
        Title.score = finalScore;
    }
}
