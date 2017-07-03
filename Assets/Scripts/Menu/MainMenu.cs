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
    [SerializeField]
    GameObject settingsPanel;

    private string ButtonHover = "ButtonHover";
    private string ButtonPress = "ButtonPress";

    AudioManager audioManager;

    private Canvas Lobby;

    // Сохраняем текущую сцену и делаем обьект не удаляемый при загрузке нового уровня
    private void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
        LevelManager.setLastLevel(SceneManager.GetActiveScene().name);
    }

    // Проверка на отсутствие AudioManager'a
    private void Start()
    {
        audioManager = AudioManager.instance;
        if (audioManager == null)
        {
            Debug.LogError("No audioManager found!");
        }
    }

    // При нажатии escap'a возвращаемся на преведущую сцену.
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
    // при нажании на кнопку загружаем уровень и проигрываем звук.
    public void LoadSingle()
    {
        audioManager.PlaySound(ButtonPress);
        SceneManager.LoadScene(single);
    }
    // при нажании на кнопку загружаем уровень и проигрываем звук.
    public void LoadMulti()
    {
        audioManager.PlaySound(ButtonPress);
        SceneManager.LoadScene(multi);
    }
    // при нажании на кнопку загружаем уровень и проигрываем звук.
    public void TwoPlayers()
    {
        audioManager.PlaySound(ButtonPress);
        SceneManager.LoadScene(twoPlayers);
    }
    // при нажании на кнопку выходим из приложения и проигрываем звук.
    public void QuitGame()
    {
        audioManager.PlaySound(ButtonPress);
        Application.Quit();
    }
    // проигрываем звук если мышка проходит рядом с кнопкой
    public void OnMouseOver()
    {
        audioManager.PlaySound(ButtonHover);
    }
    // при наведении мышки показываем окно
    public void ShowSettings()
    {
        settingsPanel.SetActive(true);
    }
    // при наведении мышки скрываем окно
    public void HideSettings()
    {
        settingsPanel.SetActive(false);
    }
}

// Статический класс по сохранению текущей сцены.
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
