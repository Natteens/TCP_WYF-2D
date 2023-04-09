using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour

{

    [SerializeField] public float speed;
    [SerializeField] private float runSpeed; 
    [SerializeField] public float rollSpeed;
    [SerializeField] public float initialSpeed;
    [SerializeField] public bool canRoll = true;


    private Rigidbody2D rig;

    
    private bool _isRunning;
    private bool _isRolling;
    

    private Vector2 _direction;

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


    private void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        initialSpeed = speed;
       
        

    }

    private void Update()
    {
        OnInput();
        OnRun();
        OnRoll();

    }

    private void FixedUpdate()
    {
        OnMove();
        
    }

    #region Movement

    void OnInput()
    {
        _direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }

    void OnMove()
    {
        _direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        _direction.Normalize();
        rig.MovePosition(rig.position + _direction * speed * Time.fixedDeltaTime);
    }

    void OnRun()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && _direction.sqrMagnitude > 0)
        {
            speed = runSpeed;
            _isRunning = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift) || _direction.sqrMagnitude == 0)
        {
            speed = initialSpeed;
            _isRunning = false;
        }
    }


    void OnRoll()
    {
        if (Input.GetKeyDown(KeyCode.Space) && canRoll && !isRolling && _direction.magnitude > 0f)
        {
            _isRolling = true;
            canRoll = false;
            speed = rollSpeed;
            
        }
    }

    void EndRoll()
    {
        speed = initialSpeed;
    }


    #endregion
}
