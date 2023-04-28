using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inimigo : MonoBehaviour
{
    public float velocidade;
    public float distanciaDeVisao;
    public float distanciaDeAtaque;
    public float tempoDeEspera;
    public Transform posicaoInicial;
    public int dano;

    private Transform jogador;
    private Rigidbody2D rig;
    private Animator anim;
    private bool atacando = false;
    private bool Spawn = false;
    private float tempoDeEsperaInicial;
    private bool voltandoParaPosicaoInicial = false;

    private void Start()
    {
        jogador = GameObject.FindGameObjectWithTag("Player").transform;
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        tempoDeEsperaInicial = tempoDeEspera;
    }

    private void FixedUpdate()
    {
        float distancia = Vector2.Distance(transform.position, jogador.position);

        if (distancia < distanciaDeVisao && !atacando)
        {
            SeguirJogador(distancia);
        }
        else
        {
            VoltarPosicaoInicial();
        }
    }

    private void SeguirJogador(float distancia)
    {
        if (!voltandoParaPosicaoInicial)
        {
            if (distancia > distanciaDeAtaque)
            {
                anim.SetInteger("transition", 2); // Walking
                Vector2 direcao = jogador.position - transform.position;
                rig.velocity = direcao.normalized * velocidade;
                Flip(direcao.x);
            }
            else
            {
                anim.SetInteger("transition", 1); // Idle
                rig.velocity = Vector2.zero;
                atacando = true;
                anim.SetBool("Atacando", true);
            }
        }
        else
        {
            anim.SetInteger("transition", 2); // Walking
            Vector2 direcao = posicaoInicial.position - transform.position;
            rig.velocity = direcao.normalized * velocidade;
            Flip(direcao.x);

            if (Vector2.Distance(transform.position, posicaoInicial.position) < 0.1f)
            {
                voltandoParaPosicaoInicial = false;
            }
        }
    }

    private void VoltarPosicaoInicial()
    {
        anim.SetInteger("transition", 1); // Idle
        rig.velocity = Vector2.zero;

        if (tempoDeEspera <= 0)
        {
            tempoDeEspera = tempoDeEsperaInicial;
            voltandoParaPosicaoInicial = true;
        }
        else
        {
            tempoDeEspera -= Time.deltaTime;
        }
    }

    private void Flip(float direcao)
    {
        if (direcao < 0)
        {
            transform.eulerAngles = new Vector2(0, 180);
        }
        else if (direcao > 0)
        {
            transform.eulerAngles = new Vector2(0, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            AplicarDano(collision.gameObject.GetComponent<Controle>());
        }
    }

    private void AplicarDano(Controle controle)
    {
        controle.VidaAtual -= dano;
    }

    public void FimDeAtaque()
    {
        atacando = false;
        anim.SetBool("Atacando", false);
    }
}
