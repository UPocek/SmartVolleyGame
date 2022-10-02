using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class VolleyAIv1 : Agent
{
    [HideInInspector]
    public bool isHeuristic = false;

    [Header("Environment references")]
    [SerializeField]
    private Transform ball;
    [SerializeField]
    private Transform opponent;
    [SerializeField]
    private GameManager gameManager;
    [SerializeField]
    private GameObject cort;

    [Header("Player Stats")]
    public float speed;
    public float jumpForce;
    public bool jumpPressed;
    public bool spikePressed;

    private Rigidbody playerRb;
    private Energy energyBar;
    private float horizontalInput;
    private float verticalInput;
    private bool isOnGround;
    private float forwardBorder;
    private float sideBorder;
    private int myPlayerIndex;
    private float winnerReward;

    public override void Initialize()
    {
        playerRb = gameObject.GetComponent<Rigidbody>();
        energyBar = gameObject.GetComponent<Energy>();

        forwardBorder = cort.transform.Find("UpOut").transform.localPosition.z + cort.transform.Find("UpOut").transform.localScale.x / 2;
        sideBorder = cort.transform.Find("RightOut").transform.localPosition.x + cort.transform.Find("RightOut").transform.localScale.x / 2;

        myPlayerIndex = int.Parse(gameObject.tag.Split("Player")[1]);
    }

    public override void OnEpisodeBegin()
    {
        winnerReward = 1f;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);

        bool myTurn = gameManager.lastTouchIndex == myPlayerIndex;
        sensor.AddObservation(myTurn ? gameManager.playerTouches : -1);

        sensor.AddObservation(GetComponent<Energy>().energyLevel);

        sensor.AddObservation(opponent.localPosition);

        sensor.AddObservation(ball.localPosition - transform.localPosition);
        sensor.AddObservation(ball.GetComponent<Rigidbody>().velocity);

        bool gemCollected = false;
        GameObject[] activeGems = GameObject.FindGameObjectsWithTag("Gem");

        foreach (GameObject gem in activeGems)
        {
            if (myPlayerIndex == gameManager.player1Index && gem.transform.position.z < 0)
            {
                sensor.AddObservation(true);
                sensor.AddObservation(gem.transform.localPosition);
                gemCollected = true;
            }
            if (myPlayerIndex == gameManager.player2Index && gem.transform.position.z > 0)
            {
                sensor.AddObservation(true);
                sensor.AddObservation(gem.transform.localPosition);
                gemCollected = true;
            }
        }
        
        if(!gemCollected)
        {
            sensor.AddObservation(false);
            sensor.AddObservation(0);
            sensor.AddObservation(0);
            sensor.AddObservation(0);
        }
        
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        if (!isHeuristic)
        {
            switch (actions.DiscreteActions[0])
            {
                case 0: Move(-1, 0); break;
                case 1: Move(1, 0); break;
                case 2: Move(0, -1); break;
                case 3: Move(0, 1); break;
                case 4: Move(0, 0); break;
                case 5: Jump(); break;
                case 6: Spike(); break;
            }
        }

        winnerReward -= 0.001f;

    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        isHeuristic = true;
    }

    private void Update()
    {
        if (isHeuristic)
        {
            jumpPressed = Input.GetKeyDown(KeyCode.Space);
            horizontalInput = Input.GetAxis("Horizontal");
            verticalInput = Input.GetAxis("Vertical");

            Move(horizontalInput, verticalInput);
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

    public void AddRewardForBallOverNet()
    {

    }

    // Player Actions

    void Move(float horizontalInput, float verticalInput)
    {
        if (HasEnergy("walking"))
        {
            transform.Translate(horizontalInput * speed * Time.deltaTime * Vector3.forward);
            transform.Translate(verticalInput * speed * Time.deltaTime * Vector3.left);
            MovementConstraint();
            if (horizontalInput != 0 || verticalInput != 0)
            {
                GetComponent<Energy>().DecreaseEnergyWalk();
            }
        }
    }

    void Jump()
    {
        if (HasEnergy("jumping") && isOnGround)
        {
            isOnGround = false;
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            GetComponent<Energy>().DecreaseEnergyJump();
            jumpPressed = false;
        }
    }

    void Spike()
    {
        if (HasEnergy("spiking") && isOnGround)
        {
            isOnGround = false;
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            GetComponent<Energy>().DecreaseEnergySpike();
            spikePressed = true;
        }
    }

    private bool HasEnergy(string action)
    {
        if (action.Equals("walking"))
        {
            return energyBar.energyLevel >= energyBar.walkCost;
        }
        else if (action.Equals("jumping"))
        {
            return energyBar.energyLevel >= energyBar.jumpCost;
        }
        else
        {
            return energyBar.energyLevel >= energyBar.spikeCost;
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
        if (transform.localPosition.z > forwardBorder)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, forwardBorder);
        }
        else if (transform.localPosition.z < -forwardBorder)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, -forwardBorder);
        }
        if (transform.localPosition.x > sideBorder)
        {
            transform.localPosition = new Vector3(sideBorder, transform.localPosition.y, transform.localPosition.z);
        }
        else if (transform.localPosition.x < -sideBorder)
        {
            transform.localPosition = new Vector3(-sideBorder, transform.localPosition.y, transform.localPosition.z);
        }
    }
}
