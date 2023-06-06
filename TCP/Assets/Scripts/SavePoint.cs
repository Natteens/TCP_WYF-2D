using UnityEngine;

public class SavePoint : MonoBehaviour
{
    public GameSaveManager saveManager;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Jogo salvando...");
        if (other.CompareTag("Player"))
        {
            // Salva o jogo quando o jogador interage com o save point
            saveManager.SaveGame();
            Debug.Log("Jogo salvo!");
        }
    }
}
