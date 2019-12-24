using System.Collections.Generic;
using UnityEngine;
using static EventManager;

public abstract class CheckingPhaseManager : MonoBehaviour {

    protected List<string> SceneObjects;

    protected AbstractSceneManager sceneManager;

    protected void Start() {
       // Setup();
    }

    public void Setup() {
        //Get the ObjectPooler instance
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
