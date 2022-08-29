using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody playerRb;
    private float horizontalInput;
    private float verticalInput;
    public float speed;
    public float jumpForce;
    public bool playerTurn;
    // Start is called before the first frame update
    void Start()
    {
        playerRb = gameObject.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (playerTurn)
        { 
            Move();
            Jump();
        } 
    }

    void Move()
    {
        transform.Translate(Vector3.right * horizontalInput * speed * Time.deltaTime);
        transform.Translate(Vector3.forward * verticalInput * speed * Time.deltaTime);

        if(horizontalInput != 0 || verticalInput != 0)
        {
            GetComponent<Energy>().DecreaseEnergyWalk();
        }
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            GetComponent<Energy>().DecreaseEnergyJump();
        }
    }
}
