using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    [SerializeField] private Animator anim;

    public bool plateDown = false;
    private void Awake()
    {
        anim.GetComponent<Animator>();

    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("DeadBody"))
        {
            plateDown= true;
            anim.SetBool("plateOn", true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {

        if (collision.CompareTag("Player") || collision.CompareTag("DeadBody"))
        {
            plateDown = false;
            anim.SetBool("plateOn", false);

        }
    }
}
