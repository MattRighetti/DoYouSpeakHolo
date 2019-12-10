using System.Collections.Generic;
using static EventManager;

public class PossessivesManager : AbstractSceneManager
{
    private readonly SceneSettings sceneObjects = SceneSwitcher.settings[2];
    public List<string> MaleObjects { get; set; }
    public List<string> FemaleObjects { get; set; }
    private int basketFull = 0;

    public override void LoadObjects() {
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
        MaleObjects = maleObjects;
    }

    internal void SetFemaleObjects(List<string> femaleObjects) {
        FemaleObjects = femaleObjects;
    }

    public override void StartListeningToCustomEvents() {
        StartListening(Triggers.BasketEmpty, CheckBaskets);
    }

    public override void StopListeningToCustomEvents() {
        StopListening(Triggers.BasketEmpty, CheckBaskets);
    }

    public override void IntroduceObject(string objKey) {
        throw new System.NotImplementedException();
    }
}
