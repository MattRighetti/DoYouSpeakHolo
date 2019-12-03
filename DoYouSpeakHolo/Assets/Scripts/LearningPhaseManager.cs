using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EventManager;

public class LearningPhaseManager : MonoBehaviour {

    private List<string> SceneObjects;
    private ObjectPooler Pooler;

    public enum ScenesEnum { Scene1, Scene2, Scene3 };
    public ScenesEnum Scene;

    void Start() {
        Setup();
    }

    // - Get the Pooler instance
    // - Set the current scene and select the right category of objects
    // - Start listening to the events
    void Setup() {
        //Get the ObjectPooler instance
        Pooler = ObjectPooler.SharedInstance;

        //Get the objects from the pooler depending on the scene
        switch (Scene) {
            case ScenesEnum.Scene1:
                SceneObjects = Pooler.GetObjectsByCategory(ObjectPooler.Animals);
                break;
            case ScenesEnum.Scene2:
                SceneObjects = Pooler.GetObjectsByCategory(ObjectPooler.Animals);
                break;
            case ScenesEnum.Scene3:
                SceneObjects = Pooler.GetObjectsByCategory(ObjectPooler.Fruits);
                break;
        }

        EventManager.StartListening(Triggers.LearningPhaseStart, HandleStartOfLearningPhase);
        EventManager.StartListening(Triggers.LearningPhaseSingleSpawn, HandleSpawn);
        EventManager.StartListening(Triggers.LearningPhasePairSpawn, HandleSpawnPairs);
    }

    //First phase of the activity, the virtual assistant shows to the user some objects and tells their name
    private void HandleStartOfLearningPhase() {
        //Trigger the spawn procedure
        EventManager.TriggerEvent(Triggers.LearningPhaseSingleSpawn);
    }

    internal void SetScene(ScenesEnum scene) {
        Scene = scene;
    }

    //Handler fot the spawn procedure
    private void HandleSpawn() {
        StartCoroutine(ShowObjects());

        //Trigger the spawning of the object pairs
        EventManager.TriggerEvent(Triggers.LearningPhasePairSpawn);
    }

    //Spawn the objects
    IEnumerator ShowObjects() {
        foreach (string objectKey in SceneObjects) {
            StartCoroutine(ShowObject(objectKey));
            yield return new WaitForSeconds(3);
        }
    }

    //Spawn the objects in front of the user and destroy them after a timeout
    IEnumerator ShowObject(string objKey) {
        //TODO: Add VA voice
        GameObject objectToCreate = Pooler.ActivateObject(objKey, Positions.Central);
        yield return new WaitForSeconds(2);
        Pooler.DeactivateObject(objKey);
    }

    void HandleSpawnPairs() {
        if (Scene == ScenesEnum.Scene3) {
            //Spawn the male character

            //Spawn the female character 
        }



        //End of the learning phase
        End();
    }


    //Stop listening to events and trigger the checking phase
    private void End() {
        EventManager.StopListening(Triggers.LearningPhaseStart, HandleStartOfLearningPhase);
        EventManager.StopListening(Triggers.LearningPhaseSingleSpawn, HandleSpawn);
        EventManager.StopListening(Triggers.LearningPhasePairSpawn, HandleSpawn);

        //start the checking phase
        //TODO: find a better way to call the method
        EventManager.TriggerEvent(Triggers.CheckingPhase);
    }
}
