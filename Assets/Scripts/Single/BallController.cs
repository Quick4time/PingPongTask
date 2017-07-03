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

    AudioManager audioManager;

    // Получаем ссылки на объекты.
    private void Awake()
    {
        Paddel1 = GameObject.FindGameObjectWithTag("Paddel1");
        Paddel2 = GameObject.FindGameObjectWithTag("Paddel2");
        GameMasterGO = GameObject.FindGameObjectWithTag("GM");
        GM = (GameManager)GameMasterGO.GetComponent(typeof(GameManager));
    }
    // Указываем стартовую позицию,проверяем ссылки на их отсутствие и запускаем сопрограмму по запуску мяча. 
    private void Start()
    {
        curPos = transform.position;
        myRigidBody = GetComponent<Rigidbody2D>();
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
        StartCoroutine(DelayStart());
    }
    // Сбрасываем позицию, velocity мяча к позиции инициализациии запускаем сопрограмму с таймером по запуску.
    public void ResetBall()
    {
        gameObject.SetActive(true);
        myRigidBody.velocity = Vector2.zero;
        transform.position = curPos;
        StartCoroutine(DelayStart());
    }

    // Сопрограмма запускающая обратный отчет после чего запускает мяч в 4-х разных направлениях
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
    
    // При пересечении триггера "ScoreZone", и если текущая позиция мяча больше или меньше по оси x то добавляем очко конкретному игроку, а зтем сбрасываем позицию и запускаем мяч в соответствии с позицией игрока.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("ScoreZone"))
        {
            audioManager.PlaySound("Goal");
            if (transform.position.x < 0)
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
