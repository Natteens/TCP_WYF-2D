using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
                            Destroy(gameObject);
                            break;
                        case TIPO.Bebida:
                            Player.GetComponent<Controle>().SedeAtual += QuantoRepor;
                            Destroy(gameObject);
                            break;
                        case TIPO.Vida:
                            Player.GetComponent<Controle>().VidaAtual += QuantoRepor;
                            Destroy(gameObject);
                            break;
                        case TIPO.Energia:
                            Player.GetComponent<Controle>().EstaminaAtual += QuantoRepor;
                            Destroy(gameObject);
                            break;
                    }
                }
            }
            
        }
           
    }
}