using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public Slider[] volume = new Slider[2];
    AudioSource[] audioSource;

    static public float a, b;
    private void Awake()
    {
        audioSource = transform.GetComponents<AudioSource>();
    }
    private void Start()
    {
        volume[0].value = PlayerPrefs.GetFloat("effectSound");
        volume[1].value = PlayerPrefs.GetFloat("backgroundSound");
    }
    private void Update()
    {
        audioSource[0].volume = volume[0].value;
        audioSource[1].volume = volume[1].value;
        VolumeSave();
    }

    private void VolumeSave()
    {
        PlayerPrefs.SetFloat("effectSound", volume[0].value);
        PlayerPrefs.SetFloat("backgroundSound", volume[1].value);
    }
}
