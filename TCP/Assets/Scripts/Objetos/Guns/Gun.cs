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
    
    [Header("Tipo de Arma")]

    [SerializeField] private PlayerAnim playerAnim;
    [SerializeField] private TipoArma _tipoArma;
    [SerializeField] private Animator anim;
    [SerializeField] private SpriteRenderer _srGun;

    [Space(20)]
    [Header("Bullets")]
    [SerializeField] GameObject PistolaTiro;
    [SerializeField] GameObject ShotgunTiro;
    [SerializeField] GameObject RifleTiro;
  
    
    [Space(20)]
    [Header("Posição da Mira")]
    [SerializeField] Transform pontoDeFogo;
    [SerializeField] private Vector3 posicaoPontoDeFogoPistola = new Vector3(0f, 0f, 0f);
    [SerializeField] private Vector3 posicaoPontoDeFogoRifle = new Vector3(1.2f, 0.07f, 0f);
    [SerializeField] private Vector3 posicaoPontoDeFogoShotgun = new Vector3(0.7f, 0f, 0f);   
    [SerializeField] private bool _podeAtirar = false;
    public LayerMask layerJogador;
    float angle;

    #region ARMAS E MUNIÇÃO

     [Space(20)]
     [Header("Armas")]

    public List<TipoArma> armasDesbloqueadas = new List<TipoArma>();
    public bool podeTrocarArma = true;
    bool AnimRecarga = false;
    [SerializeField] private int armaAtual = 0; // valor da arma atual
    [SerializeField] public bool armaDesbloqueada = false;
    [SerializeField] public int numArmasDesbloqueadas;
    [SerializeField] private bool _nadaDesbloqueado = true;   
    [SerializeField] private bool _rifleDesbloqueado = false;   // indica se o rifle está desbloqueado
    [SerializeField] private bool _shotgunDesbloqueado = false; // indica se a shotgun está desbloqueada
    [SerializeField] private bool _pistolaDesbloqueado = false; // indica se a pistola está desbloqueada
  
    [SerializeField] private RuntimeAnimatorController rifle, shotgun, pistola;

    Vector2 mousePosi;
    Vector2 dirArma;
    #region Balas

    [Space(20)]
    [Header("Balas")]
    [SerializeField] private int _balasRecarregadas = 0;    
    [SerializeField] private bool _recarregando = false;      
    public float tempoDeTroca = 2f;
    public int tempoMaximo = 3;
    public float tempoDecorrido = 0f;

    #region VAR ARMAS
    // Pistola
  [HideInInspector]
  public int pistolaMaxBalasGuardadas;

  public int pistolaBalasGuardadas;
  public int pistolaBalasNoPente;

  [HideInInspector]
  public int pistolaTamanhoPente;
  [HideInInspector]
  public int pistolaNumBullets;
  [HideInInspector]
  public float pistolaAngleBetweenBullets;
  [HideInInspector]
  public float pistolaBulletSpread;

  // Shotgun
  [HideInInspector]
  public int shotgunMaxBalasGuardadas;

  public int shotgunBalasGuardadas;
  public int shotgunBalasNoPente;

  [HideInInspector]
  public int shotgunTamanhoPente;
  [HideInInspector]
  public int shotgunNumBullets;
  [HideInInspector]
  public float shotgunAngleBetweenBullets;
  [HideInInspector]
  public float shotgunBulletSpread;

 
  // Rifle
  [HideInInspector]
  public int rifleMaxBalasGuardadas;

  public int rifleBalasGuardadas;
  public int rifleBalasNoPente;

  [HideInInspector]
  public int rifleTamanhoPente;
  [HideInInspector]
  public int rifleNumBullets;
  [HideInInspector]
  public float rifleAngleBetweenBullets;
  [HideInInspector]
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
        GameSaveManager.instance.gun = this;
        srGun = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        GameObject playerObject = GameObject.Find("Player");
        playerAnim = playerObject.GetComponent<PlayerAnim>();
    }

    void Start()
    {
       
        
        armasDesbloqueadas.Add(TipoArma.Nenhum);
        armasDesbloqueadas.Add(TipoArma.Pistola);
        armasDesbloqueadas.Add(TipoArma.Rifle);
        armasDesbloqueadas.Add(TipoArma.Shotgun);
        if (GameSaveManager.instance.SaveExists() == false)
        {

            pistolaBalasGuardadas = Random.Range(10, 35);
            pistolaBalasNoPente = Random.Range(7, 12);

            rifleBalasGuardadas = Random.Range(10, 30);
            rifleBalasNoPente = Random.Range(15, 30);

            shotgunBalasGuardadas = Random.Range(10, 24);
            shotgunBalasNoPente = Random.Range(7, 12);

        }

        pistolaMaxBalasGuardadas = 60;     
        pistolaTamanhoPente = 12;
        pistolaNumBullets = 1;
        pistolaAngleBetweenBullets = 0f;
        pistolaBulletSpread = 0f;

        rifleMaxBalasGuardadas = 120;
        rifleTamanhoPente = 30;
        rifleNumBullets = 1;
        rifleAngleBetweenBullets = 0f;
        rifleBulletSpread = 0f;

        shotgunMaxBalasGuardadas = 24;
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
         
     switch (tipoArma)
     {
         case TipoArma.Pistola: // Pistola
          if (Input.GetMouseButtonUp(0) && podeAtirar )
         {
             if (tipoArma == TipoArma.Pistola && pistolaBalasNoPente > 0)
             {
                 anim.SetTrigger("OnFire");

                 for (int i = 0; i < pistolaNumBullets; i++)
                 {
                     float angle = (i - (pistolaNumBullets - 1) / 2f) * pistolaAngleBetweenBullets; // ângulo entre cada bala
                     angle += Random.Range(-pistolaBulletSpread, pistolaBulletSpread); // pequeno desvio aleatório

                     GameObject bulletObj = Instantiate(PistolaTiro, pontoDeFogo.position, pontoDeFogo.rotation);
                     bulletObj.transform.rotation = Quaternion.Euler(0, 0, pontoDeFogo.rotation.eulerAngles.z + angle - 90f);

                 }

                 pistolaBalasNoPente--;
                 podeAtirar = false;
                 NaoTrocarDeArma();

             }
         }     
             break;

         case TipoArma.Rifle: // Rifle
         if (Input.GetMouseButton(0) && podeAtirar)
         {
             if (tipoArma == TipoArma.Rifle && rifleBalasNoPente > 0)
             {
                 anim.SetTrigger("OnFire");

                 for (int i = 0; i < rifleNumBullets; i++)
                 {
                     float angle = (i - (rifleNumBullets - 1) / 2f) * rifleAngleBetweenBullets; // ângulo entre cada bala
                     angle += Random.Range(-rifleBulletSpread, rifleBulletSpread); // pequeno desvio aleatório

                     GameObject bulletObj = Instantiate(RifleTiro, pontoDeFogo.position, pontoDeFogo.rotation);
                     bulletObj.transform.rotation = Quaternion.Euler(0, 0, pontoDeFogo.rotation.eulerAngles.z + angle - 90f);

                 }

                 rifleBalasNoPente--;
                 podeAtirar = false;
                 NaoTrocarDeArma();
             }
         }
            
             break;

         case TipoArma.Shotgun: //Shotgun
         if (Input.GetMouseButtonUp(0) && podeAtirar)
         {
             if (tipoArma == TipoArma.Shotgun && shotgunBalasNoPente > 0)
             {
                 anim.SetTrigger("OnFire");

                 for (int i = 0; i < shotgunNumBullets; i++)
                 {
                     float angle = (i - (shotgunNumBullets - 1) / 2f) * shotgunAngleBetweenBullets; // ângulo entre cada bala
                     angle += Random.Range(-rifleBulletSpread, shotgunBulletSpread); //  desvio aleatório

                     GameObject bulletObj = Instantiate(ShotgunTiro, pontoDeFogo.position, pontoDeFogo.rotation);
                     bulletObj.transform.rotation = Quaternion.Euler(0, 0, pontoDeFogo.rotation.eulerAngles.z + angle - 90f);

                 }

                 shotgunBalasNoPente--;
                 podeAtirar = false;
                 NaoTrocarDeArma();
             }
         }
             
             break;
     }

    }

    void ReloadArma()
    {     
        if (!AnimRecarga)
        {
            switch (tipoArma)
            {
              case TipoArma.Pistola:
                  if (Input.GetKeyDown(KeyCode.R) && !recarregando && pistolaBalasGuardadas > 0 && pistolaBalasNoPente < pistolaTamanhoPente || (Input.GetMouseButton(0) && !recarregando && podeAtirar && pistolaBalasGuardadas > 0 && pistolaBalasNoPente <= 0))
                  {
                      recarregando = true;
                      AnimRecarga = true;
                      podeAtirar = false;
                      anim.SetTrigger("OnReload");
                      NaoTrocarDeArma();
                      AnimRecarga = true;
                  }
                  break;

              case TipoArma.Rifle:
                  if (Input.GetKeyDown(KeyCode.R) && !recarregando && rifleBalasGuardadas > 0 && rifleBalasNoPente < rifleTamanhoPente || (Input.GetMouseButton(0) && !recarregando && podeAtirar && rifleBalasGuardadas > 0 && rifleBalasNoPente <= 0))
                  {
                      recarregando = true;
                       AnimRecarga = true;
                      podeAtirar = false;
                      anim.SetTrigger("OnReload");
                       NaoTrocarDeArma();

                  }
                  break;

              case TipoArma.Shotgun:
                  if (Input.GetKeyDown(KeyCode.R) && !recarregando && shotgunBalasGuardadas > 0 && shotgunBalasNoPente < shotgunTamanhoPente || (Input.GetMouseButton(0) && !recarregando && podeAtirar && shotgunBalasGuardadas > 0 && shotgunBalasNoPente <= 0))
                  {
                      recarregando = true;
                      AnimRecarga = true;
                      podeAtirar = false;
                      anim.SetTrigger("OnReload");
                      NaoTrocarDeArma();

                  }
                  break;               
            } 
                   
        }
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
                PermitirTrocaDeArma();
                AnimRecarga = false;
                break;

            case TipoArma.Rifle:
                int rifleBalasParaRecarregar = rifleTamanhoPente - rifleBalasNoPente;
                int rifleBalasDisponiveis = Mathf.Min(rifleBalasGuardadas, rifleBalasParaRecarregar);
                rifleBalasNoPente += rifleBalasDisponiveis;
                rifleBalasGuardadas -= rifleBalasDisponiveis;
                podeAtirar = true;
                recarregando = false;
                PermitirTrocaDeArma();
                AnimRecarga = false;
                break;

            case TipoArma.Shotgun:
                int shotgunBalasParaRecarregar = shotgunTamanhoPente - shotgunBalasNoPente;
                int shotgunBalasDisponiveis = Mathf.Min(shotgunBalasGuardadas, shotgunBalasParaRecarregar);
                shotgunBalasNoPente += shotgunBalasDisponiveis;
                shotgunBalasGuardadas -= shotgunBalasDisponiveis;
                podeAtirar = true;
                recarregando = false;
                PermitirTrocaDeArma();
                AnimRecarga = false;
                break;

        }
        

    }

    void inputArma()
    {
        tempoDecorrido += Time.fixedDeltaTime;

        if (tempoDecorrido > tempoMaximo)
        {
            tempoDecorrido = tempoMaximo;
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll > 0f && podeTrocarArma && tempoDecorrido > tempoDeTroca)
        {
            armaAtual++;
            if (armaAtual >= armasDesbloqueadas.Count)
                armaAtual = numArmasDesbloqueadas;

            while (!ArmaDesbloqueada(armaAtual))
            {
                armaAtual++;
                if (armaAtual >= armasDesbloqueadas.Count)
                    armaAtual = numArmasDesbloqueadas;
            }

            tempoDecorrido = 0f;
            podeAtirar = true;
            PermitirTrocaDeArma();
        }
        else if (scroll < 0f && podeTrocarArma && tempoDecorrido > tempoDeTroca)
        {
            armaAtual--;
            if (armaAtual < 0)
                armaAtual = 0;

            while (!ArmaDesbloqueada(armaAtual))
            {
                armaAtual--;
                if (armaAtual < 0)
                    armaAtual = 0;
            }

            tempoDecorrido = 0f;
            podeAtirar = true;
            PermitirTrocaDeArma();
        }


        TipoArma armaSelecionada = ArmaAtualDesbloqueada();


        switch (armaSelecionada)
        {
            case TipoArma.Nenhum:
                anim.runtimeAnimatorController = null;
                tipoArma = TipoArma.Nenhum;
                playerAnim.equipado = false;
                srGun.sprite = null;
                pontoDeFogo.localPosition = Vector3.zero;
                PermitirTrocaDeArma();
                break;

            case TipoArma.Pistola:
                anim.runtimeAnimatorController = pistola;
                tipoArma = TipoArma.Pistola;
                playerAnim.equipado = true;
                recarregando = false;
                anim.SetInteger("transition", 2);
                srGun.sortingLayerName = "Player ARM";
                pontoDeFogo.localPosition = posicaoPontoDeFogoPistola;
                break;

            case TipoArma.Shotgun:
                anim.runtimeAnimatorController = shotgun;
                tipoArma = TipoArma.Shotgun;
                playerAnim.equipado = true;
                recarregando = false;
                anim.SetInteger("transition", 2);
                srGun.sortingLayerName = "Player ARM";
                pontoDeFogo.localPosition = posicaoPontoDeFogoShotgun;
                break;

            case TipoArma.Rifle:
                anim.runtimeAnimatorController = rifle;
                tipoArma = TipoArma.Rifle;
                playerAnim.equipado = true;
                recarregando = false;
                anim.SetInteger("transition", 2);
                srGun.sortingLayerName = "Player ARM";
                pontoDeFogo.localPosition = posicaoPontoDeFogoRifle;
                break;
        }
    }

    bool ArmaDesbloqueada(int indice)
    {
        switch (indice)
        {
            case 0:
                return _nadaDesbloqueado;
            case 1:
                return _pistolaDesbloqueado;
            case 2:
                return _shotgunDesbloqueado;
            case 3:
                return _rifleDesbloqueado;
            default:
                return false;
        }
    }

    TipoArma ArmaAtualDesbloqueada()
    {
        if (armaAtual == 0 && _nadaDesbloqueado)
        {
            return TipoArma.Nenhum;
        }
        else if (armaAtual == 1 && _pistolaDesbloqueado)
        {
            return TipoArma.Pistola;
        }
        else if (armaAtual == 2 && _shotgunDesbloqueado)
        {
            return TipoArma.Shotgun;
        }
        else if (armaAtual == 3 && _rifleDesbloqueado)
        {
            return TipoArma.Rifle;
        }
        else
        {
            return TipoArma.Nenhum; // Nenhuma arma equipada
        }
    }

    void CDTiro()
    { 
       podeAtirar = true;
       PermitirTrocaDeArma();
    }

    void PermitirTrocaDeArma()
    {
        podeTrocarArma = true;
    }

    void NaoTrocarDeArma()
    {
        podeTrocarArma = false;
    }

    void DesbloquearArma(TipoArma tipo)
    {
        if (!armasDesbloqueadas.Contains(tipo))
        {
            armasDesbloqueadas.Add(tipo);
        }
    }

    void BloquearArma(TipoArma tipo)
    {
        if (armasDesbloqueadas.Contains(tipo))
        {
            armasDesbloqueadas.Remove(tipo);
        }
    }

}


   






