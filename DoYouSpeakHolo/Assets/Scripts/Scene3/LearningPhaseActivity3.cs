using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EventManager;

public class LearningPhaseActivity3 : LearningPhaseManager {
    private List<string> maleObjects;
    private List<string> femaleObjects;
    private PossessivesManager possessivesManager;

    protected override void StartSpawn() {
        StartCoroutine(ShowObjectsWithPossessives());
    }

    private IEnumerator ShowObjectsWithPossessives() {
        SplitObjects();

        //Spawn VA_Male and half of the objects
        GameObject male = possessivesManager.ActivateObject("Male", Positions.MalePosition);

        //Show objects and wait for the spawn to finish
        yield return StartCoroutine(ShowObjects(maleObjects));
       
        sceneManager.DeactivateObject(male.gameObject.name);

        //Do the same for the female
        GameObject female = possessivesManager.ActivateObject("Female", Positions.FemalePosition);

        yield return StartCoroutine(ShowObjects(femaleObjects));

        possessivesManager.DeactivateObject(female.gameObject.name);

        possessivesManager = (PossessivesManager)sceneManager;

        possessivesManager.SetMaleObjects(maleObjects);
        possessivesManager.SetFemaleObjects(femaleObjects);
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