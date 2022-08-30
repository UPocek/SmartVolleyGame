using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour
{
    private Rigidbody ballRb;
    public float hitPower = 1000f;
    public float ballBounceness = 4;
    public float spikePower = 200f;

    // Start is called before the first frame update
    void Start()
    {
        ballRb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player1") && !collision.gameObject.GetComponent<PlayerActions>().spikePressed)
        { 
            PlayerHit(1, "basicHit");  
        }
        else if (collision.gameObject.CompareTag("Player2") && !collision.gameObject.GetComponent<PlayerActions>().spikePressed)
        {
            PlayerHit(-1, "basicHit");
        }
        else if (collision.gameObject.CompareTag("Ground"))
        {
          
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player1") && other.gameObject.GetComponent<PlayerActions>().spikePressed)
        {
            PlayerHit(1, "spike");
        }
        else if (other.gameObject.CompareTag("Player2") && other.gameObject.GetComponent<PlayerActions>().spikePressed)
        {
            PlayerHit(-1, "spike");
        }
    }

    void PlayerHit(int direction, string typeOfHit)
    {
        if (typeOfHit.Equals("spike"))
        {
            Debug.Log("Spike");
            ballRb.AddForce(new Vector3(0, -spikePower, direction * hitPower * 300));
            StartCoroutine(BallDown(direction));
        }
        else
        {
            Debug.Log("Problem");
            ballRb.AddForce(new Vector3(0, ballBounceness, direction * hitPower), ForceMode.Impulse);
        }
    }

    IEnumerator BallDown(int direction)
    {
        yield return new WaitForEndOfFrame();
        ballRb.AddForce(new Vector3(0, 100, direction * 50));
    }
}
