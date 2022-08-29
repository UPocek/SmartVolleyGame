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
    private bool jumpPressed;
    // Start is called before the first frame update
    void Start()
    {
        playerRb = gameObject.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        jumpPressed = Input.GetKeyDown(KeyCode.Space);
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
        transform.Translate(Vector3.forward * horizontalInput * speed * Time.deltaTime);
        transform.Translate(Vector3.right * verticalInput * speed * Time.deltaTime);

        if(horizontalInput != 0 || verticalInput != 0)
        {
            GetComponent<Energy>().DecreaseEnergyWalk();
        }
    }

    void Jump()
    {
        if (jumpPressed)
        {
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            GetComponent<Energy>().DecreaseEnergyJump();
            jumpPressed = false;
        }
    }
}
