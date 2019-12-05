using System;
using UnityEngine;
using static EventManager;

public class PossessivesManager : AbstractSceneManager
{
    private GameObject Tree;
    private GameObject House;

    // Start is called before the first frame update
    void Start()
    {
        CreteFixedSceneObjects();
        //    gameObject.GetComponent<LearningPhaseManager>().SetScene(LearningPhaseManager.ScenesEnum.Scene3);
        //  StartListening(Triggers.PickedFruit, CountFruits);
        
    }

    //TODO Add this when we are sure that everything works
    private void CreteFixedSceneObjects() {
//        Tree = Instantiate(Resources.Load("Prefab/objects/Tree", typeof(GameObject))) as GameObject;
//        Tree.transform.position = Positions.TreePosition;
//        House = Instantiate(Resources.Load("Prefab/objects/House", typeof(GameObject))) as GameObject;
//        House.transform.position = Positions.TreePosition;
    }

    public override void StartLearningPhase() {
        TriggerEvent(Triggers.LearningPhaseStart);
    }
}
