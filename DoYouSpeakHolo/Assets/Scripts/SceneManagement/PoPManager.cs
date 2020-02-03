using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EventManager;

//  Scene Manager of Activity 1
public class PoPManager : AbstractSceneManager {

    public List<string> TargetObjects { get; set; }
    public DeskGrid Grid { get; private set; }

    //  Set the audio context to scene 1
    public override void SetAudioContext() {
        AudioContext = new AudioContext1();
    }

    //  Create the scene objects
    public override void LoadObjects() {

        Grid = Pooler.GetGrid();
        Pooler.CreateStaticObjects(sceneObjects.staticObjects);
        Pooler.CreateDynamicObjects(sceneObjects.dynamicObjects);

    }

    //private void FoundObject() {
    //    //increase the counter
    //    findObjectCounter++;
    //    if (findObjectCounter == TargetObjects.Count - 1) {
    //        //Objects are finished
    //    }
    //    else {
    //        SetTargetObject();
    //    }
    //}

    ////Set the object that the user has to find
    //private void SetTargetObject() {
    //    //VA tells the User which object he needs to find to complete the task
    //    Debug.Log("Object to find " + TargetObjects[findObjectCounter]);

    //    //Set the current target
    //    GetPooledObject(TargetObjects[findObjectCounter]).GetComponent<FindObjectTask>().IsTarget = true;
    //}

    public override void StartListeningToCustomEvents() {
        StartListening(Triggers.CorrectPositioning, FoundObject);
    //    StartListening(Triggers.SetTargetObject, SetTargetObject);
    }

    private void FoundObject() {
        CheckingPhaseActivity1 checkingManager = (CheckingPhaseActivity1)CheckingPhaseManager;
        checkingManager.FoundObject();
    }

    public override void StopListeningToCustomEvents() {
        //StopListening(Triggers.FoundObject, FoundObject);
        //StopListening(Triggers.SetTargetObject, SetTargetObject);
    }

    internal IEnumerator IntroduceMove(Tuple<string, DeskGrid.Cell.Prepositions, string> move) {
        yield return VirtualAssistant.IntroduceObjectWithContext(AudioContext, move.Item1 + "_" + DeskGrid.Cell.PrepositionAsString(move.Item2) + "_" + move.Item3);
    }
}

