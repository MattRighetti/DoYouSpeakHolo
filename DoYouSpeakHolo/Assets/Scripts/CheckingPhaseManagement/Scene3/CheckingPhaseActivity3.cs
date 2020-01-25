
using System.Collections.Generic;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.UI;
using UnityEngine;

//  Abstract class responsible of the Activity 3 Checking Phase
public abstract class CheckingPhaseActivity3 : CheckingPhaseManager
{
    protected PossessivesManager possessivesManager;
    Possessives character1Possessive;
    Possessives character2Possessive;

    //  Create and configure the characters with their baskets
    protected void CreatePeopleAndBaskets() {
        character1Possessive = Possessives.His;
        character2Possessive = Possessives.Her;
        sceneManager.ActivateObject(character1Possessive.Value + "Character", Positions.MalePosition, Positions.ObjectsRotation);
        sceneManager.ActivateObject(character2Possessive.Value + "Character", Positions.FemalePosition, Positions.ObjectsRotation);
        CreateAndConfigureBaskets();
    }

    //  Create baskets and attach the necessary scripts
    protected void CreateAndConfigureBaskets() {
        GameObject basket1 = sceneManager.ActivateObject(character1Possessive.Value + "Basket", Positions.MaleBasket, Positions.ObjectsRotation);
        GameObject basket2 = sceneManager.ActivateObject(character2Possessive.Value + "Basket", Positions.FemaleBasket, Positions.ObjectsRotation);

        ConfigureBaskets(basket1);
        ConfigureBaskets(basket2);

        basket1.GetComponent<BasketLogic>().SetFruitList(possessivesManager.PossessivesObjects[Possessives.His.Value]);
        basket2.GetComponent<BasketLogic>().SetFruitList(possessivesManager.PossessivesObjects[Possessives.Her.Value]);
    }

    //  Attach the scripts to the baskets
    protected void ConfigureBaskets(GameObject basket) {
        basket.GetComponent<BoxCollider>().isTrigger = true;
        //  TODO:Delete ManipulationHandler -> the basket can be dragged
        //basket.AddComponent<ManipulationHandler>();
        basket.AddComponent<DoNotFall>();
        basket.AddComponent<BasketLogic>();
    }

    //  Add to the object al the scripts needed for the activity
    protected void SetFruitScripts(GameObject gameObj) {
        Rigidbody body = gameObj.AddComponent<Rigidbody>();
        body.useGravity = true;
        body.constraints = RigidbodyConstraints.FreezeRotation;
        gameObj.AddComponent<BoxCollider>();
        gameObj.AddComponent<ManipulationHandler>();
        gameObj.AddComponent<NearInteractionGrabbable>();
       // gameObj.AddComponent<DoNotFall>();
    }

    //  Executed every time the user puts a fruit into the correct basket
    //  1) Check if all the fruits of a specific possessives have already been putted into the right basket
    //  2) If there are no more fruits trigger the end of the Activity
    public virtual void PickedFruit() {
        DeleteEmptyPossessives();

        if (possessivesManager.PossessivesObjects.Count == 0) {
            End();
        }
    }

    //  Checks whether a list of fruits associated to a possessive is empty and in case delete the entry from the dictonary
    private void DeleteEmptyPossessives() {
        List<string> possessivesToRemove = CheckForEmptyPossessives();

        foreach (string possessive in possessivesToRemove) {
            possessivesManager.PossessivesObjects.Remove(possessive);
        }
    }

    //  Checks whether a list of fruits associated to a possessive is empty and return its key
    private List<string> CheckForEmptyPossessives() {
        List<string> possessivesToRemove = new List<string>();
        foreach (KeyValuePair<string, List<string>> tuple in possessivesManager.PossessivesObjects) {
            if (tuple.Value.Count == 0)
                possessivesToRemove.Add(tuple.Key);
        }
        return possessivesToRemove;
    }
}
