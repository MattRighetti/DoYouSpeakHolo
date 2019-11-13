using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{

    public void GoToMenuScene()
    {
        SceneManager.LoadScene("Menu");
    }

    public void GoToScene1()
    {
        SceneManager.LoadScene("Scene1");
    }

    public void GoToScene2()
    {
        SceneManager.LoadScene("Scene2");
    }

    public void GoToScene3()
    {
        SceneManager.LoadScene("Scene3");
    }

    public void CloseGame()
    {
        Application.Quit();
    }
}
