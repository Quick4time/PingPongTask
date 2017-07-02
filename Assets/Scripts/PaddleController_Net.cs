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
    private Color paddleColor = Color.blue;


    private void Start()
    {
        curPos = transform.position;
        if(!isLocalPlayer)
        {
            gameObject.GetComponent<SpriteRenderer>().color = paddleColor;
        }
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


    private void OnCollisionExit2D(Collision2D collision)
    {
        collision.rigidbody.velocity = new Vector2(collision.rigidbody.velocity.x * multipilerSpeedColBall, collision.rigidbody.velocity.y + (direction * adjustSpeed));
    }

    public void ResetPaddle()
    {
        transform.position = curPos;
    }
}
