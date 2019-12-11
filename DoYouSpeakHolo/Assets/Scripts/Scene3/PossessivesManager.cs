using System.Collections.Generic;
using static EventManager;

public class PossessivesManager : AbstractSceneManager
{

    private readonly SceneObjectsToLoad sceneObjects = SceneSwitcher.settings[2];

    //Target fruits of the male basket
    public List<string> MaleObjects { get; set; }

    //Target fruits of the female basket
    public List<string> FemaleObjects { get; set; }

    //Keeps track of the basket with no more target fruits
    private int basketFull = 0;

    //Load scene objects
    public override void LoadObjects() {
        Pooler.CreateStaticObjects(sceneObjects.staticObjects);
        Pooler.CreateDynamicObjects(sceneObjects.dynamicObjects);
        CreateScene();
    }

    //Chek if all the baskets are full
    private void CheckBaskets() {
        basketFull++;

        if (basketFull == 2) {
            EndActivity();
        }
    }

    //Activate and put the static elements in the scene
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
