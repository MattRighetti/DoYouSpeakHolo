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

    public override void StartListeningToCustomEvents() => StartListening(Triggers.CorrectPositioning, FoundObject);

    public override void StopListeningToCustomEvents() => StopListening(Triggers.FoundObject, FoundObject);

    private void FoundObject() {
        CheckingPhaseActivity1 checkingManager = (CheckingPhaseActivity1)CheckingPhaseManager;
        checkingManager.FoundObject();
    }

    internal IEnumerator IntroduceMove(Tuple<string, DeskGrid.Cell.Prepositions, string> move) {
        yield return VirtualAssistant.IntroduceObjectWithContext(AudioContext, "Put_" + move.Item1 + "_" + DeskGrid.Cell.PrepositionAsString(move.Item2) + "_" + move.Item3);
    }

    internal IEnumerator IntroducePreposition(Tuple<string, DeskGrid.Cell.Prepositions, string> move) {
        yield return VirtualAssistant.IntroduceObjectWithContext(AudioContext, "Learn_" + move.Item1 + "_" + DeskGrid.Cell.PrepositionAsString(move.Item2) + "_" + move.Item3);
    }
}

