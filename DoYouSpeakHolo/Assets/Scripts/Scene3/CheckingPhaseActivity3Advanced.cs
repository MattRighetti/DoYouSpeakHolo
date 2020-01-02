using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.UI;
using UnityEngine;

class CheckingPhaseActivity3Advanced : CheckingPhaseManager {
    private PossessivesManager possessivesManager;

    protected override void CheckingPhase() {

        possessivesManager = (PossessivesManager)sceneManager;

        //Create the baskets and attach to them the script
        CreatePeopleAndBaskets();

        //Spawn Fruits in random order
        CreateAllObjectsAndDisplayInRandomOrder();
    }

    private void CreatePeopleAndBaskets() {
        sceneManager.ActivateObject("Male", Positions.MalePosition);
        sceneManager.ActivateObject("Female", Positions.FemalePosition);
        CreateAndConfigureBaskets();
    }

    private void CreateAndConfigureBaskets() {
        GameObject basket1 = sceneManager.ActivateObject("MaleBasket", Positions.MaleBasket);
        GameObject basket2 = sceneManager.ActivateObject("FemaleBasket", Positions.FemaleBasket);

        ConfigureBaskets(basket1);
        ConfigureBaskets(basket2);

        basket1.GetComponent<BasketLogic>().SetFruitList(possessivesManager.MaleObjects);
        basket2.GetComponent<BasketLogic>().SetFruitList(possessivesManager.FemaleObjects);
    }

    private void ConfigureBaskets(GameObject basket) {
        basket.AddComponent<Rigidbody>();
        basket.AddComponent<BoxCollider>().isTrigger = true;
        basket.AddComponent<ManipulationHandler>();
        basket.AddComponent<DoNotFall>();
        basket.AddComponent<BasketLogic>();
    }

    protected void CreateAllObjectsAndDisplayInRandomOrder() {
        //Shuffle the collection
        SceneObjects = AbstractSceneManager.Shuffle(SceneObjects);
        GameObject gameObj;
        //Define initial spawning position
        Vector3 startPosition = Positions.startPositionInlineFour;
        foreach (string obj in SceneObjects) {
            //Activate the object and attach to it the script for the task
            gameObj = possessivesManager.ActivateObject(obj, startPosition);
            SetFruitScripts(gameObj);
            startPosition += new Vector3(0.2f, 0, 0);
        }
    }

    //Add to the object al the scripts needed for the activity
    private void SetFruitScripts(GameObject gameObj) {
        Rigidbody body = gameObj.AddComponent<Rigidbody>();
        body.useGravity = true;
        body.constraints = RigidbodyConstraints.FreezeRotation;
        gameObj.AddComponent<BoxCollider>();
        gameObj.AddComponent<ManipulationHandler>();
        gameObj.AddComponent<NearInteractionGrabbable>();
        gameObj.AddComponent<DoNotFall>();
    }
}