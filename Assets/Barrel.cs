using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrel : MonoBehaviour
{
    public bool clear = true;

    private void OnTriggerEnter(Collider other)
    {
        clear = false;
    }

    private void OnTriggerExit(Collider other)
    {
        clear = true;  
    }
}
