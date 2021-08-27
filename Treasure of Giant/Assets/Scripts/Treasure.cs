using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treasure : MonoBehaviour
{
     private void OnTriggerEnter(Collider other) 
     {
         if(other.TryGetComponent(out PlayerController player))
         {
             Destroy(gameObject);
             Debug.Log("You win!");
         }
     }
}
