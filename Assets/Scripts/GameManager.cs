using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Turn Management")]
    private GameObject ball;
    public int playerTurn;
    public GameObject player1;
    public GameObject player2;

    [Space]

    public Text player1Score;
    public Text player2Score;
    // Start is called before the first frame update
    void Start()
    {
        ball = GameObject.Find("Ball");
        SwitchPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        SwitchPlayer();
    }

    void SwitchPlayer()
    {
        if (ball.transform.position.z < 0)
        {
            playerTurn = 1;
            player1.GetComponent<PlayerActions>().playerTurn = true;
            player2.GetComponent<PlayerActions>().playerTurn = false;
        }
        else
        {
            playerTurn = 2;
            player2.GetComponent<PlayerActions>().playerTurn = true;
            player1.GetComponent<PlayerActions>().playerTurn = false;
        }
    }

}
