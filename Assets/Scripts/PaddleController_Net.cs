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

    private GameObject ball_Go;
    private BallController_Net ball;

    private GameObject gm_Go;
    private GameManager_Net GM;

    private void Start()
    {
        gm_Go = GameObject.FindGameObjectWithTag("GM");
        GM = (GameManager_Net)gm_Go.GetComponent(typeof(GameManager_Net));

        ball_Go = GameObject.FindGameObjectWithTag("Ball");
        ball = (BallController_Net)ball_Go.GetComponent(typeof(BallController_Net));

        curPos = transform.position;
    }

     void Update()
    {
        if (!isLocalPlayer)
            return; 

        Vector2 pos = transform.position;
        if (Input.GetKey(KeyCode.W))
        {
            pos = new Vector2(transform.position.x, transform.position.y + (speed * Time.deltaTime));
            direction = 1.0f;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            pos = new Vector2(transform.position.x, transform.position.y - (speed * Time.deltaTime));
            direction = 1.0f;
        }
        else
        {
            direction = 0.0f;
        }
        pos.y = Mathf.Clamp(pos.y, -limitMovementPaddle, limitMovementPaddle);
        transform.position = pos;
    }

    private void OnConnectedToServer()
    {
        RpcReset();
        GM.RpcResetGM();
        ball.RpcResetBall();
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        collision.rigidbody.velocity = new Vector2(collision.rigidbody.velocity.x * multipilerSpeedColBall, collision.rigidbody.velocity.y + (direction * adjustSpeed));
    }

    [ClientRpc]
    public void RpcReset()
    {
        transform.position = curPos;
    }
}
