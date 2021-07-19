using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doors : MonoBehaviour
{
    public int accessLevel = 0;
    public bool needKey = false;
    [SerializeField] private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!needKey)
        {
            
            if (collision.GetComponent<AccessLevel>().accessLevel == accessLevel)
            {

                openDoors();

            }
            else 
            if (collision.GetComponent<AccessLevel>().accessLevel >= accessLevel)
            {
                openDoors();
            }

            
        }
        else
        {
            if (collision.GetComponent<AccessLevel>().haveKey==true)
            {
                openDoors();
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        closeDoors();
    }
    public void openDoors()
    {
        //door animation with collider
        anim.SetBool("open", true);
        gameObject.GetComponent<EdgeCollider2D>().isTrigger = true;
    }
    public void closeDoors()
    {
        //door animation with collider
        anim.SetBool("open", false);
        gameObject.GetComponent<EdgeCollider2D>().isTrigger = false;
    }
}
