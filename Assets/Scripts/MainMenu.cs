using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private string single;
    [SerializeField]
    private string multi;
    [SerializeField]
    private string twoPlayers;

    private string ButtonHover = "ButtonHover";
    private string ButtonPress = "ButtonPress";

    AudioManager audioManager;

    private Canvas Lobby;

    private void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
        LevelManager.setLastLevel(SceneManager.GetActiveScene().name);
    }

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
        if (SceneManager.GetActiveScene().name == single || SceneManager.GetActiveScene().name == twoPlayers)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                LevelManager.changeToPreviousLvl();
            }
        }   
        if(SceneManager.GetActiveScene().name == multi)
        {
            Lobby = GameObject.FindGameObjectWithTag("Lobby").GetComponent<Canvas>();
            Lobby.enabled = true;
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SceneManager.LoadScene("MainMenu");
                Lobby.enabled = false;
            }
        }
    }

    public void LoadSingle()
    {
        audioManager.PlaySound(ButtonPress);
        SceneManager.LoadScene(single);
    }
    public void LoadMulti()
    {
        audioManager.PlaySound(ButtonPress);
        SceneManager.LoadScene(multi);
    }
    public void TwoPlayers()
    {
        audioManager.PlaySound(ButtonPress);
        SceneManager.LoadScene(twoPlayers);
    }
    public void QuitGame()
    {
        audioManager.PlaySound(ButtonPress);
        Application.Quit();
    }

    public void OnMouseOver()
    {
        audioManager.PlaySound(ButtonHover);
    }
}

public static class LevelManager
{
    private static string lastLevel;

    public static void setLastLevel(string level)
    {
        lastLevel = level;
    }

    public static string getLastLevel()
    {
        return lastLevel;
    }

    public static void changeToPreviousLvl()
    {
        SceneManager.LoadScene(lastLevel);
    }
}
