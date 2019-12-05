using UnityEngine;
using static EventManager;

public class PossessivesManager : AbstractSceneManager
{
    private readonly SceneSettings sceneObjects = SceneSwitcher.settings[2];

    public override void LoadObjects() {
        Pooler.CreateObjects(sceneObjects.staticObjects);
        Pooler.CreateObjects(sceneObjects.dynamicObjects);
        CreateScene();
    }

    //Create static elements of the scene
    private void CreateScene() {
        Pooler.ActivateObject("House", Positions.HousePosition);
        Pooler.ActivateObject("Tree", Positions.TreePosition);
    }

    public override void StartLearningPhase() {
        TriggerEvent(Triggers.LearningPhaseStart);
    }
}
