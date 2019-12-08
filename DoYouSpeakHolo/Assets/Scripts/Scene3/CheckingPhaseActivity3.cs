using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class CheckingPhaseActivity3 : CheckingPhaseManager {

    protected override void CheckingPhase() {

        //Create the baskets and attach to them the script
        CreatePeopleAndBaskets();

        //Spawn Fruits in random order
        CreateAllObjectsAndDisplayInRandomOrder();
    }

    private void CreatePeopleAndBaskets() {
        Debug.Log("Setting check phase");
        sceneManager.ActivateObject("Male", Positions.MalePosition);
        sceneManager.ActivateObject("Female", Positions.FemalePosition);
        sceneManager.ActivateObject("MaleBasket", Positions.MaleBasket);
        sceneManager.ActivateObject("FemaleBasket", Positions.FemaleBasket);
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
            startPosition += new Vector3(0.5f, 0, 0);
        }
    }
}
