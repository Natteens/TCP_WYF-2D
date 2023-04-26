using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;

public class Inimigo : MonoBehaviour
{
    private Controle controle;
    private Rigidbody2D rig;
    private Animator anim;
    private bool estaAtacando;
    private bool estaProcurandoJogador;
    private bool estaIndoParaOrigem;
    private Vector2 posicaoOrigem;
    public float velocidadeMovimento;
    public int vidaMaxima;
    public int dano;
    private int vidaAtual;
    private Collider2D colisor;


    void Start()
    {
        controle = FindObjectOfType(typeof(Controle)) as Controle;
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        posicaoOrigem = transform.position;
        vidaAtual = vidaMaxima;
    }

    void Update()
    {
        if (estaAtacando)
        {
            return;
        }

        if (estaProcurandoJogador)
        {
            float distancia = Vector2.Distance(transform.position, controle.transform.position);

            if (distancia <= 1f)
            {
                AtacarJogador();
            }
            else if (distancia > 10f)
            {
                estaProcurandoJogador = false;
                estaIndoParaOrigem = true;
            }
            else
            {
                MoverJuntoJogador();
            }
        }
        else if (estaIndoParaOrigem)
        {
            float distancia = Vector2.Distance(transform.position, posicaoOrigem);

            if (distancia <= 0.1f)
            {
                estaIndoParaOrigem = false;
                anim.SetBool("Andando", false);
                anim.SetBool("Parado", true);
            }
            else
            {
                MoverParaOrigem();
            }
        }
        else
        {
            anim.SetBool("Andando", false);
            anim.SetBool("Parado", true);
        }
    }

    void AtacarJogador()
    {
        estaAtacando = true;
        anim.SetBool("Atacando", true);
    }

    void AplicarDanoAoJogador()
    {
        controle.VidaAtual -= dano;
    }

    void TerminouAtaque()
    {
        estaAtacando = false;
        anim.SetBool("Atacando", false);
        Invoke("EsperarProximoAtaque", 2f);
    }

    void EsperarProximoAtaque()
    {
        if (estaProcurandoJogador)
        {
            estaAtacando = true;
            anim.SetBool("Atacando", true);
        }
    }

    void MoverJuntoJogador()
    {
        anim.SetBool("Andando", true);
        anim.SetBool("Parado", false);

        Vector2 direcao = controle.transform.position - transform.position;
        rig.velocity = direcao.normalized * velocidadeMovimento;
        transform.right = direcao;
    }

    void MoverParaOrigem()
    {
        anim.SetBool("Andando", true);
        anim.SetBool("Parado", false);

        Vector2 direcao = posicaoOrigem - (Vector2)transform.position;
        rig.velocity = direcao.normalized * velocidadeMovimento;
        transform.right = direcao;
    }

    public void TomarDano(int quantidade)
    {
        vidaAtual -= quantidade;
        if (vidaAtual <= 0)
        {
            Morrer();
        }
    }

    private void Morrer()
    {
        // Executa animação de morte
        anim.SetTrigger("Morrer");

        // Desativa o colisor e o Rigidbody2D do inimigo
        colisor.enabled = false;
        rig.simulated = false;

        // Destroi o objeto do inimigo após 2 segundos
        Destroy(gameObject, 2f);
    }
}
