using UnityEngine;
using UnityEngine.SceneManagement;

//  Method wrapper for loading levelss
public class SceneSelector: MonoBehaviour {
    //Object containing the level selection. Used to load the correct objects
    SceneSelected sceneSelected; 

    void Start() {
        sceneSelected = GameObject.Find("SceneSelected").GetComponent<SceneSelected>();
        DontDestroyOnLoad(sceneSelected.gameObject);
    }

    public void GoToMenuScene() {
        SceneManager.LoadScene("Menu");
    }

    public void GoToScene1() {
        sceneSelected.Scene = 0;
        SceneManager.LoadScene("Scene1");
    }

    public void GoToScene2() {
        sceneSelected.Scene = 1;
        SceneManager.LoadScene("Scene2");
    }

    public void GoToScene3SingularPossessives() {
        sceneSelected.Scene = 2;
        SceneManager.LoadScene("Scene3");
    }

    public void GoToScene3PluralPossessives() {
        sceneSelected.Scene = 3;
        SceneManager.LoadScene("Scene3");
    }

    public void CloseGame() {
        Application.Quit();
    }
}

