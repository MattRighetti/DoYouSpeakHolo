using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EventManager;

public class LearningPhaseActivity3 : LearningPhaseManager {
    private List<string> maleObjects;
    private List<string> femaleObjects;

    protected override void StartSpawn() {
        StartCoroutine(ShowObjectsWithPossessives());
    }

    private IEnumerator ShowObjectsWithPossessives() {
        //Spawn VA_Male and half of the objects
        GameObject male = GetComponent<AbstractSceneManager>().ActivateObject("VA_Male", Positions.MalePosition);
        SplitObjects();

        //Show objects and wait for the spawn to finish
        StartCoroutine(ShowObjects(maleObjects));
        yield return new WaitForSeconds(3 * maleObjects.Count);

        //Do the same for the female

        GetComponent<AbstractSceneManager>().DeactivateObject(male.gameObject.name);
        GameObject female = GetComponent<AbstractSceneManager>().ActivateObject("VA_Female", Positions.FemalePosition);

        StartCoroutine(ShowObjects(femaleObjects));
        yield return new WaitForSeconds(3 * femaleObjects.Count);
        sceneManager.DeactivateObject(female.gameObject.name);

        //Start the checking phase
        TriggerEvent(Triggers.CheckingPhase);
    }

    //Split the category of the objects creating two list, one for each character
    private void SplitObjects() {
        List<string> objects = CheckingPhaseManager.Shuffle(SceneObjects);
        int half = objects.Count / 2;
        maleObjects = objects.GetRange(0, half);
        femaleObjects = objects.GetRange(half - 1, half);

    }
}