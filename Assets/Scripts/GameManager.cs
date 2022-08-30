using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Turn Management")]
    private GameObject ball;
    public int playerTurn;
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

    private int playerTouches;
    private int lastTouchIndex;

    private int player1Index = 1;
    private int player2Index = 2;

    private Vector3 ballPositionForPlayer1Serve = new(0f, 2.5f, -10);
    private Vector3 ballPositionForPlayer2Serve = new(0f, 2.5f, 10);
    private Vector3 player1ServePosition = new(0f, 1f, -10f);
    private Vector3 player1RecivePosition = new(0f, 1f, -5f);
    private Vector3 player2ServePosition = new(0f, 1f, 10f);
    private Vector3 player2RecivePosition = new(0f, 1f, 5f);
    private float playerSpeed;
    private float bottomOfGround;
    private float[] groundBounds = new float[2];

    bool gameFreezed = false;
    // Start is called before the first frame update
    void Start()
    {
        ball = GameObject.Find("Ball");

        playerTurn = Random.Range(1, 3);
        lastTouchIndex = playerTurn;
        playerTouches = 0;
        ResetPlayers(playerTurn);
        StartCoroutine(ResetBall(playerTurn));
        

        GameObject ground = GameObject.FindGameObjectWithTag("Ground");
        bottomOfGround = ground.transform.position.y;
        groundBounds[0] = ground.transform.localScale.x;
        groundBounds[1] = ground.transform.localScale.z;

        playerSpeed = player1.GetComponent<PlayerActions>().speed;
    }

    // Update is called once per frame
    void Update()
    {
        SwitchPlayer();

        CheckIfBallFallOffPlatform();
    }

    void SwitchPlayer()
    {
        if (ball.transform.position.z < 0)
        {
            playerTurn = player2Index;
            player1.GetComponent<PlayerActions>().playerTurn = true;
            player2.GetComponent<PlayerActions>().playerTurn = false;
        }
        else
        {
            playerTurn = player2Index;
            player2.GetComponent<PlayerActions>().playerTurn = true;
            player1.GetComponent<PlayerActions>().playerTurn = false;
        }
    }

    void CheckIfBallFallOffPlatform()
    {
        if(ball.transform.position.y < bottomOfGround)
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

    bool IsBallInBounds(Vector3 position)
    {
        return !(position.x < (-groundBounds[0] / 2) || position.x > (groundBounds[0] / 2) || position.z < (-groundBounds[1] / 2) || position.z > (groundBounds[1] / 2));
    }

    bool IsShotWinner(Vector3 position)
    {
        if(lastTouchIndex == player1Index)
        {
            return position.z > 0;
        }
        else
        {
            return position.z < 0;
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
            return;
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

        yield return new WaitForSeconds(timeBetweenPoints);

        ResetPlayers(indexOfPlayerThatWonThePoint);

        StartCoroutine(ResetBall(indexOfPlayerThatWonThePoint));

        UnfreezeGame();
    }

    void FreezeGame()
    {
        gameFreezed = true;

        player1.GetComponent<PlayerActions>().speed = 0;
        player2.GetComponent<PlayerActions>().speed = 0;

        GameObject.Find("PowerUpSpawner").GetComponent<PowerUpSpawner>().enabled = false;
    }

    void UpdateScore(int index)
    {
        if(index == player1Index)
        {
            player1Score.text = (int.Parse(player1Score.text) + 1).ToString();
        }
        else
        {
            player2Score.text = (int.Parse(player2Score.text) + 1).ToString();
        }
    }

    void ResetPlayers(int indexOfPlayerThatWonThePoint)
    {
        player1.GetComponent<Rigidbody>().velocity = Vector3.zero;
        player2.GetComponent<Rigidbody>().velocity = Vector3.zero;

        if (indexOfPlayerThatWonThePoint == player1Index)
        {
            player1.transform.position = player1ServePosition;
            player2.transform.position = player2RecivePosition;
        }
        else
        {
            player1.transform.position = player1RecivePosition;
            player2.transform.position = player2ServePosition;
        }
    }

    IEnumerator ResetBall(int indexOfPlayerThatWonPoint)
    {
        ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
        ball.transform.position = indexOfPlayerThatWonPoint == player1Index ? ballPositionForPlayer1Serve : ballPositionForPlayer2Serve;
        ball.GetComponent<Rigidbody>().useGravity = false;

        yield return new WaitForSeconds(2);

        ball.GetComponent<Rigidbody>().useGravity = true;
        ball.GetComponent<BallMovement>().Serve(indexOfPlayerThatWonPoint == player1Index ? -1 : 1);
    }

    void UnfreezeGame()
    {
        player1.GetComponent<PlayerActions>().speed = playerSpeed;
        player2.GetComponent<PlayerActions>().speed = playerSpeed;

        GameObject.Find("PowerUpSpawner").GetComponent<PowerUpSpawner>().enabled = false;

        gameFreezed = false;
    }
}
