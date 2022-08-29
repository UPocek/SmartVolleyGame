using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private GameObject ball;
    public int playerTurn;
    public GameObject player1;
    public GameObject player2;
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
            player1.GetComponent<PlayerMovement>().playerTurn = true;
            player2.GetComponent<PlayerMovement>().playerTurn = false;
        }
        else
        {
            playerTurn = 2;
            player2.GetComponent<PlayerMovement>().playerTurn = true;
            player1.GetComponent<PlayerMovement>().playerTurn = false;
        }
    }

}
