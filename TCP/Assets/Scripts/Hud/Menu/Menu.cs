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
    [SerializeField] private string cenaMenu;

    [Space(10)]
    [Header("------------Settings------------")]
    [SerializeField] public GameObject optionsPanel;
    [SerializeField] private GameObject PausePanel;
    [SerializeField] private GameObject continueGame;
    [SerializeField] public GameObject LoadProgress;
    [SerializeField] private Slider BarraLoading;
    [SerializeField] private GameObject deathScreen;

    [Space(10)]
    [Header("------------Sons------------")]
    [SerializeField] private AudioMixer mix;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider SFXslider;

    [Space(10)]
    [Header("------------Sons do Menu------------")]
    [SerializeField] private AudioMixer mixM;
    [SerializeField] private Slider musicSliderM;
    [SerializeField] private Slider SFXsliderM;

    [Space(10)]
    [Header("------------Dificuldade------------")]
    [SerializeField] private int dificuldade;   // 1- facil / 2- normal / 3- dificil //
    [SerializeField] private TextMeshProUGUI legenda;
    [SerializeField] private string facil;
    [SerializeField] private string normal;
    [SerializeField] private string dificil;
    [SerializeField] private Inimigo inimigo;

    [Space(10)]
    [Header("------------Save------------")]
    public GameObject confirmPanel;
    public GameSaveManager saveManager;

    public bool isPaused = false;

   // private bool isNewGame = false;
    private bool isSaveExists = false;

    private void Start()
    {
        isSaveExists = saveManager.SaveExists();

        if (saveManager.SaveExists())
        {
            continueGame.SetActive(true);
        }
        else
        {
            continueGame.SetActive(false);
        }

        if (PlayerPrefs.HasKey("musicVolume") || PlayerPrefs.HasKey("sfxVolume")) // Som Game
        {
            LoadVolume();
        }
        else
        {
            SetMusicVolume();
            SetSFXVolume();
        }

        if (PlayerPrefs.HasKey("musicVolumeM") || PlayerPrefs.HasKey("sfxVolumeM")) // Som Menu
        {
            MLoadVolume();
        }
        else
        {
            SetMusicVolumeM();
            SetSFXVolumeM();
        }

        deathScreen.SetActive(false);
    }


    public IEnumerator CarregarCena()
    {
        LoadProgress.SetActive(true);
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(cena);
        while (!asyncOperation.isDone)
        {
            this.BarraLoading.value = asyncOperation.progress;
            Debug.Log("Carregando" + (asyncOperation.progress * 100f) + "%");
            yield return null;
        }
    }

    public IEnumerator CarregarCenaMenu()
    {
        LoadProgress.SetActive(true);
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(cenaMenu);
        while (!asyncOperation.isDone)
        {
            this.BarraLoading.value = asyncOperation.progress;
            Debug.Log("Carregando" + (asyncOperation.progress * 100f) + "%");
            yield return null;
        }
    }

    public void Jogar()
    {
        if (isSaveExists)
        {
            confirmPanel.SetActive(true);
        }
        else
        {
            StartNewGame();
        }
    }

    public void ConfirmarSim()
    {
        StartNewGame();
        confirmPanel.SetActive(false);
    }

    public void ConfirmarNao()
    {
        confirmPanel.SetActive(false);
    }

    public void StartNewGame()
    {
        saveManager.DeleteSave();
        StartCoroutine(CarregarCena());
    }

    public void ContinueGame()
    {   
        StartCoroutine(CarregarCena());
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

    #region Sounds Game

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

    #endregion

    #region Sounds Menu

    public void SetMusicVolumeM()
    {
        float volume = musicSliderM.value;
        mixM.SetFloat("musicm", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("musicVolumeM", volume);
    }

    public void SetSFXVolumeM()
    {
        float volume = SFXsliderM.value;
        mixM.SetFloat("sfxm", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("sfxVolumeM", volume);
    }

    private void MLoadVolume()
    {
        musicSliderM.value = PlayerPrefs.GetFloat("musicVolumeM");
        SFXsliderM.value = PlayerPrefs.GetFloat("sfxVolumeM");

        SetMusicVolumeM();
        SetSFXVolumeM();
    }

    #endregion

    public void MenuPrincipal()
    {
        StartCoroutine(CarregarCenaMenu());
    }

    public void Sair()
    {
        Debug.Log("Saiu");
        Application.Quit();
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

    public void TelaMorte()
    {
        deathScreen.SetActive(true);
        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
      //  if (isSaveExists)
      //  {
           saveManager.LoadGame(); 
     //   }
        StartCoroutine(CarregarCena());         
        deathScreen.SetActive(false);
    }

    #region dificuldade

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

    #endregion
}
