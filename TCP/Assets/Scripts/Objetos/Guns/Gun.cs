using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TipoArma
{
    Pistola,
    Rifle,
    Shotgun
}

public class Gun : MonoBehaviour
{
    [SerializeField] private PlayerAnim playerAnim;
    [SerializeField] private Player player;
    [SerializeField] private TipoArma _tipoArma;

    Vector2 mousePosi;
    Vector2 dirArma;

    float angle;

    [SerializeField] private SpriteRenderer _srGun;

   // [SerializeField] float tempoEntreTiros;
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

    #region MUNIÇÃO

    [SerializeField] private int numBullets;                  // número de balas a serem instanciadas
    [SerializeField] private float angleBetweenBullets;       // ângulo entre cada bala em graus
    [SerializeField] private float bulletSpread;              // desvio aleatório de cada bala em graus
    [SerializeField] private int _maxBalasGuardadas;          // numero maximo de balas guardadas da arma 
    [SerializeField] private int _balasGuardadas;             // numero de balas guardadas atualmente na arma   
    [SerializeField] private int _balasNoPente;               // numero de balas atualmente no pente da arma 
    [SerializeField] private int _tamanhoPente;               // tamanho maximo do pente da arma 
    [SerializeField] private int _balasRecarregadas = 0;      // numero de balas recarregadas 
    [SerializeField] private bool _recarregando = false;      // verificação pra saber se esta dando reload
    [SerializeField] private bool _recarregado = true;        // verific~ção pra saber se o reload ja acabou ou se a arma esta recarregada

         #region Get/Set
    public int maxBalasGuardadas
    {
        get { return _maxBalasGuardadas; }
        set { _maxBalasGuardadas = value; }
    }

    public int balasGuardadas
    {
        get { return _balasGuardadas; }
        set { _balasGuardadas = value; }
    }

    public int balasNoPente
    {
        get { return _balasNoPente; }
        set { _balasNoPente = value; }
    }

    public int tamanhoPente
    {
        get { return _tamanhoPente; }
        set { _tamanhoPente = value; }
    }

    public int balasRecarregadas
    {
        get { return _balasRecarregadas; }
        set { _balasRecarregadas = value; }
    }

    public bool recarregando
    {
        get { return _recarregando; }
        set { _recarregando = value; }
    }

    public bool recarregado
    {
        get { return _recarregado; }
        set { _recarregado = value; }
    }

    #endregion

    #endregion

    public SpriteRenderer srGun
    {
        get { return _srGun; }
        set { _srGun = value; }
    }

    public TipoArma tipoArma
    {
        get { return _tipoArma; }
        set { _tipoArma = value; }
    }

    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
        srGun = GetComponent<SpriteRenderer>();
        player = GetComponent<Player>();    
        GameObject playerObject = GameObject.Find("Player");
        playerAnim = playerObject.GetComponent<PlayerAnim>();



        if (tipoArma == TipoArma.Pistola)
        {
            maxBalasGuardadas = 60;
            balasGuardadas = Random.Range(10, 35);
            balasNoPente = Random.Range(7, 12);
            tamanhoPente = 12;
            numBullets = 1;
            angleBetweenBullets = 0f;
            bulletSpread = 0f;
        }
        else if (tipoArma == TipoArma.Rifle)
        {
            maxBalasGuardadas = 120;
            balasGuardadas = Random.Range(10, 30);
            balasNoPente = Random.Range(15, 30);
            tamanhoPente = 30;
            numBullets = 1;
            angleBetweenBullets = 0f;
            bulletSpread = 0f;
        }
        else if (tipoArma == TipoArma.Shotgun)
        {
            maxBalasGuardadas = 24;
            balasGuardadas = Random.Range(10, 24);
            balasNoPente = Random.Range(7, 12);
            tamanhoPente = 12;
            numBullets = 5;
            angleBetweenBullets = 10f;
            bulletSpread = 5f;
        }

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
                        playerAnim.equipado = true;
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

            if (Input.GetMouseButton(0) && podeAtirar && balasNoPente > 0)
            {
                podeAtirar = false;
                anim.SetTrigger("OnFire");              

                for (int i = 0; i < numBullets; i++)
                {
                    float angle = (i - (numBullets - 1) / 2f) * angleBetweenBullets; // ângulo entre cada bala
                    angle += Random.Range(-bulletSpread, bulletSpread); // adicionar um pequeno desvio aleatório

                    GameObject bulletObj = Instantiate(tiro, pontoDeFogo.position, pontoDeFogo.rotation);
                    bulletObj.transform.rotation = Quaternion.Euler(0, 0, pontoDeFogo.rotation.eulerAngles.z + angle - 90f);


                   
                }
                balasNoPente--; // gasta uma bala para cada bala disparada
            }



            if (Input.GetKeyDown(KeyCode.Q))
            {
                DropGun();
                playerAnim.equipado = false;
            }

            if (Input.GetKeyDown(KeyCode.R) || (Input.GetMouseButton(0)&&  podeAtirar && balasNoPente <= 0) )
            {              
                RecargaArma();
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


    void RecargaArma()
    {
        if (!recarregando && balasGuardadas > 0 && balasNoPente < tamanhoPente)
        {
            podeAtirar = false;
            anim.SetTrigger("OnReload");
            recarregando = true;
        }
    }

    void EndReload()
    {
        int balasParaRecarregar = tamanhoPente - balasNoPente;
        int balasDisponiveis = Mathf.Min(balasGuardadas, balasParaRecarregar);
        balasNoPente += balasDisponiveis;
        balasGuardadas -= balasDisponiveis;

        podeAtirar = true;
        recarregando = false;
        recarregado = true;
    }



}
