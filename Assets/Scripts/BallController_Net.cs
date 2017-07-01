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

    [SerializeField]
    private Text GoText;

    [SerializeField]
    private bool resetting;

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
            RpcResetBall();
        }
    }

    [ClientRpc]
    public void RpcResetBall()
    {
        myRigidBody.velocity = Vector2.zero;
        transform.position = curPos;
        StartCoroutine(DelayStart());
    }

   
    private void Update()
    {
        if (resetting)
        {
            myRigidBody.velocity = Vector2.zero;
            transform.position = curPos;
            return;
        }
    }
    
    public IEnumerator DelayStart()
    {
        resetting = true;
        var random = new[]
        {
            ProportionValue.Create(0.25f, new Vector2(startForce, startForce)),
            ProportionValue.Create(0.25f, new Vector2(startForce, -startForce)),
            ProportionValue.Create(0.25f, new Vector2(-startForce, -startForce)),
            ProportionValue.Create(0.25f, new Vector2(-startForce, startForce)),
        };
        GoText.gameObject.SetActive(true);
        GoText.text = "Ready";
        GoText.GetComponent<Shadow>().effectColor = Color.red;
        GoText.GetComponent<Shadow>().effectDistance = new Vector2(-6, 0);
        yield return new WaitForSeconds(1);
        GoText.text = "Steady";
        GoText.GetComponent<Shadow>().effectColor = Color.blue;
        GoText.GetComponent<Shadow>().effectDistance = new Vector2(6, 0);
        yield return new WaitForSeconds(1);
        GoText.text = "Go";
        GoText.GetComponent<Shadow>().effectColor = Color.green;
        GoText.GetComponent<Shadow>().effectDistance = new Vector2(0, 6);
        yield return new WaitForSeconds(1);
        GoText.gameObject.SetActive(false);
        resetting = false;
        GM.RpcResetGM();
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
