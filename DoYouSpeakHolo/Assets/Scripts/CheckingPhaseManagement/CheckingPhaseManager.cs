using System.Collections.Generic;
using UnityEngine;
using static EventManager;

//  Abstract class responsible of the Checking Phase
public abstract class CheckingPhaseManager : MonoBehaviour {

    protected List<string> SceneObjects;

    protected AbstractSceneManager sceneManager;

    public void Setup() {
        sceneManager = GetComponent<AbstractSceneManager>();
        sceneManager.CheckingPhaseManager = this;
        SceneObjects = sceneManager.GetObjects();
    }

    public void StartCheckingPhase() {
        CheckingPhase();
    }

    //Stop listening to events and trigger the new phase
    private void End() {
        StopListening(Triggers.CheckingPhaseStart, StartCheckingPhase);
    }


    // ------------------------------------ ABSTRACT ----------------------------------

    protected abstract void CheckingPhase();
}
