using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EventManager;

//  Abstract class responsible of the Learning Phase
public abstract class LearningPhaseManager : MonoBehaviour {

    public List<string> SceneObjects;
    protected AbstractSceneManager sceneManager;

    public void Setup() {
        sceneManager = GetComponent<AbstractSceneManager>();
        sceneManager.LearningPhaseManager = this;
        SceneObjects = sceneManager.GetObjects();
    }

    //  First phase of the activity, the virtual assistant shows to the user some objects and tells their name
    public void StartLearningPhase() {
        LearningPhase();
    }

    //  Spawn the objects one at time playing the corresponding audio without the scene context
    protected IEnumerator ShowObjects(List<string> objectsToShow) {
        foreach (string objectKey in objectsToShow) {
            yield return StartCoroutine(ShowObject(objectKey));
        }
    }

    //  Spawn the objects in front of the user and destroy them after a timeout 
    //  playing the corresponding audio without the scene context
    protected IEnumerator ShowObject(string objKey) {
        //  Activate the object
        GameObject objectToCreate = sceneManager.ActivateObject(objKey, Positions.Central, Positions.ObjectsRotation);


        //  The VA introduces the object
        //  Wait until the end of the introduction
        yield return sceneManager.IntroduceObject(objectToCreate.name);

        //  Deactivate the object
        sceneManager.DeactivateObject(objKey);
    }

    //  Spawn the objects one at time playing the corresponding audio with the scene context
    protected IEnumerator ShowObjectsWithContext(List<string> objectsToShow) {
        foreach (string objectKey in objectsToShow) {
            yield return StartCoroutine(ShowObjectWithContext(objectKey));
        }
    }

    //  Spawn the objects in front of the user and destroy them after a timeout 
    //  playing the corresponding audio with the scene context
    protected IEnumerator ShowObjectWithContext(string objKey) {
        //  Activate the object
        GameObject objectToCreate = sceneManager.ActivateObject(objKey, Positions.Central, Positions.ObjectsRotation);


        //  The VA introduces the object
        //  Wait until the end of the introduction
        yield return sceneManager.IntroduceObjectWithContext(objectToCreate.name);

        //  Deactivate the object
        sceneManager.DeactivateObject(objKey);
    }


    //  Stop listening to events and trigger the checking phase
    protected void End() {
        //  Added only to trigger the method in the specific learning phase at the right time
        TriggerEvent(Triggers.LearningPhaseEnd);
    }

    // ---------------------------------- ABSTRACT ------------------------------
    
    //  Handles the spawn procedure start
    protected abstract void LearningPhase();
}
