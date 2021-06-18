using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LevelController : MonoBehaviour
{
    Slider XpSlider;
    [Header("Sprite")]
    public Sprite[] CharImg = new Sprite[8]; //캐릭터 이미지 총 8개

    [Header("Text")]
    public Text previousLevel;
    public Text NextLevel;
    public Text UnlimitedText; //화면 오른쪽위 무제한모드 텍스트
    public Text InfoLevel;
    public Text Info;

    [Header("Bollean")]
    public bool levelUpChk = false;
    private bool loadGame;
    private bool newGame;
    static public bool DontTouch;
    bool isUnlimited; //무제한모드 검사하기 위함

    [Header("GameObject")]
    public GameObject targetPos;
    public GameObject CharPos;
    public GameObject CharInfo;
    public GameObject Char;
    GameObject Character; //캐릭터 오브젝트
    //public GameObject Unlimited; //무제한모드

    public static int currentLevel;
    static public int BestLevel = 1; //최고 레벨

    [Header("Int")]
    public int maxXp = 100;
    public int nextLevel;
    private int temp;
    static public int BestScore;
    public static int score;

    [Header("Sprite")]
    public Sprite[] img;

    [Header("Image")]
    public Image fade;

    [Header("Audio")]
    public AudioClip Level_Up;
    AudioSource AudioSource;

    [Header("String")]
    string[] Information = 
    { 
        "꼬물꼬물 유기체에요.",
        "귀여워져 돌아온 꼬물이입니다.",
        "두 다리가 생겼어요.",
        "하와이안 셔츠를 입었어요.",
        "꼬뭉이에욧! 살쪄떠",
        "가시가 생겼어요.",
        "돈이 필요해진 꼬물이입니다!"
    };
    public void levelUp()
    {
        score = maxXp;
    }
    private void Awake()
    {
        DontTouch = false;
        Character = GameObject.FindGameObjectWithTag("Character");
        AudioSource = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioSource>();
        XpSlider = transform.GetComponent<Slider>();
        isUnlimited = false;
        fade.gameObject.SetActive(false);
    }
    void Start()
    {
        string newgame = PlayerPrefs.GetString("newGame", "true");
        string loadgame = PlayerPrefs.GetString("loadGame", "false");

        newGame = System.Convert.ToBoolean(newgame);
        loadGame = System.Convert.ToBoolean(loadgame);
        levelUpChk = false;
        if (newGame)
        {
            currentLevel = 1;
            score = 0;
            maxXp = 100;
            GameManager.tempScore = 0;
        }
        else if (loadGame)
        {
            currentLevel = (int)PlayerPrefs.GetFloat("Level");
            score = (int)PlayerPrefs.GetFloat("Score");
            maxXp = (int)PlayerPrefs.GetFloat("MaxXp");
        }
        Character.GetComponent<SpriteRenderer>().sprite = CharImg[currentLevel - 1];
    }

    void Update()
    {
        //print("maxXp : " + maxXp);
        if (isUnlimited) //무제한모드일 때
        {
            UnlimitedText.gameObject.SetActive(true);
        }
        else if (!isUnlimited) //무제한모드중이 아닐때
        {
            UnlimitedText.gameObject.SetActive(false);
        }
        if (currentLevel >= 8) Character.GetComponent<SpriteRenderer>().sprite = CharImg[7];
        LevelManager();
        LevelUp();
        nextLevel = currentLevel + 1;
        XpController();
        TextPrint();
        Gameover(); 
    }
    void LevelManager()
    {
        if (BestLevel <= currentLevel) //최고기록을 넘어설 경우
        {
            BestLevel = currentLevel;
        }
        if (currentLevel >= 8 && !isUnlimited) //최대 레벨에 도달할 경우
        {
            isUnlimited = true;
        }
    }
    private void XpController()
    {
        XpSlider.value = (float)score / maxXp;
        PlayerPrefs.SetFloat("Score", score);
    }

    private void LevelUp()
    {
        if (score >= maxXp && levelUpChk == false)
        {
            if (score > maxXp)
            {
                temp = score - maxXp;
                score = temp;
            }
            else
            {
                score = temp;
            }
            currentLevel += 1;
            levelUpChk = true;
        }

        if (levelUpChk == true && currentLevel > 0)
        {
            StartCoroutine(LVupDirector());
            
            if (currentLevel < 5)
            {
                maxXp += 50;
            }
            else if (currentLevel < 6)
            {
                maxXp += 100;
            }
            else if (currentLevel < 9)
            {
                maxXp += 250;
            }
            else
            {
                maxXp = 500;
            }
            levelUpChk = false;
            if (Character.transform.position != CharPos.transform.position)
            {
                Character.transform.DOMove(CharPos.transform.position, 1f);
            }
        }
        PlayerPrefs.SetFloat("Level", currentLevel);
        PlayerPrefs.SetFloat("MaxXp", maxXp);
        PlayerPrefs.SetString("LevelUpChk", levelUpChk.ToString());
    }

    private void TextPrint()
    {
        previousLevel.text = currentLevel.ToString();
        NextLevel.text = nextLevel.ToString();
    }

    private void Gameover()
    {
        if (GameOver.gameoverSceneChk)
        {
            if(temp > BestScore)
            {
                BestScore = temp;
            }
            PlayerPrefs.SetFloat("Level", 0);
            PlayerPrefs.SetFloat("MaxXp", 100);
            PlayerPrefs.SetFloat("Score", 0);
        }
    }
    public void InfoOff()
    {
        CharInfo.SetActive(false);
        DontTouch = false;
    }

    IEnumerator LVupDirector()
    {
        float time = 0, F_time = 0.5f;
        if (currentLevel <= 8 && currentLevel >= 1)
        {
            Character.transform.DOMove(targetPos.transform.position, 0.75f);
            fade.gameObject.SetActive(true);
            Color Alpha = fade.color;
            while (Alpha.a < 0.25f)
            {
                DontTouch = true;
                time += Time.deltaTime / F_time;
                Alpha.a = Mathf.Lerp(0, 0.25f, time);
                fade.color = Alpha;
                yield return null;
            }
            yield return new WaitForSeconds(0.5f);
            Character.GetComponent<SpriteRenderer>().sprite = CharImg[currentLevel - 1]; //레벨업할때마다 플레이어 이미지 바뀜
            while (Alpha.a > 0)
            {
                time -= Time.deltaTime / F_time;
                Alpha.a = Mathf.Lerp(0, 0.25f, time);
                fade.color = Alpha;
                yield return null;
            }
            Character.transform.DOMove(CharPos.transform.position, 0.75f);
            AudioSource.clip = Level_Up;
            AudioSource.Play();
            fade.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);
            CharInfo.SetActive(true);
            InfoLevel.text = "LV." + currentLevel.ToString();
            Char.GetComponent<Image>().sprite = img[currentLevel - 2];
            Info.text = Information[currentLevel - 2];
        }
    }
}