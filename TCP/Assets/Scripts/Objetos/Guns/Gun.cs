using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private PlayerAnim playerAnim;
    [SerializeField] private Gun gun;


    Vector2 mousePosi;
    Vector2 dirArma;

    float angle;

    [SerializeField] SpriteRenderer srGun;

    [SerializeField] float tempoEntreTiros;
    bool podeAtirar = true;

    [SerializeField] Transform pontoDeFogo;
    [SerializeField] GameObject tiro;

    [SerializeField] float distanciaMaxima = 2f;
    [SerializeField] LayerMask layerJogador;
    [SerializeField] float DropSpeed;

    bool estaNaMao = false;

    Rigidbody2D rb2d;

    Collider2D col;

    Animator anim;

    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (estaNaMao)
        {
            srGun.sortingLayerName = "Player ARM";
        }
        else
        {
            srGun.sortingLayerName = "Enviroments";
        }

        if (!estaNaMao)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, distanciaMaxima, layerJogador);

            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].tag == "Player")
                {
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                       
                        estaNaMao = true;
                         playerAnim.currentGun = gun;
                        rb2d.isKinematic = true;
                        col.enabled = false;
                        transform.parent = colliders[i].transform;
                        transform.position = colliders[i].transform.position;
                        transform.localRotation = Quaternion.identity;
                        anim.SetInteger("transition", 2);

                    }
                }
            }
        }

        if (estaNaMao)
        {
            mousePosi = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (Input.GetMouseButton(0) && podeAtirar)
            {
                podeAtirar = false;
                anim.SetTrigger("OnFire");
                GameObject bulletObj = Instantiate(tiro, pontoDeFogo.position, pontoDeFogo.rotation);
                Vector2 direction = (mousePosi - (Vector2)pontoDeFogo.position).normalized;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                bulletObj.transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                DropGun();
                playerAnim.currentGun = null;
            }
        }
    }



    void FixedUpdate()
    {
        if (estaNaMao)
        {
            dirArma = mousePosi - new Vector2(transform.parent.position.x, transform.parent.position.y);
            angle = Mathf.Atan2(dirArma.y, dirArma.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
            if (angle > -90f && angle < 90f)
            {
                srGun.flipY = false;
            }
            else
            {
                srGun.flipY = true;
            }

        }
    }

    void CDTiro()
    {
        podeAtirar = true;
    }

    void DropGun()
    {
        
        estaNaMao = false;
        rb2d.isKinematic = false;
        col.enabled = true;
        transform.parent = null;
        anim.SetInteger("transition", 1);


        // Determine a direção para onde jogar a arma
        Vector2 dropDirection = (mousePosi - (Vector2)transform.position).normalized;

        // Adicione a força para jogar a arma nessa direção
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


}
