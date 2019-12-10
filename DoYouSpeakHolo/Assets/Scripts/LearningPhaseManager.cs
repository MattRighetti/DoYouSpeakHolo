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

    private void Setup() {
        sceneManager = GetComponent<AbstractSceneManager>();
        sceneManager.LearningPhaseManager = this;
        SceneObjects = sceneManager.GetObjects();
    }

    //First phase of the activity, the virtual assistant shows to the user some objects and tells their name
    public void StartLearningPhase() {
        LearningPhase();
    }

    //Spawn the objects one at time
    protected IEnumerator ShowObjects(List<string> objectsToShow) {
        foreach (string objectKey in objectsToShow) {
            yield return StartCoroutine(ShowObject(objectKey));
        }
    }

    //Spawn the objects in front of the user and destroy them after a timeout
    protected IEnumerator ShowObject(string objKey) { 
        //Activate the object
        GameObject objectToCreate = sceneManager.ActivateObject(objKey, Positions.Central);
        
        //The VA introduces the object
      //  sceneManager.IntroduceObject(objKey);
        
        //Wait until the end of the introduction
        yield return new WaitForSeconds(2);
        
        //Deactivate the object
        sceneManager.DeactivateObject(objKey);
    }


    //Stop listening to events and trigger the checking phase
    protected void End() {
        //start the checking phase
        TriggerEvent(Triggers.LearningPhaseEnd);
    }

    // ---------------------------------- ABSTRACT ------------------------------
    //Handler fot the starting the spawn procedure
    protected abstract void LearningPhase();
}
