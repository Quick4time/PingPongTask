using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    [SerializeField]
    private GameObject spawnPoint1;
    [SerializeField]
    private GameObject spawnPoint2;
    private Vector2 curPos;

    float timerLeft = 3.0f;
    [SyncVar]
    public bool restarting = false;

    [SerializeField]
    private Text GoText;

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
            PullBall();
        }
    }
    public void Update()
    {
        if (restarting)
        {
            RpcCountDown();
        }
    }

    [ClientRpc]
    void RpcCountDown()
    {
        timerLeft -= Time.deltaTime;
        GoText.gameObject.SetActive(true);
        GoText.text = "" + Mathf.RoundToInt(timerLeft);
        if (timerLeft < 0)
        {
            ResetBall();
            PullBall();
            GM.RpcResetGM();
            restarting = false;
            GoText.gameObject.SetActive(false);
            GM.winText.gameObject.SetActive(false);
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
            if (transform.position.x < 0)
            {
                transform.position = (Vector2)spawnPoint1.transform.position + new Vector2(1.0f, 0.0f);
                GM.RpcUpdateScore(2);
                myRigidBody.velocity = new Vector2(startForce, Random.Range(-startForce, startForce) * 1.3f);
            }
            else
            {
                transform.position = (Vector2)spawnPoint2.transform.position + new Vector2(-1.0f, 0.0f);
                GM.RpcUpdateScore(1);
                myRigidBody.velocity = new Vector2(-startForce, Random.Range(-startForce, startForce) * 1.3f);
            }
        }
    }
}
