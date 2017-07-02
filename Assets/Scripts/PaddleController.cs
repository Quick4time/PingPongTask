using UnityEngine;

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
    private BallController ballController;

    private Vector2 curPos;

    private void Start()
    {
        BallGO = GameObject.FindGameObjectWithTag("Ball");
        ballController = (BallController)BallGO.GetComponent(typeof(BallController));
        curPos = transform.position;
    }

    public void Reset()
    {
        transform.position = curPos;
    }

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
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Reset();
        }
    }

    

    private void OnCollisionExit2D(Collision2D collision)
    {
        collision.rigidbody.velocity = new Vector2(collision.rigidbody.velocity.x * multipilerSpeedColBall, collision.rigidbody.velocity.y + (direction * adjustSpeed));
        Debug.Log(collision.rigidbody.velocity.ToString());
    }
}
