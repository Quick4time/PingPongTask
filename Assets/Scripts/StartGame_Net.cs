using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class StartGame_Net : NetworkManager
{
    [Header("Some Variables")]
    [SerializeField]
    private Transform SpawnP1;
    [SerializeField]
    private Transform SpawnP2;
    [SerializeField]
    private GameObject paddlePrefab;

    private GameObject ballGo;
    private BallController_Net ballController;

    private void Awake()
    {
        ballGo = GameObject.FindGameObjectWithTag("Ball");
        ballController = (BallController_Net)ballGo.GetComponent(typeof(BallController_Net));
    }

    // При каждом подключении
    public override void OnServerConnect(NetworkConnection conn)
    {

        ballController.restarting = true;
    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        print("DisconnectServer");
        base.OnServerDisconnect(conn);
    }

    // Когда подключается хост
    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);
    }

    public override void OnClientDisconnect(NetworkConnection conn)
    {
        NetworkServer.DisconnectAll();
        base.OnClientDisconnect(conn);
    }

}
