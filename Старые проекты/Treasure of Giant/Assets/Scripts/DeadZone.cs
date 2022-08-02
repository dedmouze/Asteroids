using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) 
    {
        if(other.TryGetComponent(out PlayerController player))
        {
            other.gameObject.SetActive(false);
            Debug.Log("You lose!");
        }
    }
}
