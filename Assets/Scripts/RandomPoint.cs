using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPoint : MonoBehaviour
{
    public Vector3 startingPosition;
    public Vector3 targetPosition;
    public bool idle = false;
    // Start is called before the first frame update
    void Start()
    {
        startingPosition = transform.position;
        InvokeRepeating("RandomPosition", 1, 3);
    }

    public void RandomPosition()
    {
        if(!idle)
        targetPosition = startingPosition + new Vector3(Random.value, Random.value, 0) * Random.Range(1f, 3f);
        transform.position = targetPosition;
    }
}
