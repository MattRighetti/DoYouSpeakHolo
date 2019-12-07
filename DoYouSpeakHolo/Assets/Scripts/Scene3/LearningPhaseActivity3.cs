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
        SplitObjects();

        //Spawn VA_Male and half of the objects
        GameObject male = GetComponent<AbstractSceneManager>().ActivateObject("Male", Positions.MalePosition);

        //Show objects and wait for the spawn to finish
        StartCoroutine(ShowObjects(maleObjects));
        yield return new WaitForSeconds(3 * maleObjects.Count);

        sceneManager.DeactivateObject(male.gameObject.name);

        //Do the same for the female
        GameObject female = GetComponent<AbstractSceneManager>().ActivateObject("Female", Positions.FemalePosition);

        StartCoroutine(ShowObjects(femaleObjects));
        yield return new WaitForSeconds(3 * femaleObjects.Count);

        sceneManager.DeactivateObject(female.gameObject.name);

        //End the learning phase
        TriggerEvent(Triggers.LearningPhaseEnd);
    }

    //Split the category of the objects creating two list, one for each character
    private void SplitObjects() {
        List<string> objects = CheckingPhaseManager.Shuffle(SceneObjects);
        int half = objects.Count / 2;
        maleObjects = objects.GetRange(0, half);
        femaleObjects = objects.GetRange(half, half);

    }
}