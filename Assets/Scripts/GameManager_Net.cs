using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager_Net : NetworkBehaviour {

    [SerializeField]
    public Text scoreText;
    [SerializeField]
    private Text winText;
    [SerializeField]
    [SyncVar]
    private int scoreTarget;
    [SerializeField]
    private Shadow shadowTextScore;
    [SerializeField]
    private Shadow shadowTextWins;
    [SyncVar]
    private int scorePaddel1;
    [SyncVar]
    private int scorePaddel2;

  
    private GameObject ball;

    private void Awake()
    {
        shadowTextScore.effectColor = Color.white;
        shadowTextScore.effectDistance = new Vector2(0, 0);
        ball = GameObject.FindGameObjectWithTag("Ball");
    }


    private void Update()
    {
        if (scorePaddel1 >= scoreTarget)
        {
            winText.gameObject.SetActive(true);
            winText.text = "Player 1 Wins";
            shadowTextWins.effectColor = Color.red;
            shadowTextWins.effectDistance = new Vector2(-5, 0);
            ball.SetActive(false);
        }
        if (scorePaddel2 >= scoreTarget)
        {
            winText.gameObject.SetActive(true);
            winText.text = "Player 2 Wins";
            shadowTextWins.effectColor = Color.blue;
            shadowTextWins.effectDistance = new Vector2(5, 0);
            ball.SetActive(false);
        }
    }

    [ClientRpc]
    public void RpcResetGM()
    {
        scorePaddel1 = scorePaddel2 = 0;
        scoreText.text = string.Format("{0} - {1}", scorePaddel1, scorePaddel2);
        shadowTextScore.effectColor = Color.white;
        shadowTextScore.effectDistance = new Vector2(0, 0);
    }

    [ClientRpc]
    public void RpcUpdateScore(int player)
    {
        if(!isLocalPlayer)
        {
            switch (player)
            {
                case 1:
                    scorePaddel1++;
                    shadowTextScore.effectColor = Color.red;
                    shadowTextScore.effectDistance = new Vector2(-5, 0);
                    break;
                case 2:
                    scorePaddel2++;
                    shadowTextScore.effectColor = Color.blue;
                    shadowTextScore.effectDistance = new Vector2(5, 0);
                    break;
            }
            scoreText.text = string.Format("{0} - {1}", scorePaddel1, scorePaddel2);
        }
    }
}
