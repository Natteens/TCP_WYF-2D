using UnityEngine;

public class SavePoint : MonoBehaviour
{

    private bool isPlayerInside = false;

    private void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false;
        }
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (isPlayerInside)
            {
                Debug.Log("Jogo salvando...");
                GameSaveManager.instance.SaveGame();
                Debug.Log("Jogo salvo!");
            }
        }
    }
}
