using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
#if WINDOWS_UWP
using Windows.Storage;
using System.Threading.Tasks;
using System;
#endif

[System.Serializable]
public class SingleObjectToLoad {
    public string type;
    public string path;
}

[System.Serializable]
public class SceneObjectsToLoad {
    public List<SingleObjectToLoad> staticObjects;
    public List<SingleObjectToLoad> dynamicObjects;
}

[System.Serializable]
public class SceneSettings {
    public List<SceneObjectsToLoad> scenes;
}

public class SceneSwitcher : MonoBehaviour {
    public static SceneSettings settings;

    void Start() {
        if (settings == null) {
            tryMethod();
            //settings = ReadJSONFromFile();
        }
    }

    private void tryMethod() {
#if UNITY_EDITOR && UNITY_METRO
        string path = "Assets/Resources/Prefab/objects.json";
        string text = File.ReadAllText(path);
        settings = JsonUtility.FromJson<SceneSettings>(text);
#endif

#if WINDOWS_UWP
        Task<Task> task = new Task<Task>(async () =>
            {
                try
                {
		            StorageFile jsonFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///prova.json"));
                    string jsonText = await FileIO.ReadTextAsync(jsonFile);
                    //Debug.Log(jsonText);
                    deserializedObject = JsonUtility.FromJson<prova3>(jsonText);
                }
                catch (Exception e)
                {
                    Debug.Log(e);
                }
            });
        task.Start();
        task.Wait();
        task.Result.Wait();
#endif
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
            return JsonUtility.FromJson<List<SceneObjectsToLoad>>(json);
        }
    }
}