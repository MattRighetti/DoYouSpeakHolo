using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EventManager;

public abstract class LearningPhaseManager : MonoBehaviour {

    public List<string> SceneObjects;
    protected AbstractSceneManager sceneManager;

    void Start() {
        Setup();
    }

    // - Get the Pooler instance
    // - Set the current scene and select the right category of objects
    // - Start listening to the events
    void Setup() {
        //Get the ObjectPooler instance
        sceneManager = GetComponent<AbstractSceneManager>();
        SceneObjects = sceneManager.GetObjects();
        
        StartListening(Triggers.LearningPhaseStart, HandleStartOfLearningPhase);
        StartListening(Triggers.LearningPhaseSpawn, StartSpawn);
    }

    //First phase of the activity, the virtual assistant shows to the user some objects and tells their name
    private void HandleStartOfLearningPhase() {
        //TODO: Add VA speaking

        //Trigger the spawn procedure
        TriggerEvent(Triggers.LearningPhaseSpawn);
    }

    //Handler fot the starting the spawn procedure
    protected abstract void StartSpawn();

    //Spawn the objects one at time
    protected IEnumerator ShowObjects(List<string> objectsToShow) {
        foreach (string objectKey in objectsToShow) {
            StartCoroutine(ShowObject(objectKey));
            yield return new WaitForSeconds(3);
        }

        //End of Learning Phase
        End();
    }

    //Spawn the objects in front of the user and destroy them after a timeout
    protected IEnumerator ShowObject(string objKey) {
        //TODO: Add VA voice
        GameObject objectToCreate = sceneManager.ActivateObject(objKey, Positions.Central);
        yield return new WaitForSeconds(2);
        sceneManager.DeactivateObject(objKey);
    }


    //Stop listening to events and trigger the checking phase
    protected void End() {
        StopListening(Triggers.LearningPhaseStart, HandleStartOfLearningPhase);
        StopListening(Triggers.LearningPhaseSpawn, StartSpawn);

        //start the checking phase
        //TODO: find a better way to call the method
        TriggerEvent(Triggers.CheckingPhase);
    }
}
