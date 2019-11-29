using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyLimit : MonoBehaviour
{ 
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Limit"))
        {
            Destroy(gameObject);
        }
    }
}
