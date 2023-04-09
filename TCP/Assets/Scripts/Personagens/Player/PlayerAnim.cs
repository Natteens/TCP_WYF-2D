using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnim : MonoBehaviour
{ 
    private Player player;
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Player>();
        anim = GetComponent<Animator>();
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
            
            anim.SetInteger("transition", 2); 
                     
        }
        else
        {
            anim.SetInteger("transition", 1);
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
            anim.SetInteger("transition", 3);
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
            
        }
    }


    void OnRollComplete()
    {
        player.canRoll = true;      
        Debug.Log("COMPLETO A ANIM");
    }

    #endregion





}
