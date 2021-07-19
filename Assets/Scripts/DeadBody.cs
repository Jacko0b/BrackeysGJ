using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadBody : MonoBehaviour
{
    private void Start()
    {
        deleteObj();    
    }
    private void deleteObj()
    {
        Destroy(gameObject, 60f);
    }
}
