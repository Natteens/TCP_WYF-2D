using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TipoArma
{
    Pistola,
    Rifle,
    Shotgun,
    Nenhum
}

public class Gun : MonoBehaviour
{
    [SerializeField] private PlayerAnim playerAnim;
    [SerializeField] private Player player;
    [SerializeField] private TipoArma _tipoArma;

    Vector2 mousePosi;
    Vector2 dirArma;

    float angle;

    [SerializeField] private SpriteRenderer srGun;

    bool podeAtirar = true;

    [SerializeField] Transform pontoDeFogo;
    [SerializeField] private Vector3 posicaoPontoDeFogoPistola = new Vector3(0f, 0f, 0f);
    [SerializeField] private Vector3 posicaoPontoDeFogoRifle = new Vector3(1.2f, 0.07f, 0f);
    [SerializeField] private Vector3 posicaoPontoDeFogoShotgun = new Vector3(0.7f, 0f, 0f);

    [SerializeField] GameObject tiro;

    [SerializeField] float distanciaMaxima = 2f;
    public LayerMask layerJogador;
    [SerializeField] float DropSpeed;
   
    public bool estaNaMao = false;

    Rigidbody2D rb2d;
    Collider2D col;
    Animator anim;

    #region ARMAS E MUNIÇÃO

    [SerializeField] private int armaAtual = 0; // índice da arma atual
    [SerializeField] private int numArmasDesbloqueadas = 3; // quantidade total de armas desbloqueadas


    [SerializeField] private bool _rifleDesbloqueado = false; // indica se o rifle está desbloqueado
    [SerializeField] private bool _shotgunDesbloqueado = false; // indica se a shotgun está desbloqueada
    [SerializeField] private bool _pistolaDesbloqueado = false; // indica se a pistola está desbloqueada

    public bool rifleEquipado = false;
    public bool shotgunEquipado = false;
    public bool pistolaEquipado = false;

    [SerializeField] private RuntimeAnimatorController  rifle, shotgun, pistola;
    [SerializeField] private Sprite spr_rifle, spr_shotgun, spr_pistola;
    #region Armas randomicas 

    //   [SerializeField] private int RifleBalasGuardadas;
    //   [SerializeField] private int RifleBalasNoPente;
    //  
    //   [SerializeField] private int ShotgunBalasGuardadas;
    //   [SerializeField] private int ShotgunBalasNoPente;
    //                                      
    //   [SerializeField] private int PistolaBalasGuardadas;
    //   [SerializeField] private int PistolaBalasNoPente;

    #endregion

    #region Balas

    [SerializeField] private int numBullets;                  // número de balas a serem instanciadas
    [SerializeField] private float angleBetweenBullets;       // ângulo entre cada bala em graus
    [SerializeField] private float bulletSpread;              // desvio aleatório de cada bala em graus
    [SerializeField] private int _maxBalasGuardadas;          // numero maximo de balas guardadas da arma 
    [SerializeField] private int _balasGuardadas;             // numero de balas guardadas atualmente na arma   
    [SerializeField] private int _balasNoPente;               // numero de balas atualmente no pente da arma 
    [SerializeField] private int _tamanhoPente;               // tamanho maximo do pente da arma 
    [SerializeField] private int _balasRecarregadas = 0;      // numero de balas recarregadas 
    [SerializeField] private bool _recarregando = false;      // verificação pra saber se esta dando reload
    [SerializeField] private bool _recarregado = true;        // verificção pra saber se o reload ja acabou ou se a arma esta recarregada
    #endregion


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

    #region GET/SET TIPO ARMA E ARMA DESBLOQUEADA
    public TipoArma tipoArma
    {
        get { return _tipoArma; }
        set { _tipoArma = value; }
    }

    public bool rifleDesbloqueado
    {
        get { return _rifleDesbloqueado; }
        set { _rifleDesbloqueado = value; }
    }

    public bool shotgunDesbloqueado
    {
        get { return _shotgunDesbloqueado; }
        set { _shotgunDesbloqueado = value; }
    }

    public bool pistolaDesbloqueado
    {
        get { return _pistolaDesbloqueado; }
        set { _pistolaDesbloqueado = value; }
    }
    #endregion
    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        srGun = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();       
        player = GetComponent<Player>();    
        GameObject playerObject = GameObject.Find("Player");
        playerAnim = playerObject.GetComponent<PlayerAnim>();

    }



    void Update()
    {
       // PegarArma();
        Atirar();
       // DroparArma();
        ReloadArma();
        inputArma();

        #region  TIPO ARMA RANDOMIZAÇÃO
        if (tipoArma == TipoArma.Pistola)
        {
            maxBalasGuardadas = 60;
            balasGuardadas = 25;
            balasNoPente = 7;
            tamanhoPente = 12;
            numBullets = 1;
            angleBetweenBullets = 0f;
            bulletSpread = 0f;
            pontoDeFogo.localPosition = posicaoPontoDeFogoPistola;
        }
        else if (tipoArma == TipoArma.Rifle)
        {
            maxBalasGuardadas = 120;
            balasGuardadas = 9 ;
            balasNoPente = 17 ;
            tamanhoPente = 30;
            numBullets = 1;
            angleBetweenBullets = 0f;
            bulletSpread = 0f;
            pontoDeFogo.localPosition = posicaoPontoDeFogoRifle;
        }
        else if (tipoArma == TipoArma.Shotgun)
        {
            maxBalasGuardadas = 24;
            balasGuardadas = 14;
            balasNoPente = 5;
            tamanhoPente = 12;
            numBullets = 5;
            angleBetweenBullets = 10f;
            bulletSpread = 5f;
            pontoDeFogo.localPosition = posicaoPontoDeFogoShotgun;
        }
        #endregion
    }

    void FixedUpdate()
    {
        FollowMouse();
    }

    void FollowMouse()
    {
        if (estaNaMao && armaAtual > 0)
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

    #region pegar arma 
    /*
    void PegarArma()
    {
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
    }           
    */
    #endregion
    void Atirar()
    {
        if (estaNaMao && armaAtual > 0 )
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
        }
    }

    /*
     * 
    void DroparArma()
    {
        if (estaNaMao)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                DropGun();
                playerAnim.equipado = false;
            }
        }
           
    }

    */

    void ReloadArma()
    {
        if (estaNaMao && armaAtual > 0)
        {
            if (Input.GetKeyDown(KeyCode.R) || (Input.GetMouseButton(0) && podeAtirar && balasNoPente <= 0))
            {
                RecargaArma();
            }
        }
        
    }

    void inputArma()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        // checa input do scroll do mouse
        if (scroll > 0f)
        {
            // scroll para cima: alterna para a próxima arma
            armaAtual++;
        }
        else if (scroll < 0f)
        {
            // scroll para baixo: alterna para a arma anterior
            armaAtual--;
        }

        // Verifica se a arma atual está desbloqueada
        bool armaDesbloqueada = false;
        switch (armaAtual)
        {
            case 0: // Nenhuma arma equipada
                armaDesbloqueada = true;
                break;

            case 1: // Rifle
                armaDesbloqueada = rifleDesbloqueado;
                break;

            case 2: // Shotgun
                armaDesbloqueada = shotgunDesbloqueado;
                break;

            case 3: // Pistola
                armaDesbloqueada = pistolaDesbloqueado;
                break;

            default: // Nenhuma arma equipada
                armaDesbloqueada = true;
                break;
        }

        // checa input do scroll do mouse
        if (armaDesbloqueada)
        {
            switch (armaAtual)
            {
                case 0: // Nenhuma arma equipada
                    srGun.sprite = null;
                    anim.runtimeAnimatorController = null;
                    tipoArma = TipoArma.Nenhum;
                    playerAnim.equipado = false;
                    break;

                case 1: // Rifle
                    srGun.sprite = spr_rifle;
                    anim.runtimeAnimatorController = rifle;
                    tipoArma = TipoArma.Rifle;
                    playerAnim.equipado = true;
                    srGun.sortingLayerName = "Player ARM";
                    break;

                case 2: // Shotgun
                    srGun.sprite = spr_shotgun;
                    anim.runtimeAnimatorController = shotgun;
                    tipoArma = TipoArma.Shotgun;
                    playerAnim.equipado = true;
                    srGun.sortingLayerName = "Player ARM";
                    break;

                case 3: // Pistola
                    srGun.sprite = spr_pistola;
                    anim.runtimeAnimatorController = pistola;
                    tipoArma = TipoArma.Pistola;
                    playerAnim.equipado = true;
                    srGun.sortingLayerName = "Player ARM";
                    break;

                default: // Nenhuma arma equipada
                    srGun.sprite = null;
                    anim.runtimeAnimatorController = null;
                    tipoArma = TipoArma.Nenhum;
                    playerAnim.equipado = false;
                    break;
            }
        }
        else
        {
            // Mantém a arma atual se ela não estiver desbloqueada
            armaAtual--;
        }

        // Limita a seleção de armas ao intervalo válido
        armaAtual = Mathf.Clamp(armaAtual, 0, 3);

        // Faz com que a arma atual seja sempre um número entre 0 e 3
        armaAtual %= 4;
    }



    void CDTiro()
    {
        podeAtirar = true;
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
