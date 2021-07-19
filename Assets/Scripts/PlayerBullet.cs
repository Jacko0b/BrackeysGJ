using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour

{
    public GameObject bulletEffect;

    public float bulletSpeed = 2f;
    public Rigidbody2D rb;
    public PlayerBehaviour controlls;
    void Start()
    {
        controlls= (PlayerBehaviour)FindObjectOfType(typeof(PlayerBehaviour));
       
        rb.AddForce(controlls.firePoint.up * bulletSpeed, ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer != controlls.gameObject.layer)
        {
            GameObject efect = Instantiate(bulletEffect, transform.position, Quaternion.identity);
            Destroy(efect, 5f);
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject,1f);
        }
    }

}
