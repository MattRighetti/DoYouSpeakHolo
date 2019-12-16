
using System.Collections.Generic;
using UnityEngine;
using static EventManager;

public class CandSManager : AbstractSceneManager {

    private readonly SceneObjectsToLoad sceneObjects = SceneSwitcher.settings.scenes[0];
    

    public List<string> TargetObjects { get; set; }

    private int findObjectCounter = 0;

    //Set the audio context to scene 1
    public override void SetAudioContext() {
        AudioContext = new AudioContext1();
    }

    //Create the scene objects
    public override void LoadObjects() {
        Pooler.CreateStaticObjects(sceneObjects.staticObjects);
        Pooler.CreateDynamicObjects(sceneObjects.dynamicObjects);
        TargetObjects = Shuffle(Pooler.GetDynamicObjects());
    }

    private void FoundObject() {
        //increase the counter
        findObjectCounter++;
        if (findObjectCounter == TargetObjects.Count - 1) {
            //Objects are finished
        }
        else {
            SetTargetObject();
        }
    }

    //Set the object that the user has to find
    private void SetTargetObject() {
        //VA tells the User which object he needs to find to complete the task
        Debug.Log("Object to find " + TargetObjects[findObjectCounter]);

        //Set the current target
        GetPooledObject(TargetObjects[findObjectCounter]).GetComponent<FindObjectTask>().IsTarget = true;
    }

    public override void StartListeningToCustomEvents() {
        StartListening(Triggers.FoundObject, FoundObject);
        StartListening(Triggers.SetTargetObject, SetTargetObject);
    }

    public override void StopListeningToCustomEvents() {
        StopListening(Triggers.FoundObject, FoundObject);
        StopListening(Triggers.SetTargetObject, SetTargetObject);
    }
}

