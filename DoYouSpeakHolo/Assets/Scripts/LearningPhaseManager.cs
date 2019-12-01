using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LearningPhaseManager : MonoBehaviour {

    private List<string> SceneObjects;
    private ObjectPooler Pooler;

    public enum ScenesEnum { Scene1, Scene2, Scene3 };
    public ScenesEnum Scene;
    Vector3 CentralPosition;

    void Start() {
        Setup();
    }

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


        CentralPosition = new Vector3(0, 0, 2);
        EventManager.StartListening("LearningPhaseStart", HandleStartOfLearningPhase);
        EventManager.StartListening("LearningPhaseSingleSpawn", HandleSpawn);
        EventManager.StartListening("LearningPhasePairSpawn", HandleSpawnPairs);
    }

    //First phase of the activity, the virtual assistant shows to the user some objects and tells their name
    private void HandleStartOfLearningPhase() {
        Debug.Log("Start Learning Phase");
        //Trigger the spawn procedure
        EventManager.TriggerEvent("LearningPhaseSingleSpawn");
    }

    internal void SetScene(ScenesEnum scene) {
        Scene = scene;
    }

    //Handler fot the spawn procedure
    private void HandleSpawn() {
        StartCoroutine(ShowObjects());
    }

    //Spawn the objects
    IEnumerator ShowObjects() {
        foreach (string objectKey in SceneObjects) {
            Debug.Log("Spawning " + objectKey);
            StartCoroutine(ShowObject(objectKey));
            yield return new WaitForSeconds(3);
        }

        //Trigger the spawning of the object pairs
        EventManager.TriggerEvent("LearningPhasePairSpawn");
    }

    //Spawn the objects in front of the user and destroy them after a timeout
    IEnumerator ShowObject(string objKey) {
        //TODO: Add VA voice
        GameObject objectToCreate = Pooler.ActivateObject(objKey, CentralPosition);
        yield return new WaitForSeconds(2);
        Pooler.DeactivateObject(objKey);
    }

    void HandleSpawnPairs() {
        
        //Spawn the male character

        //Spawn the female character
        
        //End of the learning phase
        End();
    }


    //Stop listening to events and trigger the checking phase
    private void End() {
        EventManager.StopListening("LearningPhaseStart", HandleStartOfLearningPhase);
        EventManager.StopListening("LearningPhaseSpawn", HandleSpawn);

        //start the checking phase
        //TODO: find a better way to call the method
        EventManager.TriggerEvent("CheckingPhase");
    }
}
