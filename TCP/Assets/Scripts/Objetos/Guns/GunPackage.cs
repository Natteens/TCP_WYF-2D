using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunPackage : MonoBehaviour
{
    [SerializeField] private int municaoNoPacote;
    [SerializeField] private Gun gun;
    [SerializeField] private PlayerAnim playerAnim;
  //  [SerializeField] private int maxBalasGuardadas; // quantidade máxima de balas que podem ser guardadas na arma
    [SerializeField] private int municaoRestante;

   


    private void Awake()
    {
        playerAnim = FindObjectOfType<PlayerAnim>();
        gun = FindObjectOfType<Gun>();
    }

    private void Start()
    {
        municaoNoPacote = Random.Range(3, 12);
        municaoRestante = municaoNoPacote;

        if (gun.tipoArma == TipoArma.Pistola)
        {
            gun.maxBalasGuardadas = 60;
        }
        else if (gun.tipoArma == TipoArma.Rifle)
        {
            gun.maxBalasGuardadas = 120;
        }
        else if (gun.tipoArma == TipoArma.Shotgun)
        {
            gun.maxBalasGuardadas = 24;
        }
    }


    private void Update()
    {
        // Se pressionar a tecla "E" e estiver próximo do pacote de munição e a arma não estiver com o pente cheio
        if (Input.GetKeyDown(KeyCode.E) && Vector2.Distance(transform.position, gun.transform.position) < 2f && gun.balasGuardadas < gun.maxBalasGuardadas)
        {
            var balasQueCabemNoPente = gun.maxBalasGuardadas - gun.balasGuardadas;
            var balasQueRestamNoPacote = municaoRestante;
            var balasQueSeraoPegas = Mathf.Min(balasQueRestamNoPacote, balasQueCabemNoPente);

            if (balasQueSeraoPegas > 0)
            {
                gun.balasGuardadas += balasQueSeraoPegas;
                municaoRestante -= balasQueSeraoPegas;
            }

            if (municaoRestante == 0)
            {
                Destroy(gameObject);
            }
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 1f);
    }


}
