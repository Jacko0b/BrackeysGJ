using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCheckmark : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))GetComponent<SpriteRenderer>().enabled = true;

    }

    private void OnTriggerStay2D(Collider2D collision)
    {

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) GetComponent<SpriteRenderer>().enabled = false;

    }
}
