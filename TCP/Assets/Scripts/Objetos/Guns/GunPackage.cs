using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunPackage : MonoBehaviour                         
{
    [SerializeField] private Gun gun;
   
    [SerializeField] private TipoArma tipoArma;
    [SerializeField] private int municaoNoPacote;
    [SerializeField] private int municaoRestante;
    [SerializeField] private Sprite pistolaSprite;
    [SerializeField] private Sprite rifleSprite;
    [SerializeField] private Sprite shotgunSprite;   
    private Dictionary<TipoArma, Sprite> sprites;

    private void Awake()
    {
        gun = FindAnyObjectByType<Gun>();

        sprites = new Dictionary<TipoArma, Sprite>()
        {
            { TipoArma.Pistola, pistolaSprite },
            { TipoArma.Rifle, rifleSprite },
            { TipoArma.Shotgun, shotgunSprite }
        };
    }


    private void Start()
    {
        tipoArma = (TipoArma)Random.Range(1, 4);
        switch (tipoArma)
        {
            case TipoArma.Pistola:
                municaoNoPacote = Random.Range(16, 20);
                municaoRestante = municaoNoPacote;
                GetComponent<SpriteRenderer>().sprite = sprites[TipoArma.Pistola];
                break;

            case TipoArma.Rifle:
                municaoNoPacote = Random.Range(15, 30);
                municaoRestante = municaoNoPacote;
                GetComponent<SpriteRenderer>().sprite = sprites[TipoArma.Rifle];
                break;

            case TipoArma.Shotgun:
                municaoNoPacote = Random.Range(5, 12);
                municaoRestante = municaoNoPacote;
                GetComponent<SpriteRenderer>().sprite = sprites[TipoArma.Shotgun];
                break;
        }
    }


     void Update()
    {
        PegarMunicao();
    }

    void PegarMunicao()
{  
    switch (tipoArma)
    {
        case TipoArma.Pistola:
            if (Input.GetKeyDown(KeyCode.E) && Vector2.Distance(transform.position, transform.position) < 1f && gun.pistolaBalasGuardadas < gun.pistolaMaxBalasGuardadas)
            {
                var balasQueCabemNoPente = gun.pistolaMaxBalasGuardadas - gun.pistolaBalasGuardadas;
                var balasQueRestamNoPacote = municaoRestante;
                var balasQueSeraoPegas = Mathf.Min(balasQueRestamNoPacote, balasQueCabemNoPente);

                if (balasQueSeraoPegas > 0)
                {
                    gun.pistolaBalasGuardadas += balasQueSeraoPegas;
                    municaoRestante -= balasQueSeraoPegas;
                }

                if (municaoRestante == 0)
                {
                    Destroy(gameObject);
                }
            }
            break;

        case TipoArma.Rifle:
            if (Input.GetKeyDown(KeyCode.E) && Vector2.Distance(transform.position, gun.transform.position) < 1f && gun.rifleBalasGuardadas < gun.rifleMaxBalasGuardadas)
            {
                var balasQueCabemNoPente = gun.rifleMaxBalasGuardadas - gun.rifleBalasGuardadas;
                var balasQueRestamNoPacote = municaoRestante;
                var balasQueSeraoPegas = Mathf.Min(balasQueRestamNoPacote, balasQueCabemNoPente);

                if (balasQueSeraoPegas > 0)
                {
                    gun.rifleBalasGuardadas += balasQueSeraoPegas;
                    municaoRestante -= balasQueSeraoPegas;
                }

                if (municaoRestante == 0)
                {
                    Destroy(gameObject);
                }
            }
            break;

        case TipoArma.Shotgun:
            if (Input.GetKeyDown(KeyCode.E) && Vector2.Distance(transform.position, gun.transform.position) < 1f && gun.shotgunBalasGuardadas < gun.shotgunMaxBalasGuardadas)
            {
                var balasQueCabemNoPente = gun.shotgunMaxBalasGuardadas - gun.shotgunBalasGuardadas;
                var balasQueRestamNoPacote = municaoRestante;
                var balasQueSeraoPegas = Mathf.Min(balasQueRestamNoPacote, balasQueCabemNoPente);

                if (balasQueSeraoPegas > 0)
                {
                    gun.shotgunBalasGuardadas += balasQueSeraoPegas;
                    municaoRestante -= balasQueSeraoPegas;
                }

                if (municaoRestante == 0)
                {
                    Destroy(gameObject);
                }
            }
            break;
    }
}

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
    }
}


///ARRUMAR A TROCA DE ARMAS !!!!!!!!!!

