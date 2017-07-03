using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMenuPaddle : MonoBehaviour
{
    private float speed = 10;
    private float limitMovementPaddle = 7.5f;
    [SerializeField]
    SimpleMenuBall ball;

    private string Hit = "Hit";

    AudioManager audioManager;

    private void Start()
    {
        audioManager = AudioManager.instance;
        if (audioManager == null)
        {
            Debug.LogError("No audioManager found!");
        }
    }

    private void Update()
    {
        Vector2 pos = transform.position;
        pos = Vector2.MoveTowards(new Vector2(ball.transform.position.x, transform.position.y), transform.position, speed * Time.deltaTime);
        pos.x = Mathf.Clamp(pos.x, -limitMovementPaddle, limitMovementPaddle);
        transform.position = pos;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        audioManager.PlaySound(Hit);
    }
}
