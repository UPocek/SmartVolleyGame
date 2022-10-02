using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Environment references")]
    [SerializeField]
    private GameObject ball;
    [SerializeField]
    private GameObject ground;
    [SerializeField]
    private GameObject powerUpSpawner;

    [Space]

    [Header("Turn Management")]
    [SerializeField]
    private GameObject player1;
    [SerializeField]
    private GameObject player2;
    [SerializeField]
    private float timeBetweenPoints;

    [Space]

    [Header("Score Management")]
    [SerializeField]
    private Text player1Score;
    [SerializeField]
    private Text player2Score;

    [HideInInspector]
    public readonly int player1Index = 1;
    [HideInInspector]
    public readonly int player2Index = 2;
    [HideInInspector]
    public int playerTouches;
    [HideInInspector]
    public int lastTouchIndex;
    [HideInInspector]
    public Vector3 ballPositionForPlayer1Serve = new(0f, 7f, -4.5f);
    [HideInInspector]
    public Vector3 ballPositionForPlayer2Serve = new(0f, 7f, 4.5f);
    [HideInInspector]
    public Vector3 player1ServePosition = new(0f, 1f, -4.5f);
    [HideInInspector]
    public Vector3 player1RecivePosition = new(0f, 1f, -4.5f);
    [HideInInspector]
    public Vector3 player2ServePosition = new(0f, 1f, 4.5f);
    [HideInInspector]
    public Vector3 player2RecivePosition = new(0f, 1f, 4.5f);

    private float playerSpeed;
    private float bottomOfGround;
    private float[] groundBounds = new float[2];
    private bool gameFreezed = false;

    // Start is called before the first frame update
    void Start()
    {
        playerSpeed = player1.GetComponent<VolleyAIv1>().speed;
        lastTouchIndex = Random.Range(1, 3);
        bottomOfGround = ground.transform.localPosition.y;
        groundBounds[0] = ground.transform.localScale.x;
        groundBounds[1] = ground.transform.localScale.z;

        ResetToStart();        
    }

    // Update is called once per frame
    void Update()
    {
        CheckIfBallFallOffPlatform();
    }

    public void ResetToStart()
    {
        FreezePlayers();
        ResetPlayers(lastTouchIndex);
        StartCoroutine(ResetBall(lastTouchIndex));
    }

    void CheckIfBallFallOffPlatform()
    {
        if(ball.transform.localPosition.y < bottomOfGround)
        {
            PointEnd(GetOppositePlayerIndex(lastTouchIndex));
        }
    }

    public void GroundContact(Vector3 contactPossition)
    {
        if (IsBallInBounds(contactPossition))
        {
            if (IsShotWinner(contactPossition))
            {
                PointEnd(lastTouchIndex);
            }
            else
            {
                PointEnd(GetOppositePlayerIndex(lastTouchIndex));
            }
        }
        else
        {            
            PointEnd(GetOppositePlayerIndex(lastTouchIndex));
        }
    }

    bool IsBallInBounds(Vector3 ballContactPosition)
    {
        return !(ballContactPosition.x < (-groundBounds[0] / 2) || ballContactPosition.x > (groundBounds[0] / 2) || ballContactPosition.z < (-groundBounds[1] / 2) || ballContactPosition.z > (groundBounds[1] / 2));
    }

    bool IsShotWinner(Vector3 localPosition)
    {
        if (lastTouchIndex == player1Index)
        {
            return localPosition.z > 0 && playerTouches > 0;
        }
        else
        {
            return localPosition.z < 0 && playerTouches > 0;
        }
    }

    public void Touch(int touchDirection)
    {
        int indexOfPlayerWithBall = touchDirection == 1 ? player1Index : player2Index;

        if(indexOfPlayerWithBall == lastTouchIndex)
        {
            playerTouches += 1;
        }
        else
        {
            lastTouchIndex = indexOfPlayerWithBall;
            playerTouches = 1;
        }

        if (playerTouches > 3)
        {
            PointEnd(GetOppositePlayerIndex(indexOfPlayerWithBall));
        }
    }

    int GetOppositePlayerIndex(int playerIndex)
    {
        return playerIndex == player1Index ? player2Index : player1Index;
    }

    void PointEnd(int indexOfPlayerThatWonThePoint)
    {
        if (gameFreezed) return;
        StartCoroutine(UpdateGame(indexOfPlayerThatWonThePoint));
    }

    IEnumerator UpdateGame(int indexOfPlayerThatWonThePoint)
    {
        FreezeGame();

        UpdateScore(indexOfPlayerThatWonThePoint);

        NotifyAgents(indexOfPlayerThatWonThePoint);

        yield return new WaitForSeconds(timeBetweenPoints);

        ResetPlayers(indexOfPlayerThatWonThePoint);

        StartCoroutine(ResetBall(indexOfPlayerThatWonThePoint));

        UnfreezeGame();
    }

    void FreezeGame()
    {
        gameFreezed = true;

        FreezePlayers();

        powerUpSpawner.GetComponent<PowerUpSpawner>().enabled = false;
    }

    void FreezePlayers()
    {
        player1.GetComponent<VolleyAIv1>().speed = 0;
        player2.GetComponent<VolleyAIv1>().speed = 0;
    }

    void UpdateScore(int indexOfPlayerThatWonThePoint)
    {
        if (indexOfPlayerThatWonThePoint == player1Index)
        {
            player1Score.text = (int.Parse(player1Score.text) + 1).ToString();
        }
        else
        {
            player2Score.text = (int.Parse(player2Score.text) + 1).ToString();
        }
    }

    void NotifyAgents(int indexOfPlayerThatWonThePoint)
    {
        player1.GetComponent<VolleyAIv1>().EndEpisode();
        player2.GetComponent<VolleyAIv1>().EndEpisode();
    }

    void ResetPlayers(int indexOfPlayerThatWonThePoint)
    {
        player1.GetComponent<Rigidbody>().velocity = Vector3.zero;
        player2.GetComponent<Rigidbody>().velocity = Vector3.zero;

        player1.GetComponent<Energy>().energyLevel = player1.GetComponent<Energy>().maxEnergyLevel;
        player2.GetComponent<Energy>().energyLevel = player2.GetComponent<Energy>().maxEnergyLevel;

        lastTouchIndex = indexOfPlayerThatWonThePoint;
        playerTouches = 0;

        if (indexOfPlayerThatWonThePoint == player1Index)
        {
            player1.transform.localPosition = player1ServePosition;
            player2.transform.localPosition = player2RecivePosition;
        }
        else
        {
            player1.transform.localPosition = player1RecivePosition;
            player2.transform.localPosition = player2ServePosition;
        }
    }

    IEnumerator ResetBall(int indexOfPlayerThatWonPoint)
    {
        ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
        ball.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        ball.transform.localPosition = indexOfPlayerThatWonPoint == player1Index ? ballPositionForPlayer1Serve : ballPositionForPlayer2Serve;
        ball.GetComponent<Rigidbody>().useGravity = false;

        yield return new WaitForSeconds(2);

        UnFreezePlayers();
        ball.GetComponent<Rigidbody>().useGravity = true;
    }

    void UnFreezePlayers()
    {
        player1.GetComponent<VolleyAIv1>().speed = playerSpeed;
        player2.GetComponent<VolleyAIv1>().speed = playerSpeed;
    }

    void UnfreezeGame()
    {
        powerUpSpawner.GetComponent<PowerUpSpawner>().enabled = true;
        gameFreezed = false;
    }
}
