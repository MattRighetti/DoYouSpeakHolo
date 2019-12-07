using Microsoft.MixedReality.Toolkit.UI;
using System;
using UnityEngine;
using static EventManager;

class CheckingPhaseActivity1 : CheckingPhaseManager {

    private int findObjectCounter = 0;

    void Start() {
        base.Start();
        StartListening(Triggers.FoundObject, FoundObject);
    }

    //Spawn the objects in random order and ask the user to pick a specific one
    protected override void CheckingPhase() {
        Debug.Log("Entered in CheckingPhase");
        CreateAllObjectsAndDisplayInRandomOrder();
        //Shuffle the collection again
        SceneObjects = Shuffle(SceneObjects);
        SetTargetObject();
    }

    private void FoundObject() {
        //increase the counter
        findObjectCounter++;
        if (findObjectCounter == SceneObjects.Count - 1) {
            //Objects are finished
        }
        else {
            SetTargetObject();
        }
    }

    //Set the object that the user has to find
    private void SetTargetObject() {
        //VA tells the User which object he needs to find to complete the task
        Debug.Log("Object to find " + SceneObjects[findObjectCounter]);

        //Set the current target
        GetComponent<AbstractSceneManager>().GetPooledObject(SceneObjects[findObjectCounter]).GetComponent<FindObjectTask>().IsTarget = true;
    }

    protected override void CreateAllObjectsAndDisplayInRandomOrder() {
        //Shuffle the collection
        SceneObjects = Shuffle(SceneObjects);
        GameObject gameObj;
        //Define initial spawning position
        Vector3 startPosition = Positions.startPositionInlineFour;
        foreach (string obj in SceneObjects) {
            //Activate the object and attach to it the script for the task
            gameObj = GetComponent<AbstractSceneManager>().ActivateObject(obj, startPosition);
            FindObjectTask task = gameObj.AddComponent<FindObjectTask>();
            gameObj.AddComponent<Interactable>().AddReceiver<InteractableOnPressReceiver>().OnPress.AddListener(() => task.Check());
            startPosition += new Vector3(0.5f, 0, 0);
        }
    }
}

