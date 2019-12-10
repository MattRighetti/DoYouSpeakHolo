using Microsoft.MixedReality.Toolkit.UI;
using UnityEngine;
using static EventManager;

class CheckingPhaseActivity1 : CheckingPhaseManager {

    private CandSManager candsManager;



    //Spawn the objects in random order and ask the user to pick a specific one
    protected override void CheckingPhase() {
        candsManager = (CandSManager)sceneManager;

        CreateAllObjectsAndDisplayInRandomOrder();
        //Shuffle the collection again
        SceneObjects = AbstractSceneManager.Shuffle(SceneObjects);

        //TODO call it in another method
        TriggerEvent(Triggers.SetTargetObject);
    }


    protected override void CreateAllObjectsAndDisplayInRandomOrder() {
        //Shuffle the collection
        SceneObjects = AbstractSceneManager.Shuffle(SceneObjects);
        GameObject gameObj;
        //Define initial spawning position
        Vector3 startPosition = Positions.startPositionInlineFour;
        foreach (string obj in SceneObjects) {
            //Activate the object and attach to it the script for the task
            gameObj = candsManager.ActivateObject(obj, startPosition);
            FindObjectTask task = gameObj.AddComponent<FindObjectTask>();
            gameObj.AddComponent<Interactable>().AddReceiver<InteractableOnPressReceiver>().OnPress.AddListener(() => task.Check());
            startPosition += new Vector3(0.5f, 0, 0);
        }
    }
}

