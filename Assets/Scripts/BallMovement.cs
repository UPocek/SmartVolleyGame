using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour
{
    private Rigidbody ballRb;
    private GameManager gameManager;
    private float hitPower = 2.5f;
    private float ballBounceness = 6;
    private float spikePower = 100;
    // Start is called before the first frame update
    void Start()
    {
        ballRb = gameObject.GetComponent<Rigidbody>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
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
            Debug.Log("Spike Done");
        }
        else
        {
            ballRb.AddForce(new Vector3(0, ballBounceness, direction * hitPower), ForceMode.Impulse);
        }
    }
}
