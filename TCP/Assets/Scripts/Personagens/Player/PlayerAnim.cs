using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnim : MonoBehaviour
{ 
    private Player player;
    GameObject myObject;

    private Animator _anim;
    public bool equipado;
    private Gun gun;


    public Animator anim
    {
        get { return _anim; }
        set { _anim = value; }
    }

    private void Awake()
    {
        myObject = GameObject.Find("arma");
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Player>();
        gun = FindAnyObjectByType<Gun>();                  
        anim = GetComponent<Animator>();
        equipado = false;
    }

    // Update is called once per frame
    void Update()
    {
        OnMove();
        OnRun();
        OnRoll();

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


    void OnRoll()
    {
        if (player.isRolling && !anim.GetCurrentAnimatorStateInfo(0).IsName("Roll"))
        {
            if(equipado)        
            {
                anim.SetTrigger("isRoll");
                player.isRolling = false;
                player.canRoll = false;
                myObject.SetActive(false);
            }
            else if (!equipado && gun.numArmasDesbloqueadas > 0)
            {
                anim.SetTrigger("isRoll");
                player.isRolling = false;
                player.canRoll = false;
                myObject.SetActive(false);
            }

        }
    }

    void OnRollComplete()
    {
        player.canRoll = true;
        if(gun.numArmasDesbloqueadas > 0 &&  gun.tipoArma != TipoArma.Nenhum)
        {
            myObject.SetActive(true);
        }
        
    }



    #endregion





}
