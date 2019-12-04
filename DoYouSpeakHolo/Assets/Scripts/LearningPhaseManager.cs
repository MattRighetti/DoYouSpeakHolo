using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EventManager;

public class LearningPhaseManager : MonoBehaviour {

    public List<string> SceneObjects;
    private ObjectPooler Pooler;

    void Start() {
        Setup();
    }

    // - Get the Pooler instance
    // - Set the current scene and select the right category of objects
    // - Start listening to the events
    void Setup() {
        //Get the ObjectPooler instance
        Pooler = ObjectPooler.GetPooler();

        SceneObjects = Pooler.GetObjects();
        

        StartListening(Triggers.LearningPhaseStart, HandleStartOfLearningPhase);
        StartListening(Triggers.LearningPhaseSingleSpawn, HandleSpawn);
        StartListening(Triggers.LearningPhasePairSpawn, HandleSpawnPairs);
    }

    //First phase of the activity, the virtual assistant shows to the user some objects and tells their name
    private void HandleStartOfLearningPhase() {
        //Trigger the spawn procedure
        TriggerEvent(Triggers.LearningPhaseSingleSpawn);
    }
    
    //Handler fot the single object spawn procedure
    void HandleSpawn() {
        StartCoroutine(ShowObjects());
    }

    //Spawn the objects one at time
    IEnumerator ShowObjects() {
        foreach (string objectKey in SceneObjects) {
            StartCoroutine(ShowObject(objectKey));
            yield return new WaitForSeconds(3);
        }

        //End of Learning Phase
        End();
    }

    //Spawn the objects in front of the user and destroy them after a timeout
    IEnumerator ShowObject(string objKey) {
        //TODO: Add VA voice
        GameObject objectToCreate = Pooler.ActivateObject(objKey, Positions.Central);
        yield return new WaitForSeconds(2);
        Pooler.DeactivateObject(objKey);
    }

    //Spawn objects
    void HandleSpawnPairs() {
        return;
    }


    //Stop listening to events and trigger the checking phase
    private void End() {
        StopListening(Triggers.LearningPhaseStart, HandleStartOfLearningPhase);
        StopListening(Triggers.LearningPhaseSingleSpawn, HandleSpawn);
        StopListening(Triggers.LearningPhasePairSpawn, HandleSpawn);

        //start the checking phase
        //TODO: find a better way to call the method
        TriggerEvent(Triggers.CheckingPhase);
    }
}
