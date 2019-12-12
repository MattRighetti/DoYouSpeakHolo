using System;
using System.Collections.Generic;
using UnityEngine;

public class BasketLogic : MonoBehaviour {
    PossessivesManager GameManager;

    private List<string> fruitList;
    BoxCollider objectCollider;

    void Start() {
        SetupBoxCollider();
        SetupGameManager();
    }

    private void SetupBoxCollider() {
        objectCollider = gameObject.GetComponent<BoxCollider>();
    }

    private void SetupGameManager() {
        GameManager = GameObject.Find("PossessivesManager").GetComponent<PossessivesManager>();

        if (GameManager == null)
            throw new Exception("GameManager Object could not be found");
    }

    public void SetFruitList(List<string> fruitListString) {
        fruitList = fruitListString;
    }

    public List<string> GetFruitList() {
        return fruitList;
    }

    private bool CheckIfInList(string objectStringIdentifier) {
        return fruitList.Contains(objectStringIdentifier);
    }

    private void OnTriggerEnter(Collider otherCollider) {
        //If the collider belongs to a target fruit
        if (CheckIfInList(otherCollider.gameObject.name)) {
            //Make the object disappear
            GameManager.DeactivateObject(otherCollider.gameObject.name);
            //Remove it from the list of target fruits
            fruitList.Remove(otherCollider.gameObject.name);
            //Trigger the positive reaction of the Virtual assistant
            EventManager.TriggerEvent(EventManager.Triggers.VAOk);
            EventManager.TriggerEvent(EventManager.Triggers.PickedFruit);
            //If there are no more target fruits
            if (fruitList.Count == 0) {
                //Notify the Possessives Manager
                EventManager.TriggerEvent(EventManager.Triggers.BasketEmpty);
            }
        }
        else {
            //Trigger the negative reaction of the Virtual assistant
            EventManager.TriggerEvent(EventManager.Triggers.VAKo);
        }

        return;
    }

}
