using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
public class BallController_Net : NetworkBehaviour
{
    [SerializeField]
    private float startForce;
    private Rigidbody2D myRigidBody;
    private GameObject GameMasterGO;
    private GameManager_Net GM;
    private Vector2 curPos;

    AudioManager audioManager;

    private void Awake()
    {
        GameMasterGO = GameObject.FindGameObjectWithTag("GM");
        GM = (GameManager_Net)GameMasterGO.GetComponent(typeof(GameManager_Net));
        myRigidBody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        curPos = transform.position;
        if (myRigidBody != null)
        {
            myRigidBody.gravityScale = 0.0f;
            myRigidBody.sharedMaterial = Resources.Load("PhysicsMaterials/Bouncy") as PhysicsMaterial2D;
        }

        audioManager = AudioManager.instance;
        if (audioManager == null)
        {
            Debug.LogError("No audioManager found!");
        }
    }

    public void ResetBall()
    {
        myRigidBody.velocity = Vector2.zero;
        transform.position = curPos;
    }

    public void PullBall()
    {
        var random = new[]
        {
            ProportionValue.Create(0.25f, new Vector2(startForce, startForce)),
            ProportionValue.Create(0.25f, new Vector2(startForce, -startForce)),
            ProportionValue.Create(0.25f, new Vector2(-startForce, -startForce)),
            ProportionValue.Create(0.25f, new Vector2(-startForce, startForce)),
        };

        myRigidBody.velocity = random.ChoseByRandom();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("ScoreZone"))
        {
            audioManager.PlaySound("Goal");
            if (transform.position.x < 0)
            {
                GM.RpcUpdateScore(2);
            }  
            else
            {
                GM.RpcUpdateScore(1);
            }    
            ResetBall();
            PullBall();
        }
    }
}
