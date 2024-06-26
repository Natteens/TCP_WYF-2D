﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pegouitem : MonoBehaviour
{
    [SerializeField] private PlayerAnim playerAnim;
    [SerializeField] private Player player;
    [SerializeField] private TipoArma tipoArma;
    [SerializeField] private Gun gun;
    public LayerMask layerJogador;

    [SerializeField] float distanciaMaxima;

    private void Start()
    {
        gun = FindObjectOfType<Gun>();

        if (gun.pistolaDesbloqueado == true && tipoArma == TipoArma.Pistola)
        {
            Destroy(gameObject);
        }
        if (gun.rifleDesbloqueado == true && tipoArma == TipoArma.Rifle)
        {
            Destroy(gameObject);
        }
        if (gun.shotgunDesbloqueado == true && tipoArma == TipoArma.Shotgun)
        {
            Destroy(gameObject);
        }

    }

    private void Update()
    {
        PegarArma();       
    }
    void PegarArma()
    {   
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, distanciaMaxima, layerJogador);

            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].tag == "Player")
                {
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        switch (tipoArma)
                        {
                            case TipoArma.Rifle:
                               
                                gun.rifleDesbloqueado = true;
                                gun.numArmasDesbloqueadas++;
                                gun.podeAtirar = true;
                               
                             
                            break;
                            case TipoArma.Shotgun:
                                gun.shotgunDesbloqueado = true;
                                gun.numArmasDesbloqueadas++;
                                gun.podeAtirar = true;
                                
                        
                            break;
                            case TipoArma.Pistola:
                                gun.pistolaDesbloqueado = true;
                                gun.numArmasDesbloqueadas++;
                                gun.podeAtirar = true;
                                
                         
                            break;
                        }
                   
                                              
                        Destroy(gameObject);
                    }

                }
            }
        


    }
}
