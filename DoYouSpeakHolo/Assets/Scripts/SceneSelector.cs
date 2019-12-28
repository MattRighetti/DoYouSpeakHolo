using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
#if WINDOWS_UWP
using Windows.Storage;
using System.Threading.Tasks;
using System;
#endif

public class SceneSelector: MonoBehaviour {
    public static SceneSettings settings;

    void Start() {
        //tryMethod();
        //settings = ReadJSONFromFile();
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
		            StorageFile jsonFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx://objects.json"));
                    string jsonText = await FileIO.ReadTextAsync(jsonFile);
                    //Debug.Log(jsonText);
                    deserializedObject = JsonUtility.FromJson<SceneSettings>(jsonText);
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