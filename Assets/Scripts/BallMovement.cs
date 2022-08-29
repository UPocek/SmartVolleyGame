using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour
{
    private Rigidbody ballRb;
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
       ballRb.AddForce(direction * new Vector3(0, 1, 1), ForceMode.Impulse);
    }
}
