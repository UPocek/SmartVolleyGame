using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    private Energy energyBar;
    private Rigidbody playerRb;
    private float horizontalInput;
    private float verticalInput;
    public float speed;
    public float jumpForce;
    public bool playerTurn;
    public bool jumpPressed;
    public bool spikePressed;
    private bool isOnGround;
    private float forwardBorder = 11;
    private float sideBorder = 6.5f;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = gameObject.GetComponent<Rigidbody>();
        energyBar = gameObject.GetComponent<Energy>();
    }

    private void Update()
    {
        jumpPressed = Input.GetKeyDown(KeyCode.Space);
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        if (playerTurn)
        {
            Move();
            if (jumpPressed)
            {
                Jump();
            }

            if (Input.GetKeyDown(KeyCode.X))
            {
                Spike();
            }
        }
    }

    void Move()
    {
        if (hasEnergy("walking"))
        { 
            transform.Translate(horizontalInput * speed * Time.deltaTime * Vector3.forward);
            transform.Translate(speed * Time.deltaTime * verticalInput * Vector3.left);
            MovementConstraint();
            if(horizontalInput != 0 || verticalInput != 0)
            {
                GetComponent<Energy>().DecreaseEnergyWalk();
            }
        }
        
    }

    void Jump()
    {
        if (hasEnergy("jumping") && isOnGround)
        {
            isOnGround = false;
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            GetComponent<Energy>().DecreaseEnergyJump();
            jumpPressed = false;
        }
    }

    void Spike()
    {
        if (hasEnergy("spiking") && isOnGround)
        {
            isOnGround = false;
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            GetComponent<Energy>().DecreaseEnergySpike();
            spikePressed = true;
        }
    }

    bool hasEnergy(string action)
    {
        if (action.Equals("walking"))
        {
            if (energyBar.EnergyLevel < energyBar.walkCost)
            {
                return false;
            }
            return true;
        }
        else if (action.Equals("jumping"))
        {
            if (energyBar.EnergyLevel < energyBar.jumpCost)
            {
                return false;
            }
            return true;
        }
        else 
        {
            if (energyBar.EnergyLevel < energyBar.spikeCost)
            {
                return false;
            }
            return true;
        }


    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
            spikePressed = false;
        }
    }

    void MovementConstraint()
    {
        if (transform.position.z > forwardBorder)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, forwardBorder);
        } else if (transform.position.z < -forwardBorder)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, -forwardBorder);
        }
        if (transform.position.x > sideBorder)
        {
            transform.position = new Vector3(sideBorder, transform.position.y, transform.position.z);
        }
        else if (transform.position.x < -sideBorder)
        {
            transform.position = new Vector3(-sideBorder, transform.position.y, transform.position.z);
        }
    }
}
