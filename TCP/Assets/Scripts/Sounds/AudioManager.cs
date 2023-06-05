using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
   

    [Header("-------Audio Source-------")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;
   // [SerializeField] AudioSource SFXSourceGun;
   // [SerializeField] AudioSource SFXSourceItens;

    [Space(20)]
    [Header("-------Audio Clip----------")]
    public AudioClip background;

    private void Start()
    {
        musicSource.clip = background;
        musicSource.Play();
    }

}
