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

    public override void LoadObjects() {
        Pooler.Scene = ObjectPooler.ScenesEnum.Scene3;
        Pooler.Setup();
    }

    private void CreteFixedSceneObjects() {
        Tree = (GameObject)Instantiate(Resources.Load("Prefab/Objects/Tree"));
        Tree.transform.position = Positions.TreePosition;
        House = (GameObject)Instantiate(Resources.Load("Prefab/Objects/House"));
        House.transform.position = Positions.HousePosition;

    }

    public override void StartLearningPhase() {
        TriggerEvent(Triggers.LearningPhaseStart);
    }
}
