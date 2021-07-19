using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public GameObject bulletEffect;

    [SerializeField] private float bulletSpeed = 0.1f;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private PlayerControlls controlls;


    void Start()
    {
        rb.AddForce((GameObject.FindObjectOfType<PlayerControlls>().transform.position - transform.position).normalized * bulletSpeed, ForceMode2D.Impulse);
        //Debug.Log(controlls.getPlayerPosition());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer != gameObject.layer)
        {
            GameObject efect = Instantiate(bulletEffect, transform.position, Quaternion.identity);
            Destroy(efect, 5f);
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject, 10f);
        }
    }


}
