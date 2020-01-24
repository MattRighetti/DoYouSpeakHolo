using UnityEngine;
using UnityEngine.SceneManagement;
#if WINDOWS_UWP
using Windows.Storage;
using System.Threading.Tasks;
using System;
#endif

//  Method wrapper for loading levelss
public class SceneSelector: MonoBehaviour {

    public void GoToMenuScene() {
        SceneManager.LoadScene("Menu");
    }

    public void GoToScene1() {
        SceneManager.LoadScene("Scene1");
    }

    public void GoToScene2() {
        SceneManager.LoadScene("Scene2");
    }

    public void GoToScene3() {
        SceneManager.LoadScene("Scene3");
    }

    public void CloseGame() {
        Application.Quit();
    }
}