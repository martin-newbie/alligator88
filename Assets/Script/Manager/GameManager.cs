using System.Collections;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System.IO;
using System;
using System.Globalization;

public class GameManager : MonoBehaviour
{
    [Header("Saved lines")]
    public string[] tempLine;
    public string[] tempLine1;
    public string[] tempLine2;
    public string[] tempLine3;
    public string[] tempLine4;
    public string[] tempLine5;
    public string[] tempLine6;
    public string[] tempLine7;
    int[] array = new int[SIZE];
    int[] array1 = new int[SIZE];
    int[] array2 = new int[SIZE];
    int[] array3 = new int[SIZE];
    int[] array4 = new int[SIZE];
    int[] array5 = new int[SIZE];
    int[] array6 = new int[SIZE];
    int[] array7 = new int[SIZE];
    

    public static string path = Application.persistentDataPath + @"\array.txt";
    //public static string path = System.IO.Directory.GetCurrentDirectory() + @"\array.txt";

    [Header("Text")]
    [SerializeField] private Text scoreTxt;

    [Header("Color")]
    public Color[] ShapeColors;
    
    [Header("Int")]
    const int SIZE = 8;
    public int[,] Array = new int[SIZE, SIZE];
    int count;
    int[] random = new int[3];

    [Header("Audio")]
    public AudioClip Block_Input;
    public AudioClip Mistake_Input;
    public AudioClip Line_Clear;
    AudioSource AudioSource;

    [Header("Transform")]
    public Transform[] BlockPos;
    
    [Header("GameObject")]
    public GameObject[] Cells;
    public GameObject[] prefab;


    static public Vector3[] spawnPoint = new Vector3[3];
    public static float tempScore = 0;
    public static float bestScore = 0;
    bool isEmpty;
    bool isLineClear;
    string score;

    private void Awake()
    {
        if(File.Exists(path))
        {
            
        }
        else
        {
            for (int i = 0; i < SIZE; i++)
            {
                for (int j = 0; j < SIZE; j++)
                {
                    Array[i, j] = 0;
                    Debug.Log(Array[i, j]);
                }
            }
            Save();
        }

        for (int i = 0; i < 3; i++)
            random[i] = UnityEngine.Random.Range(0, prefab.Length);
        spawnPoint[0] = BlockPos[0].position;
        spawnPoint[1] = BlockPos[1].position;
        spawnPoint[2] = BlockPos[2].position;
        AudioSource = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioSource>();
        if (ButtonController.newGame)
        {
            tempScore = 0;
            PlayerPrefs.SetFloat("temp", tempScore);
            ArrayInit();
        }
        else if (ButtonController.loadGame)
        {
            tempScore = PlayerPrefs.GetFloat("temp");
            Load();
            ColorInit();
        }
    }

    private void Start()
    {
        PlayerPrefs.GetFloat("BestScore");
        StartCoroutine(SpawnBlocks(random));

        isEmpty = Convert.ToBoolean(PlayerPrefs.GetString("isEmpty"));
        if(!isEmpty)
        {

        }
    }
    void Update()
    {
        score = "\n" + tempScore.ToString();
        if (count == 3)
        {
            count = 0;
            isEmpty = true;
        }

        if (isEmpty)
        {
            isEmpty = false;
            for (int i = 0; i < 3; i++)
            {
                spawnPoint[i] = BlockPos[i].position;
            }
            for (int i = 0; i < 3; i++)
            {
                random[i] = UnityEngine.Random.Range(0, prefab.Length);
            }
            PlayerPrefs.SetFloat("randBlock1", random[0]);
            PlayerPrefs.SetFloat("randBlock2", random[1]);
            PlayerPrefs.SetFloat("randBlock3", random[2]);
            StartCoroutine(SpawnBlocks(random));
        }
        PlayerPrefs.SetString("isEmpty", isEmpty.ToString());
        if (bestScore < tempScore)
        {
            bestScore = tempScore;
            PlayerPrefs.SetFloat("BestScore", bestScore);
        }

        for (int i = 0; i < 3; i++)
            PlayerPrefs.SetInt("BlockIndex" + i, random[i]);

        scoreTxt.text = "SCORE" + "\n" + score;
        Save();
    }
    private void ArrayInit()
    {
        for (int i = 0; i < SIZE; i++)
        {
            for (int j = 0; j < SIZE; j++)
            {
                Array[i, j] = 0; 
            }
        }
    }

    private void ColorInit()
    {
        for (int i = 0; i < SIZE; i++)
        {
            for (int j = 0; j < SIZE; j++)
            {
                if(Array[i, j] != 0)
                {
                    switch(Array[i, j])
                    {
                        case 1: GetCell(i, j).GetComponent<SpriteRenderer>().color = ShapeColors[1]; break;
                        case 2: GetCell(i, j).GetComponent<SpriteRenderer>().color = ShapeColors[2]; break;
                        case 3: GetCell(i, j).GetComponent<SpriteRenderer>().color = ShapeColors[3]; break;
                        case 4: GetCell(i, j).GetComponent<SpriteRenderer>().color = ShapeColors[4]; break;
                        case 5: GetCell(i, j).GetComponent<SpriteRenderer>().color = ShapeColors[5]; break;
                        case 6: GetCell(i, j).GetComponent<SpriteRenderer>().color = ShapeColors[6]; break;
                        case 7: GetCell(i, j).GetComponent<SpriteRenderer>().color = ShapeColors[7]; break;
                        case 8: GetCell(i, j).GetComponent<SpriteRenderer>().color = ShapeColors[8]; break;
                        case 9: GetCell(i, j).GetComponent<SpriteRenderer>().color = ShapeColors[9]; break;
                        case 10: GetCell(i, j).GetComponent<SpriteRenderer>().color = ShapeColors[10]; break;
                        case 11: GetCell(i, j).GetComponent<SpriteRenderer>().color = ShapeColors[11]; break;
                        case 12: GetCell(i, j).GetComponent<SpriteRenderer>().color = ShapeColors[12]; break;
                        case 13: GetCell(i, j).GetComponent<SpriteRenderer>().color = ShapeColors[13]; break;
                        case 14: GetCell(i, j).GetComponent<SpriteRenderer>().color = ShapeColors[14]; break;
                        default: GetCell(i, j).GetComponent<SpriteRenderer>().color = ShapeColors[0]; break;
                    }
                }
            }
        }
    }

    private void Save()
    {
        string temp = "";

        FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write);
        StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.UTF8);
        for (int i = 0; i < SIZE; i++)
        {
            for (int j = 0; j < SIZE; j++)
            {
                temp += Array[i, j].ToString() + "\t";
            }
            if (i < SIZE - 1)
            {
                temp += "\n";
            }
        }
        sw.WriteLine(temp);
        sw.Flush();
        sw.Close();
        fs.Close();
    }

    public void Load()
    {
        string[] line = File.ReadAllLines(path);

        tempLine = line[0].Split('\t');
        tempLine1 = line[1].Split('\t');
        tempLine2 = line[2].Split('\t');
        tempLine3 = line[3].Split('\t');
        tempLine4 = line[4].Split('\t');
        tempLine5 = line[5].Split('\t');
        tempLine6 = line[6].Split('\t');
        tempLine7 = line[7].Split('\t');
        
        for (int i = 0; i < SIZE; i++)
        {
            array[i] = int.Parse(tempLine[i]);
            array1[i] = int.Parse(tempLine1[i]);
            array2[i] = int.Parse(tempLine2[i]);
            array3[i] = int.Parse(tempLine3[i]);
            array4[i] = int.Parse(tempLine4[i]);
            array5[i] = int.Parse(tempLine5[i]);
            array6[i] = int.Parse(tempLine6[i]);
            array7[i] = int.Parse(tempLine7[i]);

        }

        for (int i = 0; i < SIZE; i++)
        {
            Array[0, i] = array[i];
            Array[1, i] = array1[i];
            Array[2, i] = array2[i];
            Array[3, i] = array3[i];
            Array[4, i] = array4[i];
            Array[5, i] = array5[i];
            Array[6, i] = array6[i];
            Array[7, i] = array7[i];
        }
        for (int i = 0; i < 3; i++) random[i] = PlayerPrefs.GetInt("BlockIndex" + i);
    }

    GameObject GetCell(int x, int y)
    {
        return Cells[y * SIZE + x];
    }
    bool IsRange(int x, int y)
    {
        if (x < 0 || y < 0 || x >= SIZE || y >= SIZE) return false;
        return true;
    }
    bool IsPossible(int x, int y)
    {
        if (Array[x, y] != 0) return false;
        return true;
    }
    public void BlockInput(BlockScript blockScript, int ColorIndex, Vector3 lastPos, Vector3[] blockPos)
    {
        for (int i = 0; i < blockPos.Length; i++)
        {
            Vector3 SumPos = blockPos[i] + lastPos;
            if (!IsRange((int)SumPos.x, (int)SumPos.y)) { AudioSource.clip = Mistake_Input; AudioSource.Play(); return; }
            if (!IsPossible((int)SumPos.x, (int)SumPos.y)) { AudioSource.clip = Mistake_Input; AudioSource.Play(); return; }
        }
        for (int i = 0; i < blockPos.Length; i++)
        {
            Vector3 SumPos = blockPos[i] + lastPos;
            Array[(int)SumPos.x, (int)SumPos.y] = ColorIndex;
            GetCell((int)SumPos.x, (int)SumPos.y).GetComponent<SpriteRenderer>().color = blockScript.transform.GetComponentInChildren<SpriteRenderer>().color;
        }
        count++;
        blockScript.ForceDestroy();
        tempScore += blockPos.Length;
        LineLogic();
        if (!isLineClear)
        {
            AudioSource.clip = Block_Input;
            AudioSource.Play();
        }
        if (isLineClear)
        {
            isLineClear = false;
        }

        Invoke("CheckEnd", 0.07f);
        LevelController.score += blockPos.Length;
    }

    void LineLogic()
    {
        int oneLine = 0;
        int comboScore = 2;

        for (int i = 0; i < SIZE; i++)
        {
            int horizontalCount = 0;
            int verticalCount = 0;
            for (int j = 0; j < SIZE; j++)
            {
                if (Array[j, i] != 0) ++horizontalCount;
                if (Array[i, j] != 0) ++verticalCount;
            }
            if (horizontalCount == SIZE)
            {
                ++oneLine;
                for (int j = 0; j < SIZE; j++) Array[j, i] = -1;
            }
            if (verticalCount == SIZE)
            {
                ++oneLine;
                for (int j = 0; j < SIZE; j++) Array[i, j] = -1;
            }
        }

        for (int i = 0; i < SIZE; i++)
        {
            for (int j = 0; j < SIZE; j++)
            {
                if (Array[i, j] == -1)
                {
                    Array[i, j] = 0;
                    GameObject CurCell = GetCell(i, j);
                    var doScale = CurCell.transform.DOScale(Vector3.zero, 0.5f);
                    doScale.OnComplete(() =>
                    {
                        CurCell.GetComponent<SpriteRenderer>().color = ShapeColors[0];
                        CurCell.transform.localScale = new Vector3(0.23f, 0.23f, 1);
                    });
                }
            }
        }
        if (oneLine >= 1)
        {
            isLineClear = true;
            AudioSource.clip = Line_Clear;
            AudioSource.Play();
        }
        LevelController.score += oneLine * 4 * comboScore;
        tempScore += oneLine * 4 * comboScore;
        PlayerPrefs.SetFloat("temp", tempScore);
    }
    IEnumerator SpawnBlocks(int[] index)
    {
        for (int i = 0; i < BlockPos.Length; i++)
        {
            yield return new WaitForSeconds(0.01f);
            Transform CurShape = Instantiate(prefab[index[i]], BlockPos[i].position + new Vector3(10, 0, 0), Quaternion.identity).transform;
            CurShape.gameObject.tag = i.ToString();
            CurShape.SetParent(BlockPos[i]);
            if (index[i] == 0 || index[i] == 1 || index[i] == 2 || index[i] == 4 ||
                index[i] == 6 || index[i] == 7 || index[i] == 8 || index[i] == 10 ||
                index[i] == 11 || index[i] == 12 || index[i] == 13)
            {
                CurShape.DOMove(BlockPos[i].position + new Vector3(0.46f, 1.23f, 0), 0.4f);
                spawnPoint[i] += new Vector3(0.46f, 0, 0);
            }
            else
            {
                CurShape.DOMove(BlockPos[i].position + new Vector3(0, 1.23f, 0), 0.4f);
            }
        }
        yield return null;
        if (!AvailCheck()) Die();
    }
    bool PutAble(Vector3[] ShapePos)
    {
        for (int i = 0; i < SIZE; i++)
        {
            for (int j = 0; j < SIZE; j++)
            {
                int count = 0;
                for (int k = 0; k < ShapePos.Length; k++)
                {
                    Vector3 CurShapePos = ShapePos[k] + new Vector3(i, j, 0);
                    if (!IsRange((int)CurShapePos.x, (int)CurShapePos.y)) break;
                    if (!IsPossible((int)CurShapePos.x, (int)CurShapePos.y)) break;
                    ++count;
                }
                if (count == ShapePos.Length) return true;
            }
        }
        return false;
    }
    bool AvailCheck()
    {
        int count = 0;
        for (int i = 0; i < BlockPos.Length; i++)
        {
            if (BlockPos[i].childCount != 0)
            {
                count++;
                if (PutAble(BlockPos[i].GetComponentInChildren<BlockScript>().blockPos)) { return true; }
            }
        }
        return count == 0;
    }

    void CheckEnd()
    {
        if (!AvailCheck())
        {
            Die();
            return;
        }
    }
    void Die() =>GameOver.gameoverSceneChk = true;
}