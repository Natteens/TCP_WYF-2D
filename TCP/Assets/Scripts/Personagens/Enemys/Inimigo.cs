using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Inimigo : MonoBehaviour
{
    public float velocidade;
    public float distanciaDeVisao;
    public float distanciaDeAtaque;
    public Transform posicaoInicial;
    [Range(1, 30)] public int dano;

    private Transform jogador;
    private Animator anim;
    private AIPath aiPath;
    private bool atacando = false;
    private bool voltandoParaPosicaoInicial = false;

    private enum EstadoInimigo
    {
        Idle,
        Seguindo,
        Atacando,
        Voltando
    }

    private EstadoInimigo estadoAtual;

    private void Start()
    {
        jogador = GameObject.FindGameObjectWithTag("Player").transform;
        anim = GetComponent<Animator>();
        aiPath = GetComponent<AIPath>();

        estadoAtual = EstadoInimigo.Idle;
    }

    private void Update()
    {
        float distancia = Vector2.Distance(transform.position, jogador.position);

        switch (estadoAtual)
        {
            case EstadoInimigo.Idle:
                if (distancia < distanciaDeVisao)
                {
                    estadoAtual = EstadoInimigo.Seguindo;
                }
                break;

            case EstadoInimigo.Seguindo:
                if (distancia > distanciaDeAtaque)
                {
                    SeguirJogador();
                }
                else
                {
                    estadoAtual = EstadoInimigo.Atacando;
                }
                break;

            case EstadoInimigo.Atacando:
                if (distancia > distanciaDeAtaque)
                {
                    estadoAtual = EstadoInimigo.Seguindo;
                }
                else if (!atacando)
                {
                    StartCoroutine(ExecutarAtaque());
                }
                break;

            case EstadoInimigo.Voltando:
                VoltarPosicaoInicial();
                break;
        }
    }

    private void SeguirJogador()
    {
        anim.SetInteger("transition", 2); // Walking
        aiPath.enabled = true;
    }

    private void VoltarPosicaoInicial()
    {
        anim.SetInteger("transition", 1); // Idle
        aiPath.enabled = true;

        if (Vector2.Distance(transform.position, posicaoInicial.position) <= 0.1f)
        {
            voltandoParaPosicaoInicial = false;
            aiPath.enabled = false;
            estadoAtual = EstadoInimigo.Idle;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            estadoAtual = EstadoInimigo.Seguindo;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            estadoAtual = EstadoInimigo.Voltando;
        }
    }

    private void CausarDano()
    {
        float distancia = Vector2.Distance(transform.position, jogador.position);
        if (distancia <= distanciaDeAtaque)
        {
            jogador.GetComponent<Controle>().VidaAtual -= dano;
        }
    }

    private IEnumerator ExecutarAtaque()
    {
        atacando = true;
        anim.SetInteger("transition", 1); // Idle

        yield return new WaitForSeconds(1f); // Tempo de espera antes do ataque, ajuste conforme necessário

        anim.SetBool("Atacando", true);
        yield return new WaitForSeconds(0.5f); // Duração do ataque, ajuste conforme necessário
        CausarDano(); // Chama o método para causar dano

        yield return new WaitForSeconds(1f); // Tempo de espera após o ataque, ajuste conforme necessário

        anim.SetBool("Atacando", false);
        atacando = false;
        estadoAtual = EstadoInimigo.Seguindo;
    }
}


