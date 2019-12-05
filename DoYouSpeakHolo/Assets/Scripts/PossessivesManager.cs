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

    private void CreteFixedSceneObjects() {
        Tree = ActivateObject("Tree", Positions.TreePosition);
        House = ActivateObject("House", Positions.HousePosition);
    }

    public override void StartLearningPhase() {
        TriggerEvent(Triggers.LearningPhaseStart);
    }
}
