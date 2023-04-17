using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnim : MonoBehaviour
{ 
    private Player player;
    private Gun gun;
    private Animator _anim;
    public bool equipado;
    


    public Animator anim
    {
        get { return _anim; }
        set { _anim = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Player>();
        gun = GetComponent<Gun>();
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
               anim.SetInteger("OnGun", 3);
           }
           else
           {
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
                anim.SetTrigger("isRollGun");
                player.isRolling = false;
                player.canRoll = false;
            }
            else
            {
                anim.SetTrigger("isRoll");
                player.isRolling = false;
                player.canRoll = false;
            }
            

           

        }
    }

    void OnRollComplete()
    {
        player.canRoll = true;
        // define o valor de alpha como o valor padrão

    }



    #endregion





}
