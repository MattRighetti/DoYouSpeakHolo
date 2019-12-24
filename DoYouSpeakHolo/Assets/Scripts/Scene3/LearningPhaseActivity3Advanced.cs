using System.Collections;
using UnityEngine;

public class LearningPhaseActivity3Advanced : LearningPhaseActivity3 {

    protected override IEnumerator SceneIntroduction() {
        //Spawn VA_Male and half of the objects
        Character character = Character.Male;
        GameObject male = possessivesManager.ActivateObject(character.Value, Positions.MalePosition);
        audioContext.Possessive = Possessives.His;
        //Introduce object with context
        yield return ShowObjectsWithContext(possessivesManager.PossessivesObjects[Possessives.His.Value]);
        sceneManager.DeactivateObject(male.gameObject.name);

        //Do the same for the female
        character = Character.Female;
        GameObject female = possessivesManager.ActivateObject(character.Value, Positions.FemalePosition);
        audioContext.Possessive = Possessives.Her;
        yield return ShowObjectsWithContext(possessivesManager.PossessivesObjects[Possessives.Her.Value]);
        possessivesManager.DeactivateObject(female.gameObject.name);
        //End the learning phase
        End();
    }
}