using static EventManager;

public class CandSManager : AbstractSceneManager {

    private readonly SceneSettings sceneObjects = SceneSwitcher.settings[0];

    public override void LoadObjects() {
        Pooler.CreateObjects(sceneObjects.staticObjects);
        Pooler.CreateObjects(sceneObjects.dynamicObjects);
    }

    public override void StartLearningPhase() {
        TriggerEvent(Triggers.LearningPhaseStart);
    }

}

