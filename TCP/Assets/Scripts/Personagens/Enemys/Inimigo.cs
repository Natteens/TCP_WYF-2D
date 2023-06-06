using System.Collections;
using UnityEngine;
using Pathfinding;

public class Inimigo : MonoBehaviour
{
    [Space(10)]
    [Header("Configurações de Visão")]
    [Range(1, 100)]
    public float distanciaDeVisao;
    [Range(1, 100)]
    public float distanciaMaxima;
    private float distanciaDeVisaoOriginal;

    [Space(20)]
    [Header("Configurações de Ataque")]
    [Range(1, 100)]
    public float distanciaDeAtaque;
    [Range(1f, 100f)]
    public int dano;
    public LayerMask layersDano;
    [Range(0f, 10f)]
    public float intervaloDeAtaque;
    public Player player;

    [Space(20)]
    [Header("Configurações de Vida")]
    [Range(10, 500)]
    public int vidaMaxima;
    public int vidaAtual;

    [Space(20)]
    [Header("Configurações do Knockback")]
    [Range(0, 50)]
    [SerializeField] private float knockbackDistance;

    [Space(20)]
    [Header("Configurações de Raycast")]
    public LayerMask layerParede;

    private Transform jogador;
    private Animator anim;
    private AIPath aiPath;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;


    private bool morto = false;
    private bool spawnado = false;
    private bool atacando = false;

    private void Start()
    {
      // player = GetComponent<Player>();
        jogador = GameObject.FindGameObjectWithTag("Player").transform;
        anim = GetComponent<Animator>();
        aiPath = GetComponent<AIPath>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();

        vidaAtual = vidaMaxima;
        distanciaDeVisaoOriginal = distanciaDeVisao;
        anim.SetBool("HiddenSpawn", true);

        StartCoroutine(VerificarVisao());
    }

    private IEnumerator VerificarVisao()
    {
        while (true)
        {
            if (!morto)
            {
                float distancia = Vector2.Distance(transform.position, jogador.position);

                if (distanciaDeVisao > distanciaDeVisaoOriginal)
                {
                    distanciaDeVisao -= Time.deltaTime * 10f;
                    if (distanciaDeVisao < distanciaDeVisaoOriginal)
                        distanciaDeVisao = distanciaDeVisaoOriginal;
                }
                else if (distanciaDeVisao < distanciaMaxima)
                {
                    distanciaDeVisao += Time.deltaTime * 10f;
                    if (distanciaDeVisao > distanciaMaxima)
                        distanciaDeVisao = distanciaMaxima;
                }

                if (distancia <= distanciaDeVisao)
                {
                    if (!spawnado) // Não spawnou
                    {
                        anim.SetBool("HiddenSpawn", false);
                        yield return new WaitForSeconds(1.5f);
                        if (distancia <= distanciaDeVisao)
                        {
                            if (!VerificarColisaoParede())
                            {
                                SeguirJogador();
                            }
                        }

                        Spawn();
                        spawnado = true;
                        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
                        SeguirJogador();
                    }
                    else        // já Spawnou
                    {
                        if (!VerificarColisaoParede())
                        {
                            SeguirJogador();
                        }
                    }
                }
                else
                {
                    PararMovimento();
                }
                AtacarJogador(distancia);
            }

            yield return null;
        }
    }

    private bool VerificarColisaoParede()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, jogador.position - transform.position, distanciaDeVisao, layerParede);
        if (hit.collider != null && hit.collider.CompareTag("Wall"))
        {
            PararMovimento();
           // Debug.Log("Colisão com parede: " + hit.collider.gameObject.name);
            return true;
        }
        return false;
    }


    private void Update()
    {
       

        if (!morto && !aiPath.isStopped)
        {
            bool movendoAnteriormente = anim.GetBool("Movendo");
            bool movendoAtualmente = (aiPath.velocity.sqrMagnitude > 0f);

            if (movendoAtualmente != movendoAnteriormente)
            {
                anim.SetBool("Movendo", movendoAtualmente);
                anim.SetBool("Parado", !movendoAtualmente);
            }
        }

        if (vidaAtual <= 0 && !morto)
        {
            vidaAtual = 0;
            Morrer();
        }
    }

    private void SeguirJogador()
    {
        OlharParaJogador();
        aiPath.enabled = true;
        aiPath.isStopped = false;
        
    }

    private void PararMovimento()
    {
        if (anim.GetBool("HiddenSpawn"))
        {
            aiPath.enabled = false;
            aiPath.isStopped = true;
        }
        else
        {
            anim.SetBool("Parado", true);
            aiPath.enabled = false;
           
        }
    }

    private void OlharParaJogador()
    {
        Vector2 direcao = jogador.position - transform.position;
        spriteRenderer.flipX = (direcao.x < 0);
    }

    private void AtacarJogador(float distancia)
    {
        if (distancia <= distanciaDeAtaque && !atacando)
        {
            PararMovimento();
            aiPath.isStopped = true;
            StartCoroutine(ExecutarAtaque());
        }
        else
        {
            aiPath.isStopped = false;
            anim.ResetTrigger("Atacando");
        }
    }

    private IEnumerator ExecutarAtaque()
    {
        atacando = true;
        anim.SetTrigger("Atacando");
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length + intervaloDeAtaque);
        atacando = false;
    }

    public void CausarDanoAoPlayer()
    {
        float distancia = Vector2.Distance(transform.position, jogador.position);
        if (distancia <= distanciaDeAtaque)
        {
           jogador.GetComponent<Controle>().VidaAtual -= dano;
           player.TomouDano();
        }
    }

    public void TomarDano()
    {
        anim.SetTrigger("TomandoDano");
        PararMovimento();
        atacando = false;
       
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "bullet")
        {
            Vector2 difference = (transform.position - other.transform.position).normalized;
            Vector2 knockback = new Vector2(difference.x * knockbackDistance, difference.y * knockbackDistance);
            transform.position = new Vector2(transform.position.x + knockback.x, transform.position.y + knockback.y);
        }
    }

    private void Morrer()
    {
        morto = true;
        anim.SetTrigger("Morreu");
        aiPath.enabled = false;
        GetComponent<Collider2D>().enabled = false;
        rb.bodyType = RigidbodyType2D.Static;
        Destroy(gameObject, 2f);
    }

    public void Spawn()
    {
        vidaAtual = vidaMaxima;
        morto = false;
        anim.SetTrigger("Spawn");
        aiPath.enabled = false;
        GetComponent<Collider2D>().enabled = true;
    }

}