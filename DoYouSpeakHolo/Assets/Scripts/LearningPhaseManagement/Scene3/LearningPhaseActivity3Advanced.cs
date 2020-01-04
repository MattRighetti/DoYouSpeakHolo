using System.Collections;
using UnityEngine;

//  Class responsible of the Advanced Learning Phase version of Activity 3
public class LearningPhaseActivity3Advanced : LearningPhaseActivity3 {

    protected override IEnumerator SceneIntroduction() {
        //  Spawn VA_Male and half of the objects
        Character character = Character.Male;
        GameObject male = possessivesManager.ActivateObject(character.Value, Positions.MalePosition, Positions.ObjectsRotation);
        audioContext.Possessive = Possessives.His;
        //  Introduce object with context
        yield return ShowObjectsWithContext(possessivesManager.PossessivesObjects[Possessives.His.Value]);
        sceneManager.DeactivateObject(male.gameObject.name);

        //  Do the same for the female
        character = Character.Female;
        GameObject female = possessivesManager.ActivateObject(character.Value, Positions.FemalePosition, Positions.ObjectsRotation);
        audioContext.Possessive = Possessives.Her;
        yield return ShowObjectsWithContext(possessivesManager.PossessivesObjects[Possessives.Her.Value]);
        possessivesManager.DeactivateObject(female.gameObject.name);
        //  End the learning phase
        End();
    }
}