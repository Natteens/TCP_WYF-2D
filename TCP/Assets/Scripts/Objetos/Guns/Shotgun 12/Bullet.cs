using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
  //  public Animator anim;
    public float destroyedDelay = 0.5f;
    public LayerMask layersToIgnore;
    [SerializeField] ParticleSystem effect; 

    private void Update()
    {
        transform.Translate(Vector2.up * speed * Time.deltaTime);
       // anim.SetInteger("transition", 1);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((layersToIgnore & 1 << collision.gameObject.layer) != 0) // Se a layer está na máscara de layers a ignorar, retorna sem fazer nada
        {
            return;
        }

        if (collision.CompareTag("Player") || collision.CompareTag("Weapon"))
        {
            // Não faz nada se colidir com o player
            return;
        }

        Instantiate(effect, transform.position, transform.rotation);
      //  anim.SetTrigger("OnHit");
        Destroy(gameObject, destroyedDelay);
    }
}
