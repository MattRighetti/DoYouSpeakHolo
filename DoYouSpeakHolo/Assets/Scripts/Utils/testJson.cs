using System.Collections.Generic;
using System.IO;
using UnityEngine;
#if WINDOWS_UWP
using Windows.Storage;
using System.Threading.Tasks;
using System;
#endif

[System.Serializable]
public class prova1 {
    public string type;
    public string path;
}

[System.Serializable]
public class prova2 {
    public List<prova1> staticObjects;
    public List<prova1> dynamicObjects;
}

[System.Serializable]
public class prova3 {
    public List<prova2> scenes;
}


public class testJson : MonoBehaviour
{
    prova3 deserializedObject;
    // Start is called before the first frame update
    void Start()
    {

#if UNITY_EDITOR && UNITY_METRO
        string path = "Assets/Resources/Prefab/prova.json";
        string text = File.ReadAllText(path);
        deserializedObject = JsonUtility.FromJson<prova3>(text);
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

        Debug.Log("deserialized object" + deserializedObject);
        Debug.Log(deserializedObject.scenes[0].dynamicObjects[0].type);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
