using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    public float destroyedDelay = 0.5f;
    public LayerMask layersToIgnore;
    public ParticleSystem effect;
    public float knockbackForce = 5f; // Força do knockback
    [SerializeField] private TipoArma tipoArma;

    [Space(20)]
    [Header("Configurações de Dano")]

    [Range(1,100)]
    [SerializeField] private int PistolaDamage;
    [Range(1, 100)]
    [SerializeField] private int ShotgunDamage;
    [Range(1, 100)]
    [SerializeField] private int RifleDamage;


    
    private void Update()
    {
        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((layersToIgnore & 1 << collision.gameObject.layer) != 0) // Se a layer está na máscara de layers a ignorar, retorna sem fazer nada
        {
            return;
        }

        if (collision.CompareTag("Player") || collision.CompareTag("Weapon"))
        {
            // Não faz nada se colidir com o player ou a arma
            return;
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy")) // Verifica se a colisão ocorreu com um objeto que tem a camada "Inimigo"
        {
            Inimigo inimigo = collision.GetComponent<Inimigo>();
             
            switch (tipoArma)
            {
                case TipoArma.Pistola:
                    
                    inimigo.vidaAtual -= PistolaDamage;
                    break;

                case TipoArma.Rifle:
                    inimigo.vidaAtual -= ShotgunDamage;
                    break;

                case TipoArma.Shotgun:
                   
                    inimigo.vidaAtual -= ShotgunDamage;
                    break;
            }
        }

        Instantiate(effect, transform.position, transform.rotation);
        Destroy(gameObject, destroyedDelay);
    }
}
