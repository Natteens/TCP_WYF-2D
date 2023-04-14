using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Bullet : MonoBehaviour
{
    public float speed;
    public Animator anim;
    public float destroyedDelay = 0.5f;

    private void Update()
    {
        transform.Translate(Vector2.up * speed * Time.deltaTime);
        anim.SetInteger("transition", 1);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        anim.SetTrigger("OnHit");
        Destroy(gameObject, destroyedDelay);
    }
}

