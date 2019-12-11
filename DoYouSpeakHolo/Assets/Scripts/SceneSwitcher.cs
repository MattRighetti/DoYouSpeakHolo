using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour {
    public static List<SceneObjectsToLoad> settings;

    void Start() {
        if (settings == null) {
            tryMethod();
            //settings = ReadJSONFromFile();
        }
    }

    private void tryMethod() {
        SceneObjectsToLoad scene1 = new SceneObjectsToLoad();
        SceneObjectsToLoad scene2 = new SceneObjectsToLoad();
        SceneObjectsToLoad scene3 = new SceneObjectsToLoad();
        //settings = ReadJSONFromFile();
        scene3.staticObjects.Add("House", "Prefab/objects/House_right");
        scene3.staticObjects.Add("Tree", "Prefab/objects/Tree_right");
        scene3.staticObjects.Add("MaleBasket", "Prefab/objects/Basket");
        scene3.staticObjects.Add("FemaleBasket", "Prefab/objects/Basket");
        scene3.staticObjects.Add("Male", "Prefab/people/VA_MaleCorrect");
        scene3.staticObjects.Add("VA", "Prefab/people/BabyGroot");
        scene3.staticObjects.Add("Female", "Prefab/people/VA_FemaleCorrect");
        scene3.dynamicObjects.Add("Apple", "Prefab/Fruits/Apple");
        scene3.dynamicObjects.Add("Banana", "Prefab/Fruits/Banana");
        scene3.dynamicObjects.Add("Orange", "Prefab/Fruits/Orange");
        scene3.dynamicObjects.Add("Pear", "Prefab/Fruits/Pear");
        settings = new List<SceneObjectsToLoad>();
        settings.Add(scene1);
        settings.Add(scene2);
        settings.Add(scene3);
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
    public List<SceneObjectsToLoad> ReadJSONFromFile() {
        using (StreamReader file = new StreamReader("Assets/Resources/Prefab/objects.json")) {
            string json = file.ReadToEnd();
            return JsonConvert.DeserializeObject<List<SceneObjectsToLoad>>(json);
        }
    }
}

public class SceneObjectsToLoad {
    public Dictionary<string, string> staticObjects;
    public Dictionary<string, string> dynamicObjects;

    public SceneObjectsToLoad() {
        staticObjects = new Dictionary<string, string>();
        dynamicObjects = new Dictionary<string, string>();
    }
}
