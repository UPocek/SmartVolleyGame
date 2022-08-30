using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour
{
    private Rigidbody ballRb;
    private float hitPower = 2f;
    private float ballBounceness = 4;
    public float spikePower;
 
    // Start is called before the first frame update
    void Start()
    {
        ballRb = gameObject.GetComponent<Rigidbody>();
        Serve(1);
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
            other.gameObject.GetComponent<PlayerActions>().spikePressed = false;
        }
        else if (other.gameObject.CompareTag("Player2") && other.gameObject.GetComponent<PlayerActions>().spikePressed)
        {
            PlayerHit(-1, "spike");
            other.gameObject.GetComponent<PlayerActions>().spikePressed = false;
        }
    }

    void PlayerHit(int direction, string typeOfHit)
    {
        if (typeOfHit.Equals("spike"))
        {
            ballRb.AddForce(new Vector3(0, 50, direction * 25));
            StartCoroutine(BallDown(direction));
        }
        else
        {
            ballRb.AddForce(new Vector3(0, ballBounceness, direction * hitPower), ForceMode.Impulse);
        }
    }

    IEnumerator BallDown(int direction)
    {
        yield return new WaitForSeconds(0.1f);
        ballRb.AddForce(new Vector3(0, -spikePower, direction * hitPower * 400));
    }

    void Serve(int direction)
    {
        ballRb.transform.position = new Vector3(0, 2, direction * 10);
        float serveDistance = Random.Range(6.0f, 9.0f);
        ballRb.velocity = new Vector3(0, serveDistance, -direction * 10);
    }
}
