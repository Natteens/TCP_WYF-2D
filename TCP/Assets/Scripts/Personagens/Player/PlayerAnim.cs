using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnim : MonoBehaviour
{ 
    private Player player;
    private Animator _anim;
    private Gun gun;
    public bool equipado;
    



    public Animator anim
    {
        get { return _anim; }
        set { _anim = value; }
    }

    private void Awake()
    {
       
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
           anim.SetTrigger("isRoll");
           player.isRolling = false;
           player.canRoll = false;
           gun.srGun.color = new Color(gun.srGun.color.r, gun.srGun.color.g, gun.srGun.color.b, 0f);
           gun.podeAtirar = false;
        }
        
    }

    void OnRollComplete()
    {
        player.canRoll = true;      
        gun.srGun.color = new Color(gun.srGun.color.r, gun.srGun.color.g, gun.srGun.color.b, 1f);
        gun.podeAtirar = true;

    }

    #endregion





}
