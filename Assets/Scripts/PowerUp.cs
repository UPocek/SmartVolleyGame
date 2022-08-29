using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player1") || collision.gameObject.CompareTag("Player2"))
        {
            Destroy(gameObject);
            collision.gameObject.GetComponent<PlayerPowers>().StartPowerUp(gameObject.name);
        }
    }
}
