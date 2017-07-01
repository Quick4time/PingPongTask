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
}
