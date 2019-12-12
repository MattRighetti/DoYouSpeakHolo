using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LearningPhaseActivity3Advanced : LearningPhaseManager
{
    private List<string> maleObjects;
    private List<string> femaleObjects;
    private PossessivesManager possessivesManager;
    AudioContext3 audioContext;

    protected override void LearningPhase() {
        possessivesManager = (PossessivesManager)sceneManager;
        audioContext = (AudioContext3)possessivesManager.AudioContext;
        StartCoroutine(SceneIntroduction());
    }

    private IEnumerator SceneIntroduction() {
        yield return ShowObjectsWithPossessives();
        yield return End();

        //TODO:Move to activity 3 advanced
        //yield return ShowObjectsWithPossessives();
    }

    private IEnumerator ShowObjectsWithPossessives() {
        SplitObjects();

        //Spawn VA_Male and half of the objects
        GameObject male = possessivesManager.ActivateObject("Male", Positions.MalePosition);

        //Set the AudioContext possessive
        audioContext.Possessive = Possessives.His;

        //Show objects and wait for the spawn to finish
        yield return StartCoroutine(ShowObjectsWithContext(maleObjects));

        sceneManager.DeactivateObject(male.gameObject.name);

        //Do the same for the female
        GameObject female = possessivesManager.ActivateObject("Female", Positions.FemalePosition);

        audioContext.Possessive = Possessives.Her;

        yield return StartCoroutine(ShowObjectsWithContext(femaleObjects));

        possessivesManager.DeactivateObject(female.gameObject.name);

        //Set the list of target fruits into the PossessivesManager
        possessivesManager.SetMaleObjects(maleObjects);
        possessivesManager.SetFemaleObjects(femaleObjects);
    }

    //Split the category of the objects creating two list, one for each character
    private void SplitObjects() {
        List<string> objects = AbstractSceneManager.Shuffle(SceneObjects);
        int half = objects.Count / 2;
        maleObjects = objects.GetRange(0, half);
        femaleObjects = objects.GetRange(half, half);
    }
}
