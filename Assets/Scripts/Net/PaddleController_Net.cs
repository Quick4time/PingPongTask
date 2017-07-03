using UnityEngine.Networking;
using UnityEngine;

public class PaddleController_Net : NetworkBehaviour
{
    private float speed = 10.0f;
    private float direction;
    private float adjustSpeed = 2.0f;
    private float multipilerSpeedColBall = 1.10f;
    private float limitMovementPaddle = 3.25f;
    private Vector2 curPos;

    [SyncVar]
    private Vector2 syncPos;
    private float lerpRate = 15;
    private float threshold = 0.0001f;
    private Vector2 lastPos;

    [SyncVar]
    private Color paddleColor = Color.blue;

    AudioManager audioManager;

    // Указываем текущую позицию, меняем цвет противника на синий и проверяем ссылки на их отсутствие.
    private void Start()
    {
        curPos = transform.position;
        if (!isLocalPlayer)
        {
            gameObject.GetComponent<SpriteRenderer>().color = paddleColor;
        }
        audioManager = AudioManager.instance;
        if (audioManager == null)
        {
            Debug.LogError("No audioManager found!");
        }
    }

    // Осуществляется проверка на локального игрока и если условие возвращает истину, предаем управление игроку.
    private void Update()
    {
        if (isLocalPlayer)
        {
            if (Input.GetKey(KeyCode.W))
            {
                curPos = new Vector2(transform.position.x, transform.position.y + (speed * Time.deltaTime));
                direction = 1.0f;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                curPos = new Vector2(transform.position.x, transform.position.y - (speed * Time.deltaTime));
                direction = 1.0f;
            }
            else
            {
                direction = 0.0f;
            }
            curPos.y = Mathf.Clamp(curPos.y, -limitMovementPaddle, limitMovementPaddle);
            transform.position = curPos;
        }
    }
    // Выполняем фиксированое обновление Методов отвичающих за синхронизацию текущей позиции игрока
    private void FixedUpdate()
    {
        TransmitPosition();
        LerpPosition();
    }

    // Этот метод смещает игрока в зависимости от синхронизированной позиции.  
    void LerpPosition()
    {
        if (!isLocalPlayer)
        {
            curPos = Vector2.Lerp(curPos, syncPos, Time.deltaTime * lerpRate);
        }
    }

    // Предоставляет текущую позицию серверу.
    [Command]
    void CmdProvidePositionToServer(Vector2 pos)
    {
        syncPos = curPos;
    }

    // Передаем информацию о передвижении игрока серверу, и эти передачи зависят от величены передвижения. 
    [ClientCallback]
    void TransmitPosition()
    {
        if (isLocalPlayer && Vector2.Distance(curPos,lastPos) > threshold)
        {
            CmdProvidePositionToServer(curPos);
            lastPos = curPos;
        }
    }
    // При выходе из столкновения увеличиваем скорость мячу, задаем направление и проигрываем звук.
    private void OnCollisionExit2D(Collision2D collision)
    {
        collision.rigidbody.velocity = new Vector2(collision.rigidbody.velocity.x * multipilerSpeedColBall, collision.rigidbody.velocity.y + (direction * adjustSpeed));
        audioManager.PlaySound("Hit");
    }
}
