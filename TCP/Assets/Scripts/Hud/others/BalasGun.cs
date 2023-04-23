using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BalasGun : MonoBehaviour
{
    public Text balasText;
    public PlayerAnim playerAnim;
    public Gun gun;

    [SerializeField] private GameObject pistolaCanvas;
    [SerializeField] private GameObject rifleCanvas;
    [SerializeField] private GameObject shotgunCanvas;


    private void Update()
    {
        // Verificar se o jogador está equipado com uma arma
        if (playerAnim.equipado)
        {
            GameObject playerObj = GameObject.Find("Player");
            Gun gun = playerObj.GetComponentInChildren<Gun>();

            // Atualizar a variável Text com a contagem de balas atual da arma
            if (gun.tipoArma == TipoArma.Shotgun)
            {
                balasText.text = gun.shotgunBalasNoPente.ToString() + "/" + gun.shotgunBalasGuardadas.ToString();
                shotgunCanvas.SetActive(true);
                rifleCanvas.SetActive(false);
                pistolaCanvas.SetActive(false);
            }
            else if (gun.tipoArma == TipoArma.Rifle)
            {
                balasText.text = gun.rifleBalasNoPente.ToString() + "/" + gun.rifleBalasGuardadas.ToString();
                rifleCanvas.SetActive(true);
                shotgunCanvas.SetActive(false);
                pistolaCanvas.SetActive(false);
            }
            else if (gun.tipoArma == TipoArma.Pistola)
            {
                balasText.text = gun.pistolaBalasNoPente.ToString() + "/" + gun.pistolaBalasGuardadas.ToString();
                pistolaCanvas.SetActive(true);
                rifleCanvas.SetActive(false);
                shotgunCanvas.SetActive(false);
            }
        }
        else
        {
            // Se o jogador não estiver equipado, exibir um texto vazio
            balasText.text = "";
            shotgunCanvas.SetActive(false);
            pistolaCanvas.SetActive(false);
            rifleCanvas.SetActive(false);
        }

    }
}