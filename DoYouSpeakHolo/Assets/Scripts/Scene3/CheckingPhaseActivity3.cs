using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class CheckingPhaseActivity3 : CheckingPhaseManager {
    private PossessivesManager possessivesManager;

    protected override void CheckingPhase() {

        possessivesManager = (PossessivesManager)sceneManager;

        //Create the baskets and attach to them the script
        CreatePeopleAndBaskets();

        //Spawn Fruits in random order
        CreateAllObjectsAndDisplayInRandomOrder();
    }

    private void CreatePeopleAndBaskets() {
        Debug.Log("Setting check phase");
        sceneManager.ActivateObject("Male", Positions.MalePosition);
        sceneManager.ActivateObject("Female", Positions.FemalePosition);
        CreateAndConfigureBaskets();
    }

    private void CreateAndConfigureBaskets() {
        GameObject basket1 = sceneManager.ActivateObject("MaleBasket", Positions.MaleBasket);
        GameObject basket2 = sceneManager.ActivateObject("FemaleBasket", Positions.FemaleBasket);

        ConfigureBaskets(basket1);
        ConfigureBaskets(basket2);

        basket1.GetComponent<BasketLogic>().SetFruitList(possessivesManager.maleObjects);
        basket2.GetComponent<BasketLogic>().SetFruitList(possessivesManager.femaleObjects);
    }

    private void ConfigureBaskets(GameObject basket) {
        basket.AddComponent<DoNotFall>();
        basket.AddComponent<Rigidbody>();
        basket.AddComponent<BoxCollider>().isTrigger = true;
        basket.AddComponent <ManipulationHandler>();
        basket.AddComponent<BasketLogic>();
    }

    protected override void CreateAllObjectsAndDisplayInRandomOrder() {
        //Shuffle the collection
        SceneObjects = Shuffle(SceneObjects);
        GameObject gameObj;
        //Define initial spawning position
        Vector3 startPosition = Positions.startPositionInlineFour;
        foreach (string obj in SceneObjects) {
            //Activate the object and attach to it the script for the task
            gameObj = GetComponent<AbstractSceneManager>().ActivateObject(obj, startPosition);
            startPosition += new Vector3(0.25f, 0, 0);
            gameObj.AddComponent<DoNotFall>();
            gameObj.AddComponent<BoxCollider>();
            Rigidbody body = gameObj.AddComponent<Rigidbody>();
            body.useGravity = true;
            body.constraints = RigidbodyConstraints.FreezeRotation;
            gameObj.AddComponent<ManipulationHandler>();
            gameObj.AddComponent<NearInteractionGrabbable>();
        }
    }
}
