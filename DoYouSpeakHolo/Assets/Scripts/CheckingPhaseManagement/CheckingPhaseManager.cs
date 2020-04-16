using System.Collections.Generic;
using UnityEngine;
using static EventManager;


/// <summary>
/// Abstract class responsible of the Checking Phase
/// </summary>
/// <remarks>
/// The class contains the methods guaranteeing the flow of the activity (start and end of checking phase)
/// and the abstract method to be overrided in order to implement a specific checking phases
/// </remarks>
public abstract class CheckingPhaseManager : MonoBehaviour {

    /// <summary>
    /// Contains the scene objects (the dynamic objects loaded from JSON)
    /// </summary>
    protected List<string> SceneObjects;

    protected AbstractSceneManager sceneManager;

    /// <summary>
    /// Configure the class setting the references with the AbstractSceneManager 
    /// from both sides.
    /// </summary>
    public void Setup() {
        sceneManager = GetComponent<AbstractSceneManager>();
        sceneManager.CheckingPhaseManager = this;
        SceneObjects = sceneManager.GetObjects();
    }

    /// <summary>
    /// Trigger the checking phase start.
    /// </summary>
    public void StartCheckingPhase() {
        CheckingPhase();
    }

    /// <summary>
    /// Stop listening to events and triggers the new phase
    /// </summary>
    protected void End() {
        StopListening(Triggers.CheckingPhaseStart, StartCheckingPhase);
        TriggerEvent(Triggers.CheckingPhaseEnd);

        Destroy(GameObject.Find("SceneSelected"));
        Destroy(GameObject.Find("SpatialMapping"));
        Destroy(GameObject.Find("SpatialProcessing"));
    }


    // ------------------------------------ ABSTRACT ----------------------------------

    
    /// <summary>
    /// Override to implement the checking phase
    /// </summary>
    protected abstract void CheckingPhase();
}
