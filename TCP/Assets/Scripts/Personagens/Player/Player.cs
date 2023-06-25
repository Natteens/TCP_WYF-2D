using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour

{
    [Space(10)]
    [Header("-------------Configs---------")]
    [SerializeField] private PlayerAnim playerAnim;
    [SerializeField] private Gun gun;
    [SerializeField] private Controle controle;
    [SerializeField] private Menu menu;
    [SerializeField] private Bilhetes bilhetes;


    [Space(10)]
    [Header("-------------Movimentação---------")]
    [SerializeField] public float speed;
    [SerializeField] public float runSpeed; 
    [SerializeField] public float rollSpeed;
    [SerializeField] public float initialSpeed;
    [SerializeField] public bool canRoll = true;
    public bool canMove = true;
    [Range(1, 5)]public int energiaGasta;
    private Rigidbody2D rig;  
    private bool _isRunning;
    private bool _isRolling;
    private Vector2 _direction;
    public bool estaMorto = false; 

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
    private void Awake()
    {
        GameSaveManager.instance.player = this;
    }
    private void Start()
    {
        GameSaveManager.instance.LoadGame();

        Time.timeScale = 1f;
        rig = GetComponent<Rigidbody2D>();
        initialSpeed = speed;
        
    }
    void Update()
    {
        if (!estaMorto || !bilhetes.LendoBilhete)
        {

            if (!menu.isPaused && !menu.optionsPanel.activeSelf) // Verifica se o jogo não está pausado e o menu de opções não está ativo
            {

                OnInput();
                OnRun();

                if (controle.EstaminaAtual >= 15)
                {
                    OnRoll();
                }


            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (!menu.optionsPanel.activeSelf) // Verifica se o menu de op��es n�o est� ativo
                {
                    menu.PauseScreen();
                }
                else
                {
                    menu.BackToMenu(); // Fecha o menu de op��es
                }
            } 
        }
    }
    void FixedUpdate()
    {
        if (!estaMorto  || !bilhetes.LendoBilhete)
        {
            if (!menu.isPaused && !menu.optionsPanel.activeSelf) // Verifica se o jogo não está pausado e o menu de opções não está ativo
            {
                OnMove();
            } 
        }
    }

    #region Movement

    void OnInput()
    {
        if (canMove)
        {
            direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        }
        else
        {
            direction = Vector2.zero;
        }
        
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
        StartCoroutine(OnRollCoroutine());
    }

    IEnumerator OnRollCoroutine()
    {
        if (Input.GetKeyDown(KeyCode.Space) && canMove && canRoll && !isRolling && direction.magnitude > 0)
        {
            direction.Normalize();
            isRolling = true;
            canRoll = false;
            speed = rollSpeed;
            controle.EstaminaAtual -= 15;

            yield return new WaitForSeconds(playerAnim.anim.GetCurrentAnimatorStateInfo(0).length);

            EndRoll(); 
        }
    }

    void EndRoll()
    {
        isRolling = false;
        canRoll = true;
        speed = initialSpeed;
    }

    #endregion

    #region controle
    public void TomouDano()
    {
        if (isRolling)
        {
            // O jogador est� rolando, ignorar a anima��o de dano
            return;
        }
        // para o movimento do player por 0.5f e dps ele pode voltar a se mover 
        canMove = false;
        Invoke("PermitirMovimento", 0.4f);
        playerAnim.TomandoDano();
        
    }

    private void PermitirMovimento()
    {
        canMove = true;
        canRoll = true;
        playerAnim.atacando = false;
        
    }

    #endregion

}
