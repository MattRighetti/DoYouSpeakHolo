using UnityEngine;
using static EventManager;

public class PossessivesManager : AbstractSceneManager
{
    private readonly SceneSettings sceneObjects = SceneSwitcher.settings[2];

    public override void LoadObjects() {
        Pooler.CreateStaticObjects(sceneObjects.staticObjects);
        Pooler.CreateDynamicObjects(sceneObjects.dynamicObjects);
        CreateScene();
    }

    //Create static elements of the scene
    private void CreateScene() {
        Pooler.ActivateObject("House", Positions.HousePosition);
        Pooler.ActivateObject("Tree", Positions.TreePosition);
        Pooler.ActivateObject("VA", Positions.VAPosition);
    }

    public override void StartLearningPhase() {
        TriggerEvent(Triggers.LearningPhaseStart);
    }

    public override void IntroduceObject(string objKey) {
        
    }
}
