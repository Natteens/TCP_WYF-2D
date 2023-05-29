using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour

{

    [SerializeField] public float speed;
    [SerializeField] public float runSpeed; 
    [SerializeField] public float rollSpeed;
    [SerializeField] public float initialSpeed;
    [SerializeField] public bool canRoll = true;
    [SerializeField] private PlayerAnim playerAnim;
    [SerializeField] private Gun gun;
    [SerializeField] private Controle controle;

     [Range(1, 5)]public int energiaGasta;

    private Rigidbody2D rig;  
    private bool _isRunning;
    private bool _isRolling;
    private Vector2 _direction;
    
    #region get set

    public Vector2 direction
    {
        get { return _direction; }
        set { _direction = value; }
    }
    public bool isRunning
    {
        get { return _isRunning; }
        set { _isRunning = value; }
    }
    public bool isRolling
    {
        get { return _isRolling; }
        set { _isRolling = value; }
    }
    #endregion
    private void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        initialSpeed = speed;
    }

    void Update()
    {
           OnInput();
           OnRun();
           
           if (controle.EstaminaAtual >= 15)
        {
            OnRoll();
        }

    }

    void FixedUpdate()
    {          
        OnMove();       
    }

    #region Movement

    void OnInput()
    {
        direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }

    void OnMove()
    {
      //  direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        direction.Normalize();
        rig.MovePosition(rig.position + direction * speed * Time.fixedDeltaTime);
    }

     void OnRun()
    {
       
        if (Input.GetKeyDown(KeyCode.LeftShift) && direction.sqrMagnitude > 0f && controle.EstaminaAtual > 0)
        {
            direction.Normalize();
            speed = runSpeed;
            isRunning = true;
           
        }
        if (Input.GetKeyUp(KeyCode.LeftShift) || direction.sqrMagnitude == 0f )
        {
            direction.Normalize();
            speed = initialSpeed;
            isRunning = false;
        }
    }

    void OnRoll()
    {
        if (Input.GetKeyDown(KeyCode.Space) && canRoll && !isRolling && direction.magnitude > 0)
        {
            direction.Normalize();
            isRolling = true;
            canRoll = false;
            speed = rollSpeed;
            controle.EstaminaAtual -= 15;
        }
    }

    void EndRoll()
    {
        isRolling = false;
        canRoll = true;
        speed = initialSpeed;
       
    }


    #endregion

    #region 
    #endregion

}
