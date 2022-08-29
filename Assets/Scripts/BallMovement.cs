using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour
{
    private Rigidbody ballRb;
    private float hitPower = 2.5f;
    private float ballBounceness = 6;
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
            PlayerHit(1);
        }
        else if (collision.gameObject.CompareTag("Player2"))
        {
            PlayerHit(-1);
        }
    }

    void PlayerHit(int direction)
    {
       ballRb.AddForce(new Vector3(0, ballBounceness, direction * hitPower), ForceMode.Impulse);
    }
}
