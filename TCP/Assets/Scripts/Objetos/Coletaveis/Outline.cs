using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Outline : MonoBehaviour
{
    private Transform jogador;
    [SerializeField] private float raioBrilho;
    [SerializeField] private Material outlineMaterial;
    [SerializeField] private Material noOutlineMaterial;

    private SpriteRenderer spriteRenderer;
    private bool brilhando = false;

    private void Awake()
    {
        jogador = GameObject.FindWithTag("Player").transform;
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.material = noOutlineMaterial;
    }

    private void Update()
    {
        float distancia = Vector2.Distance(transform.position, jogador.position);

        if (distancia <= raioBrilho && !brilhando)
        {
            spriteRenderer.material = outlineMaterial;
            brilhando = true;
        }
        else if (distancia > raioBrilho && brilhando)
        {
            spriteRenderer.material = noOutlineMaterial;
            brilhando = false;
        }
    }
}

