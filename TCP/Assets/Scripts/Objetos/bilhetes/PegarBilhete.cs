using UnityEngine;

public class PegarBilhete : MonoBehaviour
{
    public Bilhetes bilhetesScript;
    public BilheteEnum bilhete;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.E))
            {            
                bilhetesScript.DesbloquearBilhete(bilhete);
                Destroy(gameObject);
            }
        }
    }
}
