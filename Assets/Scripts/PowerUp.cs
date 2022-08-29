using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player1") || other.gameObject.CompareTag("Player2"))
        {
            Destroy(gameObject);
            other.gameObject.GetComponent<PlayerPowers>().StartPowerUp(gameObject.name);
        }
    }

}
