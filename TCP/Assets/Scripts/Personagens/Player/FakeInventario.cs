using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeInventario : MonoBehaviour
{
    public List<GameObject> itensColetaveis = new List<GameObject>();

    private void Start()
    {
        // Adiciona todos os objetos com a tag "ItensColetaveis" � lista de itens colet�veis
        GameObject[] objetosComTag = GameObject.FindGameObjectsWithTag("Weapons");
        foreach (GameObject objeto in objetosComTag)
        {
            itensColetaveis.Add(objeto);
        }
    }
}
