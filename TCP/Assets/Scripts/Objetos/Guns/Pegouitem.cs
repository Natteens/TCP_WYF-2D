using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pegouitem : MonoBehaviour
{
    [SerializeField] private PlayerAnim playerAnim;
    [SerializeField] private Player player;
    [SerializeField] private TipoArma tipoArma;
    [SerializeField] private Gun gun;
    public LayerMask layerJogador;

    [SerializeField] float distanciaMaxima = 2f;
   

    private void Update()
    {
        PegarArma();
    }
    void PegarArma()
    {
       
        
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, distanciaMaxima, layerJogador);

            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].tag == "Player")
                {
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        switch (tipoArma)
                        {
                            case TipoArma.Rifle:
                               
                                gun.rifleDesbloqueado = true;
                                gun.numArmasDesbloqueadas++;
                                break;
                            case TipoArma.Shotgun:
                                gun.shotgunDesbloqueado = true;
                                gun.numArmasDesbloqueadas++;
                                break;
                            case TipoArma.Pistola:
                                gun.pistolaDesbloqueado = true;
                                gun.numArmasDesbloqueadas++;
                                break;
                        }
                   
                         gun.estaNaMao = true;                      
                        Destroy(gameObject);
                    }

                }
            }
        


    }
}
/*                          DROP GUN
   void DropGun()
   {

       estaNaMao = false;
       rb2d.isKinematic = false;
       col.enabled = true;
       transform.parent = null;
       anim.SetInteger("transition", 1);
       playerAnim.anim.SetInteger("OnGun", 0);

       // Determine a dire��o para onde jogar a arma
       Vector2 dropDirection = (mousePosi - (Vector2)transform.position).normalized;

       // Adicione a for�a para jogar a arma nessa dire��o
       rb2d.AddForce(dropDirection * DropSpeed, ForceMode2D.Impulse);

       // Reduza gradualmente a velocidade da arma
       StartCoroutine(ReduceSpeed());

   }

   IEnumerator ReduceSpeed()
   {
       yield return new WaitForSeconds(0.2f);

       rb2d.velocity = Vector2.zero;
       rb2d.angularVelocity = 0f;

       yield return new WaitForSeconds(0.1f);

       rb2d.velocity = Vector2.zero;
       rb2d.angularVelocity = 0f;

       yield return new WaitForSeconds(0.1f);

       rb2d.velocity = Vector2.zero;
       rb2d.angularVelocity = 0f;

   }
   */