using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum TipoArma
{
    Nenhum,
    Pistola,
    Shotgun,
    Rifle
}
public class Gun : MonoBehaviour
{
    [SerializeField] private PlayerAnim playerAnim;
    [SerializeField] private TipoArma _tipoArma;
    [SerializeField] private Animator anim;
    
    Vector2 mousePosi;
    Vector2 dirArma;

    [SerializeField] private SpriteRenderer _srGun;
    [SerializeField] GameObject tiro;

    [SerializeField] Transform pontoDeFogo;
    [SerializeField] private Vector3 posicaoPontoDeFogoPistola = new Vector3(0f, 0f, 0f);
    [SerializeField] private Vector3 posicaoPontoDeFogoRifle = new Vector3(1.2f, 0.07f, 0f);
    [SerializeField] private Vector3 posicaoPontoDeFogoShotgun = new Vector3(0.7f, 0f, 0f);

    float angle;
    [SerializeField] private bool _podeAtirar = false;
    public LayerMask layerJogador;
    public bool estaNaMao = false;


    #region ARMAS E MUNIÇÃO

    [SerializeField] private int armaAtual = 0; // valor da arma atual
    [SerializeField] public bool armaDesbloqueada = false;
    [SerializeField] public int numArmasDesbloqueadas;
    [SerializeField] private bool _nadaDesbloqueado = true;   
    [SerializeField] private bool _rifleDesbloqueado = false;   // indica se o rifle está desbloqueado
    [SerializeField] private bool _shotgunDesbloqueado = false; // indica se a shotgun está desbloqueada
    [SerializeField] private bool _pistolaDesbloqueado = false; // indica se a pistola está desbloqueada

    [SerializeField] private RuntimeAnimatorController rifle, shotgun, pistola;



    #region Balas

    [SerializeField] private int _balasRecarregadas = 0;      // numero de balas recarregadas 
    [SerializeField] private bool _recarregando = false;      // verificação pra saber se esta dando reload

    public float tempoDeTroca = 2f;
    public int tempoMaximo = 3;
    public float tempoDecorrido = 0f;

    #region VAR ARMAS
    // Pistola
    public int pistolaMaxBalasGuardadas;
    public int pistolaBalasGuardadas;
    public int pistolaBalasNoPente;
    public int pistolaTamanhoPente;
    public int pistolaNumBullets;
    public float pistolaAngleBetweenBullets;
    public float pistolaBulletSpread;

    // Shotgun
    public int shotgunMaxBalasGuardadas;
    public int shotgunBalasGuardadas;
    public int shotgunBalasNoPente;
    public int shotgunTamanhoPente;
    public int shotgunNumBullets;
    public float shotgunAngleBetweenBullets;
    public float shotgunBulletSpread;

    // Rifle
    public int rifleMaxBalasGuardadas;
    public int rifleBalasGuardadas;
    public int rifleBalasNoPente;
    public int rifleTamanhoPente;
    public int rifleNumBullets;
    public float rifleAngleBetweenBullets;
    public float rifleBulletSpread;

    #endregion

    #endregion


    #region Get/Set


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

  /*  public bool recarregado
    {
        get { return _recarregado; }
        set { _recarregado = value; }
    } */

    #endregion

    #endregion

    #region GET/SET TIPO ARMA E ARMA DESBLOQUEADA

    public bool podeAtirar
    {
        get { return _podeAtirar; }
        set { _podeAtirar = value; }
    }
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
    public bool nadaDesbloqueado
    {
        get { return _nadaDesbloqueado; }
        set { _nadaDesbloqueado = value; }
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
        srGun = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        GameObject playerObject = GameObject.Find("Player");
        playerAnim = playerObject.GetComponent<PlayerAnim>();

    }

    void Start()
    {
       
        pistolaMaxBalasGuardadas = 60;
        pistolaBalasGuardadas = Random.Range(10, 35);
        pistolaBalasNoPente = Random.Range(7, 12);
        pistolaTamanhoPente = 12;
        pistolaNumBullets = 1;
        pistolaAngleBetweenBullets = 0f;
        pistolaBulletSpread = 0f;



        rifleMaxBalasGuardadas = 120;
        rifleBalasGuardadas = Random.Range(10, 30);
        rifleBalasNoPente = Random.Range(15, 30);
        rifleTamanhoPente = 30;
        rifleNumBullets = 1;
        rifleAngleBetweenBullets = 0f;
        rifleBulletSpread = 0f;



        shotgunMaxBalasGuardadas = 24;
        shotgunBalasGuardadas = Random.Range(10, 24);
        shotgunBalasNoPente = Random.Range(7, 12);
        shotgunTamanhoPente = 12;
        shotgunNumBullets = 5;
        shotgunAngleBetweenBullets = 10f;
        shotgunBulletSpread = 5f;

    }

    void Update()
    {       
        inputArma();
        Atirar();
        ReloadArma();  
    }

    void FixedUpdate()
    {
        FollowMouse();

    }

    void FollowMouse()
    {
        if (armaAtual > 0)
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

    void Atirar()
    {
        mousePosi = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButton(0) && podeAtirar )
        {
          // if(armaAtual > 0)
            switch (tipoArma)
            {
                case TipoArma.Pistola:
                    if (tipoArma == TipoArma.Pistola && pistolaBalasNoPente > 0)
                    {
                        anim.SetTrigger("OnFire");

                        for (int i = 0; i < pistolaNumBullets; i++)
                        {
                            float angle = (i - (pistolaNumBullets - 1) / 2f) * pistolaAngleBetweenBullets; // ângulo entre cada bala
                            angle += Random.Range(-pistolaBulletSpread, pistolaBulletSpread); // pequeno desvio aleatório

                            GameObject bulletObj = Instantiate(tiro, pontoDeFogo.position, pontoDeFogo.rotation);
                            bulletObj.transform.rotation = Quaternion.Euler(0, 0, pontoDeFogo.rotation.eulerAngles.z + angle - 90f);
                        }

                        pistolaBalasNoPente--;
                        podeAtirar = false;
                    }
                    break;

                case TipoArma.Rifle:
                    if (tipoArma == TipoArma.Rifle && rifleBalasNoPente > 0)
                    {
                        anim.SetTrigger("OnFire");

                        for (int i = 0; i < rifleNumBullets; i++)
                        {
                            float angle = (i - (rifleNumBullets - 1) / 2f) * rifleAngleBetweenBullets; // ângulo entre cada bala
                            angle += Random.Range(-rifleBulletSpread, rifleBulletSpread); // pequeno desvio aleatório

                            GameObject bulletObj = Instantiate(tiro, pontoDeFogo.position, pontoDeFogo.rotation);
                            bulletObj.transform.rotation = Quaternion.Euler(0, 0, pontoDeFogo.rotation.eulerAngles.z + angle - 90f);
                        }

                        rifleBalasNoPente--;
                        podeAtirar = false;
                    }
                    break;

                case TipoArma.Shotgun:
                    if (tipoArma == TipoArma.Shotgun && shotgunBalasNoPente > 0)
                    {
                        anim.SetTrigger("OnFire");

                        for (int i = 0; i < shotgunNumBullets; i++)
                        {
                            float angle = (i - (shotgunNumBullets - 1) / 2f) * shotgunAngleBetweenBullets; // ângulo entre cada bala
                            angle += Random.Range(-rifleBulletSpread, shotgunBulletSpread); //  desvio aleatório

                            GameObject bulletObj = Instantiate(tiro, pontoDeFogo.position, pontoDeFogo.rotation);
                            bulletObj.transform.rotation = Quaternion.Euler(0, 0, pontoDeFogo.rotation.eulerAngles.z + angle - 90f);
                        }

                        shotgunBalasNoPente--;
                        podeAtirar = false;

                    }
                    break;
            }

          //  podeAtirar = false;
        }
    }

    void ReloadArma()
    {   
      switch (tipoArma)
      {
          case TipoArma.Pistola:
              if (Input.GetKeyDown(KeyCode.R) && !recarregando && pistolaBalasGuardadas > 0 && pistolaBalasNoPente < pistolaTamanhoPente || (Input.GetMouseButton(0) && !recarregando && podeAtirar && pistolaBalasGuardadas > 0 && pistolaBalasNoPente <= 0 ))
              {
                  podeAtirar = false;
                  anim.SetTrigger("OnReload");
                  recarregando = true;
              }
              break;

          case TipoArma.Rifle:
              if (Input.GetKeyDown(KeyCode.R) && !recarregando && rifleBalasGuardadas > 0 && rifleBalasNoPente < rifleTamanhoPente || (Input.GetMouseButton(0) && !recarregando && podeAtirar && rifleBalasGuardadas > 0 && rifleBalasNoPente <= 0))
              {
                  podeAtirar = false;
                  anim.SetTrigger("OnReload");
                  recarregando = true;
              }
              break;

          case TipoArma.Shotgun:
              if (Input.GetKeyDown(KeyCode.R) && !recarregando && shotgunBalasGuardadas > 0  && shotgunBalasNoPente < rifleTamanhoPente || (Input.GetMouseButton(0) && !recarregando && podeAtirar && shotgunBalasGuardadas > 0 && shotgunBalasNoPente <= 0))
              {
                  podeAtirar = false;
                  anim.SetTrigger("OnReload");
                  recarregando = true;
              }
              break;

             
      }                  
    }

    void inputArma()
    {
        tempoDecorrido += Time.fixedDeltaTime;
        if (Input.GetKeyDown(KeyCode.Alpha1) && nadaDesbloqueado && tempoDecorrido >= tempoDeTroca )
        {
            armaAtual = 0;
            tempoDecorrido = 0f;
            podeAtirar = true;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && pistolaDesbloqueado && tempoDecorrido >= tempoDeTroca)
        {
            armaAtual = 1;
            tempoDecorrido = 0f;
            podeAtirar = true;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) && shotgunDesbloqueado && tempoDecorrido >= tempoDeTroca)
        {
            armaAtual = 2;
            tempoDecorrido = 0f;
            podeAtirar = true;

        }
        else if (Input.GetKeyDown(KeyCode.Alpha4) && rifleDesbloqueado && tempoDecorrido >= tempoDeTroca)
        {
            armaAtual = 3;
            tempoDecorrido = 0f;
            podeAtirar = true;

        }


        if (tempoDecorrido > tempoMaximo)
        {
            tempoDecorrido = tempoMaximo;
        }

        switch (armaAtual)
        {
              case 0: // Nenhuma arma equipada
                  if (nadaDesbloqueado)
                  {
                      anim.runtimeAnimatorController = null;
                      tipoArma = TipoArma.Nenhum;
                      playerAnim.equipado = false;
                      srGun.sprite = null;
                      pontoDeFogo.localPosition = Vector3.zero;
                  }

                  break;


              case 1: // Pistola

                  if (pistolaDesbloqueado && armaAtual == 1)
                  {
                      anim.runtimeAnimatorController = pistola;
                      tipoArma = TipoArma.Pistola;
                      playerAnim.equipado = true;
                      anim.SetInteger("transition", 2);
                      srGun.sortingLayerName = "Player ARM";
                      pontoDeFogo.localPosition = posicaoPontoDeFogoPistola;
                  }

                  break;

              case 2: // Shotgun

                  if (shotgunDesbloqueado && armaAtual == 2)
                  {
                      anim.runtimeAnimatorController = shotgun;
                      tipoArma = TipoArma.Shotgun;
                      playerAnim.equipado = true;
                      anim.SetInteger("transition", 2);
                      srGun.sortingLayerName = "Player ARM";
                      pontoDeFogo.localPosition = posicaoPontoDeFogoShotgun;
                  }

                  break;


              case 3: // Rifle

                  if (rifleDesbloqueado && armaAtual == 3)
                  {
                      anim.runtimeAnimatorController = rifle;
                      tipoArma = TipoArma.Rifle;
                      playerAnim.equipado = true;
                      anim.SetInteger("transition", 2);
                      srGun.sortingLayerName = "Player ARM";
                      pontoDeFogo.localPosition = posicaoPontoDeFogoRifle;
                  }

                  break;

                  default: // Nenhuma arma equipada         
                  armaAtual = 0;               
                  break;
        }
               
    }
  
    void CDTiro()
    { 
       podeAtirar = true;   
    }


    void EndReload()
    {
        switch (tipoArma)
        {
            case TipoArma.Pistola:
                int pistolaBalasParaRecarregar = pistolaTamanhoPente - pistolaBalasNoPente;
                int pistolaBalasDisponiveis = Mathf.Min(pistolaBalasGuardadas, pistolaBalasParaRecarregar);
                pistolaBalasNoPente += pistolaBalasDisponiveis;
                pistolaBalasGuardadas -= pistolaBalasDisponiveis;
                podeAtirar = true;
                recarregando = false;
                break;

            case TipoArma.Rifle:
                int rifleBalasParaRecarregar = rifleTamanhoPente - rifleBalasNoPente;
                int rifleBalasDisponiveis = Mathf.Min(rifleBalasGuardadas, rifleBalasParaRecarregar);
                rifleBalasNoPente += rifleBalasDisponiveis;
                rifleBalasGuardadas -= rifleBalasDisponiveis;
                podeAtirar = true;
                recarregando = false;
                break;

            case TipoArma.Shotgun:
                int shotgunBalasParaRecarregar = shotgunTamanhoPente - shotgunBalasNoPente;
                int shotgunBalasDisponiveis = Mathf.Min(shotgunBalasGuardadas, shotgunBalasParaRecarregar);
                shotgunBalasNoPente += shotgunBalasDisponiveis;
                shotgunBalasGuardadas -= shotgunBalasDisponiveis;
                podeAtirar = true;
                recarregando = false;
                break;

        }

    }

    // SO TROCAR DE ARMA QUANDO AS ANIMS TERMINAREM   
} 





