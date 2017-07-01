using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
public class BallController : MonoBehaviour
{
    [SerializeField]
    private float startForce;
    private Rigidbody2D myRigidBody;

    private GameObject Paddel1;
    private GameObject Paddel2;

    private GameObject GameMasterGO;
    private GameManager GM;

    private Vector2 curPos;

    [SerializeField]
    private Text GoText;

    private void Awake()
    {
        Paddel1 = GameObject.FindGameObjectWithTag("Paddel1");
        Paddel2 = GameObject.FindGameObjectWithTag("Paddel2");
        GameMasterGO = GameObject.FindGameObjectWithTag("GM");
        GM = (GameManager)GameMasterGO.GetComponent(typeof(GameManager));
    }

    private void Start()
    {
        curPos = transform.position;
        myRigidBody = GetComponent<Rigidbody2D>();
        if (myRigidBody != null)
        {
            myRigidBody.gravityScale = 0.0f;
            myRigidBody.sharedMaterial = Resources.Load("PhysicsMaterials/Bouncy") as PhysicsMaterial2D;
            StartCoroutine(DelayStart());
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ResetBall();
        }
    }

    public void ResetBall()
    {
        StartCoroutine(DelayStart());
        myRigidBody.velocity = Vector2.zero;
        transform.position = curPos;
    }

    public IEnumerator DelayStart()
    {
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
        myRigidBody.velocity = random.ChoseByRandom();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("ScoreZone"))
        {
            if(transform.position.x < 0)
            {
                transform.position = (Vector2)Paddel1.transform.position + new Vector2(1.0f, 0.0f);
                GM.UpdateScore(2);
                myRigidBody.velocity = new Vector2(startForce, startForce);
 
            }
            else
            {
                transform.position = (Vector2)Paddel2.transform.position + new Vector2(-1.0f, 0.0f);
                GM.UpdateScore(1);
                myRigidBody.velocity = new Vector2(-startForce, -startForce);
            }
        }
    }
}
