using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;
using System.IO;
using System.Security.Cryptography;
using System.Text;

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
    public static GameSaveManager instance { get; private set; }

    private const string saveFilePath = "/savegame.dat";
    private static readonly byte[] encryptionKey = GenerateKey("OmelhorTcpDoIFRJ", 256);
    private static readonly byte[] encryptionIV = GenerateKey("OmelhorTcpDoIFRJ", 128);

    private void Awake()
    {
        if (instance != null)
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

        // Criptografe o JSON
        byte[] encryptedBytes = Encrypt(jsonData);

        // Salve os bytes criptografados em um arquivo
        string savePath = Application.persistentDataPath + saveFilePath;
        File.WriteAllBytes(savePath, encryptedBytes);
        Debug.Log("Arquivo de salvamento criado em: " + savePath);
    }

    public void LoadGame()
    {
        // Verifique se o arquivo de salvamento existe
        string savePath = Application.persistentDataPath + saveFilePath;
        if (SaveExists())
        {
            // Leia os bytes do arquivo
            byte[] encryptedBytes = File.ReadAllBytes(savePath);

            // Descriptografe os bytes
            string jsonData = Decrypt(encryptedBytes);

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

            Debug.Log("Carregando arquivo de salvamento de: " + savePath);
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
        if (SaveExists())
        {
            string savePath = Application.persistentDataPath + saveFilePath;
            Debug.Log("Excluindo arquivo de salvamento em: " + savePath);
            File.Delete(savePath);
        }
    }

    private static byte[] GenerateKey(string password, int keySize)
    {
        const int SaltSize = 16;
        const int Iterations = 10000;

        byte[] salt = new byte[SaltSize];
        new RNGCryptoServiceProvider().GetBytes(salt);

        var derivedBytes = new Rfc2898DeriveBytes(password, salt, Iterations);
        return derivedBytes.GetBytes(keySize / 8);
    }

    private byte[] Encrypt(string data)
    {
        using (Aes aes = Aes.Create())
        {
            aes.Key = encryptionKey;
            aes.IV = encryptionIV;

            ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                {
                    byte[] dataBytes = Encoding.UTF8.GetBytes(data);
                    cryptoStream.Write(dataBytes, 0, dataBytes.Length);
                    cryptoStream.FlushFinalBlock();
                    return memoryStream.ToArray();
                }
            }
        }
    }

    private string Decrypt(byte[] encryptedData)
    {
        using (Aes aes = Aes.Create())
        {
            aes.Key = encryptionKey;
            aes.IV = encryptionIV;

            ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

            using (MemoryStream memoryStream = new MemoryStream(encryptedData))
            {
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader streamReader = new StreamReader(cryptoStream))
                    {
                        return streamReader.ReadToEnd();
                    }
                }
            }
        }
    }
}
