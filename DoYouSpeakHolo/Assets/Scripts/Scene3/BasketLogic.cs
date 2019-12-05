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

        if (objectCollider == null)
            objectCollider = gameObject.AddComponent<BoxCollider>();
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
        if (CheckIfInList(otherCollider.gameObject.name)) {
            GameManager.DeactivateObject(otherCollider.gameObject.name);
        }

        return;
    }

}
