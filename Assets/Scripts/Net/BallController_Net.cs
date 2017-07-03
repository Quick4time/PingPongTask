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
    private Vector2 startPos;

    AudioManager audioManager;

    // Получаем ссылки на объекты.
    private void Awake()
    {
        GameMasterGO = GameObject.FindGameObjectWithTag("GM");
        GM = (GameManager_Net)GameMasterGO.GetComponent(typeof(GameManager_Net));
        myRigidBody = GetComponent<Rigidbody2D>();
    }

    // Указываем стартовую позицию и проверяем ссылки на их отсутствие.
    private void Start()
    {
        startPos = transform.position;
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

    // Сбрасываем позицию, velocity мяча к позиции инициализации.
    public void ResetBall()
    {
        myRigidBody.velocity = Vector2.zero;
        transform.position = startPos;
    }

    // Запускаем мяч в 4-х случайных направлениях.
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

    // При пересечении триггера "ScoreZone", и если текущая позиция мяча больше или меньше по оси x то добавляем очко конкретному игроку, а зтем сбрасываем позицию и запускаем мяч.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("ScoreZone"))
        {
            // Воспроизведение звука "Goal"
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
