using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
#if WINDOWS_UWP
using Windows.Storage;
using System.Threading.Tasks;
using System;
#endif

public abstract class AbstractSceneManager : MonoBehaviour {

    protected ObjectPooler Pooler;
    public SceneObjectsToLoad sceneSettings;
    public AnimateAvatar VirtualAssistant;
    public LearningPhaseManager LearningPhaseManager { get; set; }
    public CheckingPhaseManager CheckingPhaseManager { get; set; }
    public AudioContext AudioContext { get; set; }
    protected SceneSettings settings;

    public void ConfigureScene() {
        ParseJson();
        //Debug.Log("obj" + settings.scenes[0].dynamicObjects[0].type);
        Pooler = ObjectPooler.GetPooler();
        Pooler.FindFloor();
        LoadObjects();
        SetAudioContext();
        StartListening();
        VirtualAssistant = ActivateObject("VA", Positions.VAPosition).GetComponent<AnimateAvatar>();
        VirtualAssistant.Setup();
    }

    //The VA introduces the activity
    //Triggers the method AnimateAvatar.PlayIntroduction
    public void StartIntroduction() {
        VirtualAssistant.PlayIntroduction();
    }

    //Start the Learning phase
    protected void StartLearningPhase() {
        LearningPhaseManager.StartLearningPhase();
    }

    //Start the Checking Phase
    protected void StartCheckingPhase() {
        CheckingPhaseManager.StartCheckingPhase();
    }

    protected void EndActivity() {
        //TODO after the beta
        //Check if there are more levels to load;

        StopListening();

        //For the demo load the menu
        SceneManager.LoadScene("Menu");

    }

    internal IEnumerator IntroduceObject(string objectToIntroduce) {
        yield return VirtualAssistant.IntroduceObject(AudioContext, objectToIntroduce);
    }

    internal IEnumerator IntroduceObjectWithContext(string objectToIntroduce) {
        yield return VirtualAssistant.IntroduceObjectWithContext(AudioContext, objectToIntroduce);
    }

    public GameObject ActivateObject(string key, Vector3 position) {
        return Pooler.ActivateObject(key, position);
    }

    public void DeactivateObject(string key) {
        Pooler.DeactivateObject(key);
    }

    public GameObject GetPooledObject(string key) {
        return Pooler.GetPooledObject(key);
    }

    public List<string> GetObjects() {
        return Pooler.GetDynamicObjects();
    }

    private void StartListening() {
        EventManager.StartListening(EventManager.Triggers.VAIntroductionEnd, StartLearningPhase);
        EventManager.StartListening(EventManager.Triggers.LearningPhaseEnd, StartCheckingPhase);
        EventManager.StartListening(EventManager.Triggers.CheckingPhaseEnd, EndActivity);
        StartListeningToCustomEvents();
    }

    private void StopListening() {
        EventManager.StopListening(EventManager.Triggers.VAIntroductionEnd, StartLearningPhase);
        EventManager.StopListening(EventManager.Triggers.LearningPhaseEnd, StartCheckingPhase);
        EventManager.StopListening(EventManager.Triggers.CheckingPhaseEnd, EndActivity);
        StopListeningToCustomEvents();
    }

    //Randomize a List
    public static List<string> Shuffle(List<string> list) {
        List<string> randomList = new List<string>();

        System.Random random = new System.Random();
        int randomIndex = 0;

        while (list.Count > 0) {
            randomIndex = random.Next(0, list.Count);
            randomList.Add(list[randomIndex]);
            list.RemoveAt(randomIndex);
        }

        return randomList;
    }

    // -------------------------- ABSTRACT --------------------------------

    public abstract void LoadObjects();

    public abstract void SetAudioContext();

    public abstract void StartListeningToCustomEvents();
    
    public abstract void StopListeningToCustomEvents();

    // ------------------------- OBJECTS LOAD -----------------------------

    private void ParseJson() {
        string path = "Assets/Resources/Prefab/objects.json";
        string text = File.ReadAllText(path);
        Debug.Log("Text: " + text);
        settings = JsonUtility.FromJson<SceneSettings>(text);
#if UNITY_EDITOR && UNITY_METRO
        string path = "Assets/Resources/Prefab/objects.json";
        string text = File.ReadAllText(path);
        Debug.Log("Text: " + text);
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
}

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
