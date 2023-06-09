using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageKnife : MonoBehaviour
{
    public PlayerAnim  playerAnim;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            // Aplicar dano ao inimigo
            Inimigo inimigo = other.GetComponent<Inimigo>();
            inimigo.vidaAtual -= playerAnim.Dano;
        }
    }
}
