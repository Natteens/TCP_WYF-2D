using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Controle : MonoBehaviour
{
    
    private Player player;
    public Image BarraVida, BarraEstamina, BarraFome, BarraSede;
    [Range(20, 500)]
    public float VidaCheia = 100, EstaminaCheia = 100, FomeCheia = 100, SedeCheia = 100, velocidadeEstamina = 250;
    [HideInInspector]
    public float VidaAtual, EstaminaAtual, FomeAtual, SedeAtual;
    private bool semEstamina = false;
    private float cronometroFome, cronometroSede, velocidadeCaminhando, velocidadeCorrendo;

    public float danoSmoke = 2f;
    public float danoSmokeAumentado = 5f;
    public float tempoMaximoSmoke = 120f; // 2 minutos em segundos
    private float tempoDecorridoSmoke = 0f;
    private bool estaNaSmoke = false;
   // private bool estaComMascara = false;


    void Start()
    {
        player = GetComponent<Player>();

        VidaAtual = VidaCheia;
        EstaminaAtual = EstaminaCheia;
        FomeAtual = FomeCheia;
        SedeAtual = SedeCheia;
       
        velocidadeCaminhando = player.speed;
        velocidadeCorrendo = player.runSpeed;
    }
    void Update()
    {      
        SistemaDeVida();
        SistemaDeEstamina();
        SistemaDeFome();
        SistemaDeSede();
        AplicarBarras();   
    }
    void FixedUpdate()
    {
        tomandoDanoSmoke();
    }
    void SistemaDeFome()
    {
        FomeAtual -= Time.deltaTime;
        if (FomeAtual >= FomeCheia)
        {
            FomeAtual = FomeCheia;
        }
        if (FomeAtual <= 0)
        {
            FomeAtual = 0;
            cronometroFome += Time.deltaTime;
            if (cronometroFome >= 3)
            {
                VidaAtual -= (VidaCheia * 0.005f);
                EstaminaAtual -= (EstaminaCheia * 0.1f);
                cronometroFome = 0;
            }
        }
        else
        {
            cronometroFome = 0;
        }
    }
    void SistemaDeSede()
    {
        SedeAtual -= Time.deltaTime;
        if (SedeAtual >= SedeCheia)
        {
            SedeAtual = SedeCheia;
        }
        if (SedeAtual <= 0)
        {
            SedeAtual = 0;
            cronometroSede += Time.deltaTime;
            if (cronometroSede >= 3)
            {
                EstaminaAtual -= (EstaminaCheia * 0.1f);
                cronometroSede = 0;
            }
        }
        else
        {
            cronometroSede = 0;
        }
    }
    void SistemaDeEstamina()
    {
        float multEuler = ((1 / EstaminaCheia) * EstaminaAtual) * ((1 / FomeCheia) * FomeAtual);
        if (EstaminaAtual >= EstaminaCheia)
        {
            EstaminaAtual = EstaminaCheia;
        }
        else
        {
            EstaminaAtual += Time.deltaTime * (velocidadeEstamina / 40) * Mathf.Pow(2.718f, multEuler);
        }
        if (EstaminaAtual <= 0)
        {
            EstaminaAtual = 0;
            player.speed = velocidadeCaminhando;
            semEstamina = true;
            player.isRunning = false;
            
        }
        if (semEstamina == true && EstaminaAtual >= (EstaminaCheia * 0.15f))
        {
            player.runSpeed = velocidadeCorrendo;
            semEstamina = false;
        }
        if (player.direction.sqrMagnitude > 0f && semEstamina == false && player.isRunning)
        {

            EstaminaAtual -= Time.deltaTime * (velocidadeEstamina / 15) * Mathf.Pow(2.718f, multEuler);
        }
        
    }
    void SistemaDeVida()
    {
        if (VidaAtual >= VidaCheia)
        {
            VidaAtual = VidaCheia;
        }
        else if (VidaAtual <= 0)
        {
            VidaAtual = 0;
            Morreu();
        }
    }
    void AplicarBarras()
    {
        BarraVida.fillAmount = ((1 / VidaCheia) * VidaAtual);
        BarraEstamina.fillAmount = ((1 / EstaminaCheia) * EstaminaAtual);
        BarraFome.fillAmount = ((1 / FomeCheia) * FomeAtual);
        BarraSede.fillAmount = ((1 / SedeCheia) * SedeAtual);
    }
    void Morreu()
    {
        Debug.Log("Morreu por falta de comida");
    }

    #region DANO DA SMOKE 
    void tomandoDanoSmoke()
    {
        if (estaNaSmoke)
        {
            tempoDecorridoSmoke += Time.fixedDeltaTime;
            if (tempoDecorridoSmoke <= tempoMaximoSmoke)
            {
                float danoAtual = tempoDecorridoSmoke <= 60f ? danoSmoke : danoSmokeAumentado;              
                tomarDano(danoAtual);
                Debug.Log("Dano atual: " + danoAtual);
            }
            else
            {
                estaNaSmoke = false;
                tempoDecorridoSmoke = 0f;
            }
        }
        else
        {
            tempoDecorridoSmoke = 0f;
        }
    }
    void tomarDano(float dano)
    {
        VidaAtual -= dano; 
       
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Smoke"))
        {
            estaNaSmoke = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Smoke"))
        {
            estaNaSmoke = false;
            tempoDecorridoSmoke = 0f;
        }
    }
    #endregion

}