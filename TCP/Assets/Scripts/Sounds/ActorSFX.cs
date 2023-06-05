using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class ActorSFX : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
 

    public void PlaySFX(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
    

    //Trocar audio com base no local onde estou 
}
