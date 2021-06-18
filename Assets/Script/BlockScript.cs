using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BlockScript : MonoBehaviour
{
    [Header("Vector")]
    public Vector3[] blockPos;
    GameManager GM;
    [Header("Int")]
    public int colorIndex;
    int count;
    [Header("Audio")]
    public AudioClip BlockHold;
    AudioSource AudioSource;
    private void Awake()
    {
        GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        AudioSource = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioSource>();

        blockPos = new Vector3[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            blockPos[i] = transform.GetChild(i).localPosition;
        }
    }
    
    void Update()
    {
        BlockSize(transform.position.y > -0.35f);
        ChkOutCamera();
    }
    private void OnMouseDrag()
    {
        if (!LevelController.DontTouch)
        {
            if (count == 0)
            {
                count++;
                AudioSource.clip = BlockHold;
                AudioSource.Play();
            }
            Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
            transform.position = Camera.main.ScreenToWorldPoint(mousePosition);
        }
    }
    private void OnMouseUp()
    {
        count = 0;
        Vector3 lastPos = transform.position/* + new Vector3(0, 2, 0)*/;
        lastPos.x = Mathf.RoundToInt(lastPos.x);
        lastPos.y = Mathf.RoundToInt(lastPos.y);

        GM.BlockInput(this, colorIndex, lastPos, blockPos);

        if (transform.tag == "0")
        {
            transform.position = GameManager.spawnPoint[0];
        }
        else if (transform.tag == "1")
        {
            transform.position = GameManager.spawnPoint[1];
        }
        else if (transform.tag == "2")
        {
            transform.position = GameManager.spawnPoint[2];
        }
        BlockSize(transform.position.y > -0.35f);
    }
    
    void BlockSize(bool isPositionUp)
    {
        float time = 0.25f;
        if (isPositionUp == true)
        {
            transform.DOScale(Vector3.one, time);
        }
        else if (isPositionUp == false)
        {
            transform.DOScale(new Vector3(0.6f, 0.6f, 1), time);
        }
    }
    void ChkOutCamera()
    {
        if (transform.position.x < -0.76f)
        {
            transform.position = new Vector2(-0.76f, transform.position.y);
        }
        if (transform.position.x > 7.7f)
        {
            transform.position = new Vector2(7.7f, transform.position.y);
        }
        if (transform.position.y > 10.57f)
        {
            transform.position = new Vector2(transform.position.x, 10.57f);
        }
        if (transform.position.y < -2.99f)
        {
            transform.position = new Vector2(transform.position.x, -2.99f);
        }
    }
    public void ForceDestroy()
    {
        DOTween.KillAll();
        Destroy(gameObject);
    }
}
