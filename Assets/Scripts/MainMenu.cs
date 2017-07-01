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

    public void LoadSingle()
    {
        SceneManager.LoadScene(single);
    }
    public void LoadMulti()
    {
        SceneManager.LoadScene(multi);
    }
    public void TwoPlayers()
    {
        SceneManager.LoadScene(twoPlayers);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
