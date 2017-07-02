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
    private float startForce; //
    private Rigidbody2D myRigidBody; //
    private GameObject GameMasterGO;
    private GameManager_Net GM;
    private Vector2 curPos; // 
    [SyncVar]
    float timerLeft = 3.0f; //

    [SerializeField]
    [SyncVar]
    public bool restarting = false; //

    [SerializeField]
    public Text GoText; //

    [SyncVar]
    private string CountText;//

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

        CountText = "{0}";
        restarting = true;
    }
    public void Update()
    {
        Debug.Log(timerLeft.ToString());
        if (restarting)
        {
            RpcRestart();
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
            GM.RpcResetGM();
            restarting = false;
            RpcHideText();
            timerLeft = 3.0f;
        }
    }
    [ClientRpc]
    public void RpcHideText()
    {
        GoText.text = string.Empty;
        GM.winText.text = string.Empty;
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
