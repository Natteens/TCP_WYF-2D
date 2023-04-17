using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BalasGun : MonoBehaviour
{
    public Text balasText;
    public PlayerAnim playerAnim;
    public Gun gun;

    void Update()
    {


        // Verificar se o jogador est� equipado com uma arma
        if (playerAnim.equipado)
        {
            GameObject playerObj = GameObject.Find("Player");
            Gun gun = playerObj.GetComponentInChildren<Gun>();


            // Atualizar a vari�vel Text com a contagem de balas atual da arma
            if (gun.tipoArma == TipoArma.Shotgun)
            {
                balasText.text = gun.balasNoPente.ToString() + "/" + gun.balasGuardadas.ToString();
            }
            else if (gun.tipoArma == TipoArma.Rifle)
            {
                balasText.text = gun.balasNoPente.ToString() + "/" + gun.balasGuardadas.ToString();
                // exibir aqui as balas da AK47
            }
        }
        else
        {
            // Se o jogador n�o estiver equipado, exibir um texto vazio
            balasText.text = "";
        }
    }

}
