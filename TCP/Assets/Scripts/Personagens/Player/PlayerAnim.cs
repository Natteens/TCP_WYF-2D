using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnim : MonoBehaviour
{ 
    private Player player;
    private Animator _anim;
    private Gun gun;
    public bool equipado;
    public Controle controle;

    [Space(10)]
    [Header("-------------Atacando---------")]
    [SerializeField] public bool atacando;
    [SerializeField] private GameObject attackCollider;
    [Range(1, 500)]
    [SerializeField] public int Dano;

    public Animator anim
    {
        get { return _anim; }
        set { _anim = value; }
    }

    private void Awake()
    {
       
    }

    void Start()
    {
        player = GetComponent<Player>();
        gun = FindAnyObjectByType<Gun>();                  
        anim = GetComponent<Animator>();
        equipado = false;
    }

    void Update()
    {
        if (!player.estaMorto)
        {
            OnMove();
            OnRun();
            OnRolling();
            Atk(); 
        } 
    }

    private void FixedUpdate()
    {
        if(!equipado && gun.numArmasDesbloqueadas > 0)
        {
            gun.podeTrocarArma = true;
        }
    }

    #region Movement

    void OnMove()
    {
        if (player.direction.sqrMagnitude > 0) // sqr magnitude retorna a media do x e y 
        {      
           if(equipado)
           {
             anim.SetInteger("transition", 0);
             anim.SetInteger("OnGun", 2);
          }
          else
          {
                anim.SetInteger("OnGun", 0);
                anim.SetInteger("transition", 2);
           }
            
                     
        }
        else
        {
            if (equipado)
            {
                anim.SetInteger("transition", 0);
                anim.SetInteger("OnGun", 1);
            }
            else
            {
                anim.SetInteger("OnGun", 0);
                anim.SetInteger("transition", 1);
            }
        }

        if (player.direction.x > 0)
        {
            transform.eulerAngles = new Vector2(0, 0);
        }
        if (player.direction.x < 0)
        {
            transform.eulerAngles = new Vector2(0, 180);
        }
    }


    void OnRun()
    {
        if(player.isRunning)
        {

           if (equipado)
           {
                anim.SetInteger("transition", 0);
                anim.SetInteger("OnGun", 3);
           }
           else
           {
                anim.SetInteger("OnGun", 0);
                anim.SetInteger("transition", 3);
           }
        }
       
    }

    #endregion

    #region Rolling

    void OnRolling()
    {
        StartCoroutine(OnRoll());
    }

    IEnumerator OnRoll()
    {
        if (player.isRolling && !anim.GetCurrentAnimatorStateInfo(0).IsName("Roll"))
        {
            anim.SetTrigger("isRoll");         
            gun.srGun.color = new Color(gun.srGun.color.r, gun.srGun.color.g, gun.srGun.color.b, 0f);
            gun.podeAtirar = false;
            // equipado = false;

            yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);

            player.canRoll = true;
            gun.srGun.color = new Color(gun.srGun.color.r, gun.srGun.color.g, gun.srGun.color.b, 1f);
            gun.podeAtirar = true;
            // equipado = true;
        }
    }

    IEnumerator OnRollComplete()
    {
        player.canRoll = true;
        player.isRolling = false;
        player.canRoll = false;
        gun.srGun.color = new Color(gun.srGun.color.r, gun.srGun.color.g, gun.srGun.color.b, 1f);
        gun.podeAtirar = true;
        // equipado = true;

        yield return null;
    }

    #endregion

    #region ataque
    public void Atk()
    {
        if (Input.GetMouseButtonUp(0) && !equipado && !atacando && controle.EstaminaAtual >= 20)
        {
            atacando = true;
            anim.SetBool("atk", true);
            player.canMove = false;
            player.canRoll = false;
            player.direction = Vector2.zero;
            controle.EstaminaAtual -= 20;
        }
    }

    public void EndAtk()
    {
        atacando = false;
        player.canMove = true;
        player.canRoll = true;
        anim.ResetTrigger("atk");
    }
    #endregion
    public void TomandoDano()
    {
        if(!equipado)
        {
            anim.SetTrigger("TomoDano");
        }
        else
        {
            anim.SetTrigger("TomoDanoGun");
        }
        
    }

  
}
