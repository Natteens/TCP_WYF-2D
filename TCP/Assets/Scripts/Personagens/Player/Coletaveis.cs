using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Coletaveis : MonoBehaviour
{
    public enum TIPO
    {
        Comida,
        Bebida,
        Vida,
        Energia
    }

    
    public TIPO TipoDoItem;
    [Range(1, 500)]
    public float QuantoRepor = 50;
    [SerializeField] private GameObject Player;
    public LayerMask layerJogador;
    [SerializeField] float distanciaMaxima = 2f;

    [Space(20)]
    [Header("Sounds")]

    [SerializeField] private AudioSource audioSource;
    public AudioClip somComida;
    public AudioClip somBebida;
    public AudioClip somVida;
    public AudioClip somEnergia;


    private void Start()
    {
        Player = GameObject.FindWithTag("Player");
       
    }
    void Update()
    {
        PegarItem();
    }

    void PegarItem()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, distanciaMaxima, layerJogador);


        for (int i = 0; i < colliders.Length; i++)
        {

            if (colliders[i].tag == "Player")
            {
                if (Input.GetKeyDown(KeyCode.E))
                {

                    switch (TipoDoItem)
                    {
                        case TIPO.Comida:
                            Player.GetComponent<Controle>().FomeAtual += QuantoRepor;
                            audioSource.PlayOneShot(somComida);
                            Destroy(gameObject);
                            break;
                        case TIPO.Bebida:
                            Player.GetComponent<Controle>().SedeAtual += QuantoRepor;
                            audioSource.PlayOneShot(somBebida);
                            Destroy(gameObject);
                            break;
                        case TIPO.Vida:
                            Player.GetComponent<Controle>().VidaAtual += QuantoRepor;
                            audioSource.PlayOneShot(somVida);
                            Destroy(gameObject);
                            break;
                        case TIPO.Energia:
                            Player.GetComponent<Controle>().EstaminaAtual += QuantoRepor;
                            audioSource.PlayOneShot(somEnergia);
                            Destroy(gameObject);
                            break;
                    }

                }
            }
            
        }
           
    }

   
}