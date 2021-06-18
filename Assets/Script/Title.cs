using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class Title : MonoBehaviour
{
    public Text scoreTxt;
    static public float score;
    bool isAni;
    int currentLevel;
    void Start()
    {
        score = PlayerPrefs.GetFloat("BestScore");
        currentLevel = (int)PlayerPrefs.GetFloat("Level");
        transform.position = new Vector3(0, 12);
        transform.DOMove(new Vector3(0, 5.11f), 1f).SetEase(Ease.OutBounce);
    }
    
    private void Update()
    {
        scoreTxt.text = score.ToString();
        if(!isAni)
            StartCoroutine(TitleAnimation());
    }
    private void OnMouseDown()
    {
        if(LevelController.currentLevel >= 8)
            SceneManager.LoadScene("EasterEggScene");
    }
    IEnumerator TitleAnimation()
    {
        isAni = true;
        transform.DOScale(new Vector3(1.5f, 1.5f, 1.5f), 0.1f);
        yield return new WaitForSeconds(0.1f);
        transform.DOScale(Vector3.one, 0.1f);
        yield return new WaitForSeconds(3);
        isAni = false;
    }
}