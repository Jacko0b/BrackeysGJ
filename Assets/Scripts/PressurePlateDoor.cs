using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PressurePlateDoor : MonoBehaviour
{
    public Doors door;


    public PressurePlate plate1;
    public PressurePlate plate2;
    [SerializeField] private Animator anim;
    private void Awake()
    {
        door = GetComponent<Doors>();
        anim.GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if(plate1.plateDown == true && plate2.plateDown == true)
        {
            door.accessLevel = 0;
            anim.SetBool("doorOpen", true);
        }
        else
        {
            door.accessLevel = 99;
            anim.SetBool("doorOpen", false);

        }

    }
}
