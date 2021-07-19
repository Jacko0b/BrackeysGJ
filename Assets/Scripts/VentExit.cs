using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VentExit : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D collision)
    {
        GetComponent<BoxCollider2D>().isTrigger = false;
        GetComponent<EdgeCollider2D>().isTrigger = false;

        //change sprite to 
    }
}
