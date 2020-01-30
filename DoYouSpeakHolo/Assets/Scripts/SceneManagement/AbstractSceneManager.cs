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

//  Abstract class responsible of the entire SceneManagement
public abstract class AbstractSceneManager : MonoBehaviour {

    //  Pooler instance
    protected ObjectPooler Pooler;

    //  Scene objects information
    protected SceneObjectsToLoad sceneObjects;

    //  Contains the list of objects's name and path to load into the scene
    public SceneObjectsToLoad sceneSettings;
    public VirtualAssistantManager VirtualAssistant;
    public LearningPhaseManager LearningPhaseManager { get; set; }
    public CheckingPhaseManager CheckingPhaseManager { get; set; }

    //  Class for audio search
    public AudioContext AudioContext { get; set; }

    //  Contains the list of objects's name and path of all the scenes
    protected SceneSettings settings;

    public void ConfigureScene() {
        //  Read the objects information JSON file
        ParseJson();
        Pooler = ObjectPooler.GetPooler();
        //Select the objects to load
        int scene = GameObject.Find("SceneSelected").GetComponent<SceneSelected>().Scene;
        sceneObjects = settings.scenes[scene];

        if (scene == 0)
            Pooler.FindTable();
        else
            Pooler.FindFloor();

        
        //  Find the floor position with respect to the user gaze
        Pooler.FindFloor();

        //  Instantiate all the scene objects
        LoadObjects();
        //  Set the current AudioContext according to the scene
        SetAudioContext();
        //  Start listening to the basic events that guarantee the correct activity flow
        StartListening();

        //  Activate the Virtual Assistant and set it up
        VirtualAssistant = ActivateObject("VA", Positions.VAPosition, Positions.ObjectsRotation).GetComponent<VirtualAssistantManager>();
        VirtualAssistant.Setup();
    }


    //  The VA introduces the activity
    //  Triggers the method AnimateAvatar.PlayIntroduction
    public void StartIntroduction() {
        VirtualAssistant.PlayIntroduction(AudioContext);
    }

    //  Start the Learning phase
    protected void StartLearningPhase() {
        LearningPhaseManager.StartLearningPhase();
    }

    //  Start the Checking Phase
    protected void StartCheckingPhase() {
        CheckingPhaseManager.StartCheckingPhase();
    }

    //  The Virtual Assitant introduces the Checking Phase of the Activity
    public IEnumerator IntroduceCheckingPhase() {
        yield return VirtualAssistant.PlayCheckingPhaseIntroduction(AudioContext);
    }

    //  End of the activity
    protected void EndActivity() {
        //  Stop listening to the basic events
        StopListening();

        //  Go to the main menu
        SceneManager.LoadScene("Menu");

    }

    //  The Virtual Assistant introduces an object without the scene context
    //  e.g. pear -> audio: "This is a pear"
    public IEnumerator IntroduceObject(string objectToIntroduce) {
        yield return VirtualAssistant.IntroduceObject(AudioContext, objectToIntroduce);
    }

    //  The Virtual Assistant introduces an object wit the scene context
    //  e.g. pear with the possessives context -> audio: "This is his/her pear"
    public IEnumerator IntroduceObjectWithContext(string objectToIntroduce) {
        yield return VirtualAssistant.IntroduceObjectWithContext(AudioContext, objectToIntroduce);
    }

    //  Activate a GameObject in a specified position
    public GameObject ActivateObject(string key, Vector3 position, Quaternion rotation) {
        return Pooler.ActivateObject(key, position, rotation);
    }

    //  Deactivate a GameObject
    public void DeactivateObject(string key) {
        Pooler.DeactivateObject(key);
    }

    public GameObject GetPooledObject(string key) {
        return Pooler.GetPooledObject(key);
    }

    public List<string> GetObjects() {
        return Pooler.GetDynamicObjects();
    }


    //  Start listening to the basic events to guarantee the correct flow of the activity:
    //      - Virtual Assistant introduction end
    //      - Learning Phase end
    //      - Checking Phase end
    private void StartListening() {
        EventManager.StartListening(EventManager.Triggers.VAIntroductionEnd, StartLearningPhase);
        EventManager.StartListening(EventManager.Triggers.LearningPhaseEnd, StartCheckingPhase);
        EventManager.StartListening(EventManager.Triggers.CheckingPhaseEnd, EndActivity);
        StartListeningToCustomEvents();
    }

    //  Stop listening to the basic events
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

    //  Load the objects of the current scene
    public abstract void LoadObjects();

    //  Set AudioContext of the current scene
    public abstract void SetAudioContext();

    //  Start listening to some particular events of a scene
    public abstract void StartListeningToCustomEvents();

    //  Stop listening 
    public abstract void StopListeningToCustomEvents();

    // ------------------------- OBJECTS LOAD -----------------------------

    //  Reads from file the objects of all the scenes
    private void ParseJson() {
#if UNITY_EDITOR
        string path = "Assets/Resources/Prefab/objects.json";
        string text = File.ReadAllText(path);
        Debug.Log("Text: " + text);
        settings = JsonUtility.FromJson<SceneSettings>(text);
        if (settings == null)
            Debug.Log("Non sono riuscito a leggere il JSON");
#endif

#if WINDOWS_UWP
        Task<Task> task = new Task<Task>(async () =>
        {
            try {
                StorageFile jsonFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///objects.json"));
                string jsonText = await FileIO.ReadTextAsync(jsonFile);
                Debug.Log(jsonText);
                settings = JsonUtility.FromJson<SceneSettings>(jsonText);
            }
            catch (Exception e) {
                Debug.Log(e);
            }
        });
        task.Start();
        task.Wait();
        task.Result.Wait();
#endif
    }
}




//  Wrapper of object's information:
//      - type: GameObject name
//      - path: relative path starting from the Resources folder
[System.Serializable]
public class SingleObjectToLoad {
    public string type;
    public string path;
}

//  Wrapper of scene's objects:
//      - staticObjects: background objects
//      - dynamicObjects: active objects in the scene
[System.Serializable]
public class SceneObjectsToLoad {
    public List<SingleObjectToLoad> staticObjects;
    public List<SingleObjectToLoad> dynamicObjects;
}

//  Wrapper of the scenes configurations
[System.Serializable]
public class SceneSettings {
    public List<SceneObjectsToLoad> scenes;
}

