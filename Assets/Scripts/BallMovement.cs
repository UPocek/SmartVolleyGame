using System.Collections;
using UnityEngine;

public class BallMovement : MonoBehaviour
{
    [SerializeField]
    private GameManager gameManager;
    [SerializeField]
    private GameObject player1;
    [SerializeField]
    private GameObject player2;

    private Rigidbody ballRb;

    [HideInInspector]
    public float spikePower = 400f;
    private float hitPower = 2.5f;
    private float ballBounceness = 4f;
    private float spikeArcY = 60f;
 
    // Start is called before the first frame update
    void Start()
    {
        ballRb = gameObject.GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player1") && !collision.gameObject.GetComponent<VolleyAIv1>().spikePressed)
        { 
            PlayerHit(1, "basicHit");  
        }
        else if (collision.gameObject.CompareTag("Player2") && !collision.gameObject.GetComponent<VolleyAIv1>().spikePressed)
        {
            PlayerHit(-1, "basicHit");
        }
        else if (collision.gameObject.CompareTag("Ground"))
        {
            gameManager.GroundContact(transform.localPosition);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player1") && other.gameObject.GetComponent<VolleyAIv1>().spikePressed)
        {
            PlayerHit(1, "spike");
            other.gameObject.GetComponent<VolleyAIv1>().spikePressed = false;
        }
        else if (other.gameObject.CompareTag("Player2") && other.gameObject.GetComponent<VolleyAIv1>().spikePressed)
        {
            PlayerHit(-1, "spike");
            other.gameObject.GetComponent<VolleyAIv1>().spikePressed = false;
        }
    }

    void PlayerHit(int direction, string typeOfHit)
    {
        gameManager.Touch(direction);

        if (typeOfHit.Equals("spike"))
        {
            ballRb.AddForce(new Vector3(GetBallRandomXOffset(), spikeArcY, direction * spikePower));
            StartCoroutine(BallDownForce(direction));
        }
        else
        {
            float playerInconsistencyY = Random.Range(1f, 1.1f);
            float playerInconsistencyZ = Random.Range(1f, 1.25f);
            ballRb.AddForce(new Vector3(GetBallRandomXOffset(), ballBounceness * playerInconsistencyY, direction * hitPower * playerInconsistencyZ), ForceMode.Impulse);
        }
    }

    IEnumerator BallDownForce(int direction)
    {
        yield return new WaitForSeconds(0.1f);
        ballRb.AddForce(new Vector3(0, -spikeArcY * 11, direction));
    }

    float GetBallRandomXOffset()
    {
        float ballMovementX = Random.Range(0f, 0f);

        float ballCurrentXTrajectory = (transform.localPosition.x + 0.1f) / Mathf.Abs(transform.localPosition.x + 0.1f);

        return ballMovementX * -ballCurrentXTrajectory;
    }
}
