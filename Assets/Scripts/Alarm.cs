using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alarm : MonoBehaviour
{
    public GameObject guardPrefab;

    public void alarmActivated()
    {
        Instantiate(guardPrefab, transform.position, Quaternion.identity); 
        Instantiate(guardPrefab, transform.position - new Vector3(0.2f,0.2f, 0), Quaternion.identity);
        
        
    }

}
