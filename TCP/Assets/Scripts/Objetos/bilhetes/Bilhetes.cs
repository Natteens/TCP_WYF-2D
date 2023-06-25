using UnityEngine;

public enum BilheteEnum
{
    Bilhete1,
    Bilhete2,
    Bilhete3,
    Bilhete4,
    Bilhete5,
    Bilhete6,
    Bilhete7,
    Bilhete8,
    Bilhete9,
    Bilhete10
}

public class Bilhetes : MonoBehaviour
{
    public GameObject objCanvas;
    public GameObject[] bilhetes;
    public float distanciaMaxima = 2f;
    public LayerMask layerJogador;
    public bool LendoBilhete = false;

    [SerializeField] private bool[] bilhetesDesbloqueados;
    private int bilheteAtual = 0;

    private void Start()
    {
        bilhetesDesbloqueados = new bool[bilhetes.Length];
        DesbloquearBilhete(BilheteEnum.Bilhete1);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, distanciaMaxima, layerJogador);

            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].CompareTag("Player"))
                {
                    AbrirBilhete(bilheteAtual);
                    return;
                }
            }

            FecharBilhetes();
        }
    }

    public void AbrirBilhete(int indiceBilhete)
    {
        bilheteAtual = indiceBilhete;
        objCanvas.SetActive(true);
        AtualizarBilhetes();
        LendoBilhete = true;
    }

    public void FecharBilhetes()
    {
        objCanvas.SetActive(false);
        LendoBilhete = false;
    }

    public void DesbloquearBilhete(BilheteEnum bilhete)
    {
        bilhetesDesbloqueados[(int)bilhete] = true;
        // Aqui você pode fazer qualquer ação necessária ao desbloquear um bilhete, como mostrar um ícone de bilhete desbloqueado, etc.
    }

    public bool VerificarBilheteDesbloqueado(BilheteEnum bilhete)
    {
        return bilhetesDesbloqueados[(int)bilhete];
    }

    public void AvancarBilhete()
    {
        if (bilhetesDesbloqueados.Length == 0)
            return;

        int proximoBilhete = (bilheteAtual + 1) % bilhetesDesbloqueados.Length;

        while (!bilhetesDesbloqueados[proximoBilhete])
        {
            proximoBilhete = (proximoBilhete + 1) % bilhetesDesbloqueados.Length;

            // Verifica se todos os bilhetes estão bloqueados
            if (proximoBilhete == bilheteAtual)
                return;
        }

        bilheteAtual = proximoBilhete;
        AbrirBilhete(bilheteAtual);
    }

    public void VoltarBilhete()
    {
        if (bilhetesDesbloqueados.Length == 0)
            return;

        int bilheteAnterior = (bilheteAtual - 1 + bilhetesDesbloqueados.Length) % bilhetesDesbloqueados.Length;

        while (!bilhetesDesbloqueados[bilheteAnterior])
        {
            bilheteAnterior = (bilheteAnterior - 1 + bilhetesDesbloqueados.Length) % bilhetesDesbloqueados.Length;

            // Verifica se todos os bilhetes estão bloqueados
            if (bilheteAnterior == bilheteAtual)
                return;
        }

        bilheteAtual = bilheteAnterior;
        AbrirBilhete(bilheteAtual);
    }

    private void AtualizarBilhetes()
    {
        for (int i = 0; i < bilhetes.Length; i++)
        {
            if (i == bilheteAtual)
                bilhetes[i].SetActive(true);
            else
                bilhetes[i].SetActive(false);
        }
    }
}
