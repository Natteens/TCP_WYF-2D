using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;
using System.IO;

public class Menu : MonoBehaviour
{
    [Space(10)]
    [Header("------------Cenas------------")]
    [SerializeField] private string cena;

    [Space(10)]
    [Header("------------Settings------------")]
    [SerializeField] public GameObject optionsPanel;
    [SerializeField] private GameObject PausePanel;

    [Space(10)]
    [Header("------------Sons------------")]
    [SerializeField] private AudioMixer mix;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider SFXslider;

    [Space(10)]
    [Header("------------Dificuldade------------")]
    [SerializeField] private int dificuldade;   // 1- facil / 2- normal / 3- dificil //
    [SerializeField] private TextMeshProUGUI legenda;
    [SerializeField] private string facil;
    [SerializeField] private string normal;
    [SerializeField] private string dificil;



    [Space(10)]
    [Header("------------Save------------")]
    public GameObject confirmPanel;
    public GameSaveManager saveManager;

    public bool isPaused = false;



    // Outras variáveis que você pode precisar para controlar o estado do jogo
    private bool isNewGame = false;
    private bool isSaveExists = false;

    private void Start()
    {
        // Verifique se existe um save
        isSaveExists = saveManager.SaveExists();

        if (PlayerPrefs.HasKey("musicVolume"))
        {
            LoadVolume();
        }
        else
        {
            SetMusicVolume();
            SetSFXVolume();
        }
    }


    public void Jogar()
    {
        if (isSaveExists)
        {
            // Ative o painel de confirmação
            confirmPanel.SetActive(true);
        }
        else
        {
            // Se não houver um save existente, inicie um novo jogo
            StartNewGame();
        }
    }

    public void ConfirmarSim()
    {
        // Se o jogador confirmou iniciar um novo jogo
        StartNewGame();

        // Desative o painel de confirmação
        confirmPanel.SetActive(false);
    }

    public void ConfirmarNao()
    {
        // Se o jogador optou por não iniciar um novo jogo

        // Desative o painel de confirmação
        confirmPanel.SetActive(false);
    }



    public void StartNewGame()
    {
        // Verifique se há um save existente antes de iniciar um novo jogo
        if (isSaveExists)
        {
            // Se houver um save existente, exclua-o
            saveManager.DeleteSave();
        }

        // Inicie um novo jogo
        isNewGame = true;
        StartGame();
    }

    public void ContinueGame()
    {
       

        // Continue o jogo a partir do save
        isNewGame = false;
        StartGame();
    }

    private void StartGame()
    {
        SceneManager.LoadScene(cena);
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

    public void SetMusicVolume()
    {
        float volume = musicSlider.value;
        mix.SetFloat("music", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("musicVolume", volume);
    }

    public void SetSFXVolume()
    {
        float volume = SFXslider.value;
        mix.SetFloat("sfx", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("sfxVolume", volume);
    }

    private void LoadVolume()
    {
        musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
        SFXslider.value = PlayerPrefs.GetFloat("sfxVolume");

        SetMusicVolume();
        SetSFXVolume();
    }

    public void MenuPrincipal()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Sair()
    {
        Debug.Log("Saiu");
        //Sai do jogo no edit mode
        UnityEditor.EditorApplication.isPlaying = false;
        // Fecha o jogo Compilado
        // Application.Quit();  
    }

    public void PauseScreen()
    {
        if (isPaused)
        {
            isPaused = false;
            Time.timeScale = 1f;
            PausePanel.SetActive(false);
        }
        else
        {
            isPaused = true;
            Time.timeScale = 0f;
            PausePanel.SetActive(true);
        }
    }

    #region dificuldade
    // -------------------------------- DIFICULDADE ----------------------------------------\\

    void GetDificuldade()
    {
        if (PlayerPrefs.HasKey("Dificuldade"))
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
            case 1:
                legenda.text = facil;
                break;
            case 2:
                legenda.text = normal;
                break;
            case 3:
                legenda.text = dificil;
                break;
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

    // -------------------------------- DIFICULDADE ----------------------------------------\\
    #endregion
}