using System;
using System.Collections.Generic;
using static EventManager;

public class PossessivesManager : AbstractSceneManager
{
    private readonly SceneSettings sceneObjects = SceneSwitcher.settings[2];
    public List<string> maleObjects;
    public List<string> femaleObjects;
    private int basketFull = 0;
    

    public override void LoadObjects() {
        EventManager.StartListening(Triggers.BasketEmpty, CheckBaskets);
        Pooler.CreateStaticObjects(sceneObjects.staticObjects);
        Pooler.CreateDynamicObjects(sceneObjects.dynamicObjects);
        CreateScene();
    }

    private void CheckBaskets() {
        basketFull++;

        if (basketFull == 2) {
            EndActivity();
        }
    }

    //Create static elements of the scene
    private void CreateScene() {
        Pooler.ActivateObject("House", Positions.HousePosition);
        Pooler.ActivateObject("Tree", Positions.TreePosition);
        Pooler.ActivateObject("VA", Positions.VAPosition);
    }

    internal void SetMaleObjects(List<string> maleObjects) {
        this.maleObjects = maleObjects;
    }

    internal void SetFemaleObjects(List<string> femaleObjects) {
        this.femaleObjects = femaleObjects;
    }

    public override void StartLearningPhase() {
        TriggerEvent(Triggers.LearningPhaseStart);
    }

    public override void IntroduceObject(string objKey) {
        
    }
}
