using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;
public class Menu : MonoBehaviour
{
    [Space(10)]
    [Header("------------Cenas------------")]
    [SerializeField] private string cena;

    [Space(10)]
    [Header("------------Settings------------")]
    [SerializeField] private GameObject optionsPanel;

    [Space(10)]
    [Header("------------Sons------------")]
    [SerializeField] private AudioSource music;
    [SerializeField] private AudioSource sfx;

    [Space(10)]
    [Header("------------Dificuldade------------")]
    [SerializeField] private int dificuldade;   // 1- facil / 2- normal / 3- dificil //
    [SerializeField] private TextMeshProUGUI legenda;
    [SerializeField] private string facil;
    [SerializeField] private string normal;
    [SerializeField] private string dificil;



    public void Jogar()
    {
        SceneManager.LoadScene(cena);
    }


    public void Sair()
    {
        Debug.Log("Saiu");
        //Sai do jogo no edit mode
        UnityEditor.EditorApplication.isPlaying = false;
        // Fecha o jogo Compilado
        // Application.Quit();  
    }

    public void ShowOptions()
    {
        optionsPanel.SetActive(true);
        GetDificuldade();
        SetLegenda();
    }
    public void BackToMenu()
    {
        optionsPanel.SetActive(false);
    }

    public void VolumeMusica(float value)
    {
        music.volume = value;
    }

    public void VolumeSFX(float value)
    {
        sfx.volume = value;
    }


    // -------------------------------- DIFICULDADE ----------------------------------------\\

    void GetDificuldade()
    {
        if(PlayerPrefs.HasKey("Dificuldade"))
        {
            dificuldade = PlayerPrefs.GetInt("Dificuldade");
        }
        else
        {
            dificuldade = 2;
        }
    }

    void SetLegenda()
    {
        switch (dificuldade)
        {
            case 1: legenda.text = facil; break;
            case 2: legenda.text = normal; break;
            case 3: legenda.text = dificil; break;
        }
    }

    public void SetEasy()
    {
        GetDificuldade();
        dificuldade--;

        if (dificuldade < 1) dificuldade = 1;

        SetDificuldade();
    }

    public void SetHard()
    {
        GetDificuldade();
        dificuldade++;

        if (dificuldade > 3) dificuldade = 3;

        SetDificuldade();
    }

    public void SetDificuldade()
    {
        PlayerPrefs.SetInt("Dificuldade", dificuldade);

        SetLegenda();
    }
}
