using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    //Load the Main Scene when press start
    public void OnPlayButton()
    {
        SceneManager.LoadScene(1);
    }

    //Load the Start Menu when press main menu
    public void OnStartMenuButton()
    {
        SceneManager.LoadScene(0);
    }

    //quit the application
    public void OnQuitButton()
    {
        Application.Quit();
    }
}
