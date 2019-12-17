using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LearningPhaseActivity3Advanced : LearningPhaseManager {
    private List<string> maleObjects;
    private List<string> femaleObjects;
    private PossessivesManager possessivesManager;
    private AudioContext3 audioContext;

    protected override void LearningPhase() {
        audioContext = (AudioContext3)sceneManager.AudioContext;
        possessivesManager = (PossessivesManager)sceneManager;
        StartCoroutine(SceneIntroduction());
    }

    protected override IEnumerator SceneIntroduction() {
        SplitObjects();


        //Spawn VA_Male and half of the objects
        Character character = Character.Male;
        GameObject male = possessivesManager.ActivateObject(character.Value, Positions.MalePosition);
        audioContext.Possessive = Possessives.His;
        //Introduce object with context
        yield return ShowObjectsWithContext(maleObjects);
        sceneManager.DeactivateObject(male.gameObject.name);

        //Do the same for the female
        character = Character.Female;
        GameObject female = possessivesManager.ActivateObject(character.Value, Positions.FemalePosition);
        audioContext.Possessive = Possessives.Her;
        yield return ShowObjectsWithContext(femaleObjects);
        possessivesManager.DeactivateObject(female.gameObject.name);

        //Set the list of target fruits into the PossessivesManager
        possessivesManager.SetMaleObjects(maleObjects);
        possessivesManager.SetFemaleObjects(femaleObjects);

        //End the learning phase
        End();
    }

    //Split the category of the objects creating two list, one for each character
    private void SplitObjects() {
        List<string> objects = AbstractSceneManager.Shuffle(SceneObjects);
        int half = objects.Count / 2;
        maleObjects = objects.GetRange(0, half);
        femaleObjects = objects.GetRange(half, half);
    }
}