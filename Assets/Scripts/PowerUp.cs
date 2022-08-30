using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{

    [SerializeField]
    private GameObject explosion;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player1") || other.gameObject.CompareTag("Player2"))
        {
            GameObject activeExplosion = Instantiate(explosion, transform.position, transform.rotation);
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            gameObject.GetComponent<SphereCollider>().enabled = false;
            other.gameObject.GetComponent<PlayerPowers>().StartPowerUp(gameObject.name);
            StartCoroutine(StopExplosion(activeExplosion));
        }
    }

    IEnumerator StopExplosion(GameObject activeExplosion)
    {
        yield return new WaitForSeconds(2);
        Destroy(activeExplosion);
        Destroy(gameObject);
    }   

}
