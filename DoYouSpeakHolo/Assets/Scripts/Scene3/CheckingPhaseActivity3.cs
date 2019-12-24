
using System;
using System.Collections.Generic;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.UI;
using UnityEngine;

public abstract class CheckingPhaseActivity3 : CheckingPhaseManager
{
    protected PossessivesManager possessivesManager;

    protected void CreatePeopleAndBaskets() {
        sceneManager.ActivateObject("Male", Positions.MalePosition);
        sceneManager.ActivateObject("Female", Positions.FemalePosition);
        CreateAndConfigureBaskets();
    }

    protected void CreateAndConfigureBaskets() {
        GameObject basket1 = sceneManager.ActivateObject("MaleBasket", Positions.MaleBasket);
        GameObject basket2 = sceneManager.ActivateObject("FemaleBasket", Positions.FemaleBasket);

        ConfigureBaskets(basket1);
        ConfigureBaskets(basket2);

        basket1.GetComponent<BasketLogic>().SetFruitList(possessivesManager.PossessivesObjects[Possessives.His.Value]);
        basket2.GetComponent<BasketLogic>().SetFruitList(possessivesManager.PossessivesObjects[Possessives.Her.Value]);
    }

    protected void ConfigureBaskets(GameObject basket) {
        basket.AddComponent<Rigidbody>();
        basket.AddComponent<BoxCollider>().isTrigger = true;
        //TODO:Delete ManipulationHandler -> the basket can be dragged
        basket.AddComponent<ManipulationHandler>();
        basket.AddComponent<DoNotFall>();
        basket.AddComponent<BasketLogic>();
    }

    //Add to the object al the scripts needed for the activity
    protected void SetFruitScripts(GameObject gameObj) {
        Rigidbody body = gameObj.AddComponent<Rigidbody>();
        body.useGravity = true;
        body.constraints = RigidbodyConstraints.FreezeRotation;
        gameObj.AddComponent<BoxCollider>();
        gameObj.AddComponent<ManipulationHandler>();
        gameObj.AddComponent<NearInteractionGrabbable>();
        gameObj.AddComponent<DoNotFall>();
    }

    public virtual void PickedFruit() {
        DeleteEmptyPossessives();
       
        //If there are no more fruits end the activity
        if (possessivesManager.PossessivesObjects.Count == 0) {
            EventManager.TriggerEvent(EventManager.Triggers.CheckingPhaseEnd);
        }
    }

    private void DeleteEmptyPossessives() {
        List<string> possessivesToRemove = CheckForEmptyPossessives();

        foreach (string possessive in possessivesToRemove) {
            possessivesManager.PossessivesObjects.Remove(possessive);
        }
    }

    private List<string> CheckForEmptyPossessives() {
        List<string> possessivesToRemove = new List<string>();
        foreach (KeyValuePair<string, List<string>> tuple in possessivesManager.PossessivesObjects) {
            if (tuple.Value.Count == 0)
                possessivesToRemove.Add(tuple.Key);
        }
        return possessivesToRemove;
    }
}
