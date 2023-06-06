using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;
using System.IO;

[System.Serializable]
public class GameSaveData
{
    public Vector3 playerPosition;
    public float VidaAtual;
    public float EstaminaAtual;
    public float FomeAtual;
    public float SedeAtual;
    public bool rifleDesbloqueado;
    public bool shotgunDesbloqueado;
    public bool pistolaDesbloqueado;
    public int pistolaBalasGuardadas;
    public int pistolaBalasNoPente;
    public int shotgunBalasGuardadas;
    public int shotgunBalasNoPente;
    public int rifleBalasGuardadas;
    public int rifleBalasNoPente;
}

public class GameSaveManager : MonoBehaviour
{
    private GameSaveData gameSaveData;
    public Player player;
    public Controle controle;
    public Gun gun;
    private static GameSaveManager instance;

    private const string saveFilePath = "/savegame.json";

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SaveGame()
    {
        // Crie um objeto GameSaveData e defina as informações que você deseja salvar
        gameSaveData = new GameSaveData();
        gameSaveData.playerPosition = player.transform.position;
        gameSaveData.VidaAtual = controle.VidaAtual;
        gameSaveData.EstaminaAtual = controle.EstaminaAtual;
        gameSaveData.FomeAtual = controle.FomeAtual;
        gameSaveData.SedeAtual = controle.SedeAtual;
        gameSaveData.rifleDesbloqueado = gun.rifleDesbloqueado;
        gameSaveData.shotgunDesbloqueado = gun.shotgunDesbloqueado;
        gameSaveData.pistolaDesbloqueado = gun.pistolaDesbloqueado;
        gameSaveData.pistolaBalasGuardadas = gun.pistolaBalasGuardadas;
        gameSaveData.pistolaBalasNoPente = gun.pistolaBalasNoPente;
        gameSaveData.shotgunBalasGuardadas = gun.shotgunBalasGuardadas;
        gameSaveData.shotgunBalasNoPente = gun.shotgunBalasNoPente;
        gameSaveData.rifleBalasGuardadas = gun.rifleBalasGuardadas;
        gameSaveData.rifleBalasNoPente = gun.rifleBalasNoPente;

        // Converta o objeto em JSON
        string jsonData = JsonUtility.ToJson(gameSaveData);

        // Salve o JSON em um arquivo
        string savePath = Application.persistentDataPath + saveFilePath;
        File.WriteAllText(savePath, jsonData);
    }

    public void LoadGame()
    {
        // Verifique se o arquivo de salvamento existe
        string savePath = Application.persistentDataPath + saveFilePath;
        if (File.Exists(savePath))
        {
            // Leia o JSON do arquivo
            string jsonData = File.ReadAllText(savePath);

            // Converta o JSON de volta para o objeto GameSaveData
            gameSaveData = JsonUtility.FromJson<GameSaveData>(jsonData);

            // Restaure as informações salvas no jogo
            player.transform.position = gameSaveData.playerPosition;
            controle.VidaAtual = gameSaveData.VidaAtual;
            controle.EstaminaAtual = gameSaveData.EstaminaAtual;
            controle.FomeAtual = gameSaveData.FomeAtual;
            controle.SedeAtual = gameSaveData.SedeAtual;
            gun.rifleDesbloqueado = gameSaveData.rifleDesbloqueado;
            gun.shotgunDesbloqueado = gameSaveData.shotgunDesbloqueado;
            gun.pistolaDesbloqueado = gameSaveData.pistolaDesbloqueado;
            gun.pistolaBalasGuardadas = gameSaveData.pistolaBalasGuardadas;
            gun.pistolaBalasNoPente = gameSaveData.pistolaBalasNoPente;
            gun.shotgunBalasGuardadas = gameSaveData.shotgunBalasGuardadas;
            gun.shotgunBalasNoPente = gameSaveData.shotgunBalasNoPente;
            gun.rifleBalasGuardadas = gameSaveData.rifleBalasGuardadas;
            gun.rifleBalasNoPente = gameSaveData.rifleBalasNoPente;
        }
    }

    public bool SaveExists()
    {
        // Verifique se o arquivo de salvamento existe
        string savePath = Application.persistentDataPath + saveFilePath;
        return File.Exists(savePath);
    }

    public void DeleteSave()
    {
        // Exclua o arquivo de salvamento, se existir
        string savePath = Application.persistentDataPath + saveFilePath;
        if (File.Exists(savePath))
        {
            File.Delete(savePath);
        }
    }
}
