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
        SetSceneAndCreateObjects(ObjectPooler.ScenesEnum.Scene1);
        SceneManager.LoadScene("Scene1");
    }

    public void GoToScene2()
    {
        SetSceneAndCreateObjects(ObjectPooler.ScenesEnum.Scene2);
        SceneManager.LoadScene("Scene1_bis");
    }

    public void GoToScene3()
    {
        SetSceneAndCreateObjects(ObjectPooler.ScenesEnum.Scene3);
        SceneManager.LoadScene("Scene3");
    }

    private void SetSceneAndCreateObjects(ObjectPooler.ScenesEnum scene) {
        GameObject obj = GameObject.Find("Pooler");
        ObjectPooler pooler = obj.GetComponent<ObjectPooler>();
        pooler.Scene = scene;
        DontDestroyOnLoad(pooler);
    }

    public void CloseGame()
    {
        Application.Quit();
    }
}
