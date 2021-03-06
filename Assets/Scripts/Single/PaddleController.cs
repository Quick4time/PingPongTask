﻿using UnityEngine;

public class PaddleController : MonoBehaviour {

    [SerializeField]
    private float speed;
    private float direction;
    [SerializeField]
    private float adjustSpeed;
    [SerializeField]
    private float multipilerSpeedColBall;

    private float limitMovementPaddle = 3.25f;
    [SerializeField]
    private bool isPlayerOne;

    [SerializeField]
    private bool isAI;

    private GameObject BallGO;
    BallController ballController;

    private Vector2 curPos;

    AudioManager audioManager;

    // Указываем текущую позицию, меняем цвет противника на синий и проверяем ссылки на их отсутствие.
    private void Start()
    {
        BallGO = GameObject.FindGameObjectWithTag("Ball");
        ballController = (BallController)BallGO.GetComponent(typeof(BallController));

        audioManager = AudioManager.instance;
        if (audioManager == null)
        {
            Debug.LogError("No audioManager found!");
        }
    }
    // Осущствляем управление и передвижение лопаткой в соответствии с определенным булином, а также устанавливаем границы передвижения.
    void Update ()
    {
        if (isAI)
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, ballController.transform.position.y), speed * Time.deltaTime);
        }
        else
        {
            if (isPlayerOne)
            {
                if (Input.GetKey(KeyCode.W))
                {
                    transform.position = new Vector2(transform.position.x, transform.position.y + (speed * Time.deltaTime));
                    direction = 1.0f;
                }
                else if (Input.GetKey(KeyCode.S))
                {
                    transform.position = new Vector2(transform.position.x, transform.position.y - (speed * Time.deltaTime));
                    direction = 1.0f;
                }
                else
                {
                    direction = 0.0f;
                }
            }
            else
            {
                if (Input.GetKey(KeyCode.UpArrow))
                {
                    transform.position = new Vector2(transform.position.x, transform.position.y + (speed * Time.deltaTime));
                    direction = 1.0f;
                }
                else if (Input.GetKey(KeyCode.DownArrow))
                {
                    transform.position = new Vector2(transform.position.x, transform.position.y - (speed * Time.deltaTime));
                    direction = 1.0f;
                }
                else
                {
                    direction = 0.0f;
                }
            }
        }
	   
        if (transform.position.y > limitMovementPaddle)
        {
            transform.position = new Vector2(transform.position.x, limitMovementPaddle);
        }
        else if(transform.position.y < -limitMovementPaddle)
        {
            transform.position = new Vector2(transform.position.x, -limitMovementPaddle);
        }
    }
    // При выходе из столкновения увеличиваем скорость мячу, задаем направление и проигрываем звук.
    private void OnCollisionExit2D(Collision2D collision)
    {
        collision.rigidbody.velocity = new Vector2(collision.rigidbody.velocity.x * multipilerSpeedColBall, collision.rigidbody.velocity.y + (direction * adjustSpeed));
        audioManager.PlaySound("Hit");
    }
}
