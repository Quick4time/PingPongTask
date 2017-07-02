using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GameManager_Net : NetworkBehaviour
{

    [SerializeField] public Text GoText;
    [SerializeField] public Text scoreText;
    [SerializeField] public Text winText;

    [SerializeField][SyncVar] private int scoreTarget;

    [SerializeField] private Shadow shadowTextScore;
    [SerializeField] private Shadow shadowTextWins;

    [SyncVar] private int scorePaddel1;
    [SyncVar] private int scorePaddel2;
    [SyncVar] private string WinPlayer;
    [SyncVar] private string CountText = "{0}";
    float timerLeft = 3.0f; 

    [SerializeField] public bool restarting = false;

    private GameObject ball;
    private BallController_Net ballController;

    private void Awake()
    {
        WinPlayer = "Player {0} Wins";
        shadowTextScore.effectColor = Color.white;
        shadowTextScore.effectDistance = new Vector2(0, 0);
        ball = GameObject.FindGameObjectWithTag("Ball");
        ballController = (BallController_Net)ball.GetComponent(typeof(BallController_Net));
    }

    private void Start()
    {
        restarting = true;
    }

    private void Update()
    {
        if (restarting)
        {
            RpcRestart();
        }

        if (scorePaddel1 >= scoreTarget)
        {
            winText.gameObject.SetActive(true);
            winText.text = string.Format(WinPlayer, 1);
            shadowTextWins.effectDistance = new Vector2(-5, 0);
            ballController.ResetBall();
            if(Input.GetKeyDown(KeyCode.Space))
            {
                restarting = true;
            }
        }
        if (scorePaddel2 >= scoreTarget)
        {
            winText.gameObject.SetActive(true);
            winText.text = string.Format(WinPlayer, 2);
            shadowTextWins.effectDistance = new Vector2(5, 0);
            ballController.ResetBall();
            if (Input.GetKeyDown(KeyCode.Space))
            {
                restarting = true;
            }
        }
    }

    [ClientRpc]
    void RpcRestart()
    {
        timerLeft -= Time.deltaTime;
        GoText.gameObject.SetActive(true);
        GoText.text = string.Format(CountText, Mathf.RoundToInt(timerLeft));
        if (timerLeft <= 0)
        {
            RpcResetGM();
            restarting = false;
            RpcHideText();
            timerLeft = 3.0f;
        }
    }

    [ClientRpc]
    public void RpcHideText()
    {
        GoText.text = string.Empty;
        winText.text = string.Empty;
    }

    [ClientRpc]
    public void RpcResetGM()
    {
        ballController.PullBall();
        scorePaddel1 = scorePaddel2 = 0;
        scoreText.text = string.Format("{0} - {1}", scorePaddel1, scorePaddel2);
        shadowTextScore.effectColor = Color.white;
        shadowTextScore.effectDistance = new Vector2(0, 0);
    }

    [ClientRpc]
    public void RpcUpdateScore(int player)
    {
        switch (player)
        {
            case 1:
                scorePaddel1++;
                shadowTextScore.effectColor = Color.black;
                shadowTextScore.effectDistance = new Vector2(-5, 0);
                break;
            case 2:
                scorePaddel2++;
                shadowTextScore.effectColor = Color.black;
                shadowTextScore.effectDistance = new Vector2(5, 0);
                break;
        }
        scoreText.text = string.Format("{0} - {1}", scorePaddel1, scorePaddel2);
    }
}
