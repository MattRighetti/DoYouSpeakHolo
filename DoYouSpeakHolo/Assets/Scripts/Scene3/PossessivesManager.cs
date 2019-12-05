using System;
using UnityEngine;
using static EventManager;

public class PossessivesManager : AbstractSceneManager
{
    private readonly SceneSettings sceneObjects = SceneSwitcher.settings[2];

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public override void LoadObjects() {
        Pooler.CreateObjects(sceneObjects.staticObjects);
        Pooler.CreateObjects(sceneObjects.dynamicObjects);
    }

    public override void StartLearningPhase() {
        TriggerEvent(Triggers.LearningPhaseStart);
    }
}
