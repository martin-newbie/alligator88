using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System.Collections;

public class EasterEgg : MonoBehaviour
{
    public Text EndingCredit;
    public GameObject Moon;
    public GameObject TargetPos;
    Vector3 ObjectPos;
    bool isAction;
    private void Awake()
    {
        ObjectPos = Moon.transform.position;
    }
    void Start()
    {
        EndingCredit.rectTransform.DOMove(new Vector3(EndingCredit.rectTransform.position.x, 2500, 0), 25f);
    }
    private void Update()
    {
        if (!isAction)
        {
            StartCoroutine(MoonUpDown());
        }
    }
    IEnumerator MoonUpDown()
    {
        isAction = true;
        yield return null;
        Moon.transform.DOMoveY(3.4f, 2f);
        yield return new WaitForSeconds(3);
        Moon.transform.DOMoveY(3.2f, 2f);
        yield return new WaitForSeconds(3);
        yield return null;
        isAction = false;
    }
}