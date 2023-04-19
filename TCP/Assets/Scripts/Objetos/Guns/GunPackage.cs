using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunPackage : MonoBehaviour                         /// arrumar o GUN FIND OBJT  POIS ELE TA PEGANDO SO O GUN DO RIFLE NESSA CACETA 
{
    [SerializeField] private Gun gun;
    [SerializeField] private TipoArma tipoArma;
    [SerializeField] private int municaoNoPacote;   
    [SerializeField] private Sprite pistolaSprite;
    [SerializeField] private Sprite rifleSprite;
    [SerializeField] private Sprite shotgunSprite;
    [SerializeField] private int municaoRestante;
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
        tipoArma = (TipoArma)Random.Range(0, 3);
        switch (tipoArma)
        {
            case TipoArma.Pistola:
                municaoNoPacote = Random.Range(6, 20);
                municaoRestante = municaoNoPacote;
                GetComponent<SpriteRenderer>().sprite = sprites[TipoArma.Pistola];
                break;

            case TipoArma.Rifle:
                municaoNoPacote = Random.Range(1, 30);
                municaoRestante = municaoNoPacote;
                GetComponent<SpriteRenderer>().sprite = sprites[TipoArma.Rifle];
                break;

            case TipoArma.Shotgun:
                municaoNoPacote = Random.Range(2, 12);
                municaoRestante = municaoNoPacote;
                GetComponent<SpriteRenderer>().sprite = sprites[TipoArma.Shotgun];
                break;
        }
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && Vector2.Distance(transform.position, gun.transform.position) < 2f && gun.balasGuardadas < gun.maxBalasGuardadas)
        {
            if (tipoArma == gun.tipoArma)
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
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 2f);
    }
}


