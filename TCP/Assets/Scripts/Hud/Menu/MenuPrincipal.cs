using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipal : MonoBehaviour
{

    // Start is called before the first frame update
    public void Sair()
    {
      Debug.Log("Saiu");
      Application.Quit();  
    }

    // Update is called once per frame
    public void Jogar()
    {
        SceneManager.LoadScene("Laboratorio");
    }
}
