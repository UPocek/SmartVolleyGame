using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour
{
    private Rigidbody ballRb;
    public float hitPower = 2.5f;
    public float ballBounceness = 6f;
    public float spikePower = 100f;

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
        if (collision.gameObject.CompareTag("Player1"))
        {
            if (collision.gameObject.GetComponent<PlayerMovement>().spikePressed)
            {
                PlayerHit(1, "spike");
            }
            else
            {
                PlayerHit(1, "basicHit");
            }
            
        }
        else if (collision.gameObject.CompareTag("Player2"))
        {
            if (collision.gameObject.GetComponent<PlayerMovement>().spikePressed)
            {
                PlayerHit(-1, "spike");
            }
            else
            {
                PlayerHit(-1, "basicHit");
            }
        }
        else if (collision.gameObject.CompareTag("Ground"))
        {
          
        }
    }

    void PlayerHit(int direction, string typeOfHit)
    {
        if (typeOfHit.Equals("spike"))
        {
            ballRb.AddForce(new Vector3(0, -spikePower, direction * hitPower), ForceMode.Impulse);
        }
        else
        {
            ballRb.AddForce(new Vector3(0, ballBounceness, direction * hitPower), ForceMode.Impulse);
        }
    }
}
