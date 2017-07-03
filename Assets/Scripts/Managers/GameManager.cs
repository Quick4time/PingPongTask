using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Text scoreText;
    [SerializeField]
    private Text winText;
    [SerializeField]
    private int scoreTarget;
    [SerializeField]
    private Shadow shadowTextScore;
    [SerializeField]
    private Shadow shadowTextWins;

    private int scorePaddel1;
    private int scorePaddel2;

    private GameObject ball;
    private BallController ballController;

    // Получаем ссылки на объекты.
    private void Awake()
    {
        ball = GameObject.FindGameObjectWithTag("Ball");
        ballController = (BallController)ball.GetComponent(typeof(BallController));
    }
    // Если один из игроков набирает необходимое количество очков, выводим сообщение о его победе, после чего можно перезагрузить игру нажатием клавиши.
    private void Update()
    {
        if (scorePaddel1 >= scoreTarget)
        {
            winText.gameObject.SetActive(true);
            winText.text = "Player 1 Wins";
            shadowTextWins.effectColor = Color.red;
            shadowTextWins.effectDistance = new Vector2(-5, 0);
            if(Input.GetKeyDown(KeyCode.Space))
            {
                ResetGM();
                ballController.ResetBall();
            }
        }
        if (scorePaddel2 >= scoreTarget)
        {
            winText.gameObject.SetActive(true);
            winText.text = "Player 2 Wins";
            shadowTextWins.effectColor = Color.blue;
            shadowTextWins.effectDistance = new Vector2(5, 0);
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ResetGM();
                ballController.ResetBall();
            }
        }
    }
    // Сброс счета и текста.
    public void ResetGM()
    {
        scorePaddel1 = scorePaddel2 = 0;
        scoreText.text = string.Format("{0} - {1}", scorePaddel1, scorePaddel2);
        shadowTextScore.effectColor = Color.white;
        shadowTextScore.effectDistance = new Vector2(0, 0);
        winText.gameObject.SetActive(false);
    }

    // Изменение счета при забитии
    public void UpdateScore(int player)
    {
        switch (player)
        {
            case 1:
                scorePaddel1 += 1;
                shadowTextScore.effectColor = Color.red;
                shadowTextScore.effectDistance = new Vector2(-5, 0);
                break;
            case 2:
                scorePaddel2 += 1;
                shadowTextScore.effectColor = Color.blue;
                shadowTextScore.effectDistance = new Vector2(5, 0);
                break;
        }
        scoreText.text = string.Format("{0} - {1}", scorePaddel1, scorePaddel2);
    }
}
