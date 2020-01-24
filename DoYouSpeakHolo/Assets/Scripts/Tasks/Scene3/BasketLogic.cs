using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasketLogic : MonoBehaviour {
    private PossessivesManager GameManager;
    private Animator anime;
    private List<string> fruitList;
    private BoxCollider objectCollider;

    void Start() {
        anime = GameObject.Find("VA").GetComponent<VirtualAssistantManager>().animator;
        SetupBoxCollider();
        SetupGameManager();
    }

    private void SetupBoxCollider() {
        objectCollider = gameObject.GetComponent<BoxCollider>();
    }

    private void SetupGameManager() {
        GameManager = GameObject.Find("SceneManager").GetComponent<PossessivesManager>();

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
            Wait(3);
        }
        else {
            //Trigger the negative reaction of the Virtual assistant
            EventManager.TriggerEvent(EventManager.Triggers.VAKo);
        }
    }

    public void Wait(float seconds) {
        StartCoroutine(_wait(seconds));

    }

    IEnumerator _wait(float time) {
        yield return new WaitForSeconds(time);
        Debug.Log("Picked fruit");
        EventManager.TriggerEvent(EventManager.Triggers.PickedFruit);
    }

}
