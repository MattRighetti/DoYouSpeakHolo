using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour {
    public static List<SceneSettings> settings;

    void Start() {
        if (settings == null)
            settings = ReadJSONFromFile();
    }

    public void GoToMenuScene() {
        SceneManager.LoadScene("Menu");
    }

    public void GoToScene1() {
        SceneManager.LoadScene("Scene1");
    }

    public void GoToScene2() {
        SceneManager.LoadScene("Scene1_bis");
    }

    public void GoToScene3() {
        SceneManager.LoadScene("Scene3");
    }

    public void CloseGame() {
        Application.Quit();
    }

    //Read and parse the JSON file
    public List<SceneSettings> ReadJSONFromFile() {
        using (StreamReader file = new StreamReader("Assets/Resources/Prefab/objects.json")) {
            string json = file.ReadToEnd();
            return JsonConvert.DeserializeObject<List<SceneSettings>>(json);
        }
    }

}

public class SceneSettings {
    public Dictionary<string, string> staticObjects;
    public Dictionary<string, string> dynamicObjects;
}
