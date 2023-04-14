using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnim : MonoBehaviour
{ 
    private Player player;
    private Animator anim;

    [SerializeField]private Gun _currentGun;
    public Gun currentGun // Declara a propriedade pública para acessar e modificar a arma atual
    {
        get { return _currentGun; }
        set { _currentGun = value; }
    }

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
            if(currentGun != null)
            {
                anim.SetInteger("OnGun", 2);
            }
            else
            {
                anim.SetInteger("transition", 2);
            }
            
                     
        }
        else
        {
            if (currentGun != null)
            {
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

            if (currentGun != null)
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
