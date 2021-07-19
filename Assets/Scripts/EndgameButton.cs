using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndgameButton : MonoBehaviour
{
    public bool enabledButton = false;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            enabledButton = true;
        }  
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            enabledButton = false;
        }
    }
}
