using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GameManager_Net : NetworkBehaviour
{

    [SerializeField] public Text goText;
    [SerializeField] public Text scoreText;
    [SerializeField] public Text winText;

    [SerializeField] [SyncVar] private int scoreTarget;

    [SerializeField] private Shadow shadowTextScore;
    [SerializeField] private Shadow shadowTextWins;

    [SyncVar] private int scorePaddel1;
    [SyncVar] private int scorePaddel2;
    [SyncVar] private string winString; 
    [SyncVar] private string countString;
    [SyncVar] private string goString;
    [SyncVar] private string scoreString;
    float timerLeft = 3.0f; 

    [SerializeField] public bool restarting = false;

    private GameObject ball;
    private BallController_Net ballController;
    
    // Получаем ссылки на объекты.
    private void Awake()
    {
        ball = GameObject.FindGameObjectWithTag("Ball");
        ballController = (BallController_Net)ball.GetComponent(typeof(BallController_Net));
    }
    // Запуск игры с обратным отчетом
    private void Start()
    {
        restarting = true;
    }
    // Если один из игроков набирает необходимое количество очков, выводим сообщение о его победе, после чего можно перезагрузить игру нажатием клавиши.
    private void Update()
    {
        if (restarting)
        {
            RpcRestart();
        }
        goText.text = goString;
        winText.text = winString;
        scoreText.text = scoreString;
        if (scorePaddel1 >= scoreTarget)
        {
            winString = string.Format("Player {0} Wins Press 'Space' to Restart", 1);
            shadowTextWins.effectDistance = new Vector2(-5, 0);
            ballController.ResetBall();
            if(Input.GetKeyDown(KeyCode.Space))
            {
                restarting = true;
            }
        }
        if (scorePaddel2 >= scoreTarget)
        {
            winText.text = string.Format("Player {0} Wins Press 'Space' to Restart", 2);
            shadowTextWins.effectDistance = new Vector2(5, 0);
            ballController.ResetBall();
            if (Input.GetKeyDown(KeyCode.Space))
            {
                restarting = true;
            }
        }
    }
    // Рестарт игры с обратным отчетом
    [ClientRpc]
    void RpcRestart()
    {
        timerLeft -= Time.deltaTime;
        goString = string.Format("{0}", Mathf.RoundToInt(timerLeft));
        if (timerLeft <= 0)
        {
            RpcResetGM();
            restarting = false;
            RpcHideText();
            timerLeft = 3.0f;
        }
    }
    // Сброс текста и отправка клиенту сервером.
    [ClientRpc]
    public void RpcHideText()
    {
        winString = string.Empty;
        goString = string.Empty;        
    }
    // Сброс счета и текста и отправка клиенту сервером.
    [ClientRpc]
    public void RpcResetGM()
    {
        ballController.PullBall();
        scorePaddel1 = scorePaddel2 = 0;
        scoreString = string.Format("{0} - {1}", scorePaddel1, scorePaddel2);
        shadowTextScore.effectColor = Color.white;
        shadowTextScore.effectDistance = new Vector2(0, 0);
    }
    // Изменение счета при забитии и отправка клиенту сервером.
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
        scoreString = string.Format("{0} - {1}", scorePaddel1, scorePaddel2);
    }
}
