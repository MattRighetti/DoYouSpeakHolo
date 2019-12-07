using Microsoft.MixedReality.Toolkit.UI;
using System.Collections.Generic;
using UnityEngine;
using static EventManager;

public abstract class CheckingPhaseManager : MonoBehaviour {

    protected List<string> SceneObjects;


    protected AbstractSceneManager sceneManager;

    protected void Start() {
        sceneManager = GetComponent<AbstractSceneManager>();
        StartListening(Triggers.CheckingPhaseStart, HandleStartCheckingPhase);
    }

    private void HandleStartCheckingPhase() {
        SceneObjects = sceneManager.GetObjects();
        CheckingPhase();
    }





    //Stop listening to events and trigger the new phase
    private void End() {
        StopListening(Triggers.CheckingPhaseStart, HandleStartCheckingPhase);

        //Trigger the new phase

    }

    //Randomize a List
    public static List<string> Shuffle(List<string> list) {
        List<string> randomList = new List<string>();

        System.Random random = new System.Random();
        int randomIndex = 0;

        while (list.Count > 0) {
            randomIndex = random.Next(0, list.Count);
            randomList.Add(list[randomIndex]);
            list.RemoveAt(randomIndex);
        }

        return randomList;
    }


    // ------------------------------------ ABSTRACT ----------------------------------
    protected abstract void CreateAllObjectsAndDisplayInRandomOrder();
    protected abstract void CheckingPhase();
}
