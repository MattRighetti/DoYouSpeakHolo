using Microsoft.MixedReality.Toolkit.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static EventManager;

public class CheckingPhaseManager : MonoBehaviour {

    private List<string> gameObjects;
    private ObjectPooler objectPooler;
    private int findObjectCounter = -1;

    void Start() {
        objectPooler = gameObject.GetComponent<ObjectPooler>();
        gameObjects = gameObject.GetComponent<LearningPhaseManager>().SceneObjects;
        StartListening(Triggers.CheckingPhase, HandleStartCheckingPhase);
        StartListening(Triggers.FoundObject, FoundObject);
    }



    private void Update() {


    }

    private void HandleStartCheckingPhase() {
        CheckingPhase();
    }

    //Spawn the objects in random order and ask the user to pick a specific one
    private void CheckingPhase() {
        CreateAllObjectsAndDisplayInRandomOrder(gameObjects);
        SetTargetObject();
    }

   
    private void FoundObject() {
        throw new NotImplementedException();
    }

    //Set the object that the user has to find
    private void SetTargetObject() {
        //increase the counter
        findObjectCounter++;

        //shuffle the collection again
        gameObjects = Shuffle(gameObjects);

        //set the current target
        objectPooler.GetPooledObject(gameObjects[findObjectCounter]).GetComponent<FindObjectTask>().IsTarget = true;
    }

    private void CreateAllObjectsAndDisplayInRandomOrder(List<string> gameObjects) {

        //Shuffle the collection
        gameObjects = Shuffle(gameObjects);
        GameObject gameObj;
        //Define initial spawning position
        Vector3 startPosition = Positions.startPositionInlineFour;
        foreach (string obj in gameObjects) {
            //Activate the object and attach to it the script for the task
            gameObj = objectPooler.ActivateObject(obj, startPosition);
            FindObjectTask task = gameObj.AddComponent<FindObjectTask>();
            gameObj.AddComponent<Interactable>().AddReceiver<InteractableOnPressReceiver>().OnPress.AddListener(() => task.Check() );
            startPosition += new Vector3(0.5f, 0, 0);
        }
    }

    //Stop listening to events and trigger the new phase
    private void End() {
        StopListening(Triggers.CheckingPhase, HandleStartCheckingPhase);

        //Trigger the new phase

    }

    //Randomize a List
    public List<string> Shuffle(List<string> list) {
        List<string> randomList = new List<string>();

        for (int i = 0; i < gameObjects.Count; i++) {
            System.Random rnd = new System.Random();
            int index = rnd.Next(gameObjects.Count);
            randomList.Add(gameObjects[i]);
        }
        return randomList;
    }
}
