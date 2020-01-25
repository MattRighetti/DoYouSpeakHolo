using System.Collections;
using UnityEngine;

//  Class responsible of the Advanced Learning Phase version of Activity 3
public class LearningPhaseActivity3Advanced : LearningPhaseActivity3 {

    protected override IEnumerator SceneIntroduction() {
        //  Spawn VA_Male and half of the objects
        Possessives character1Possessive = Possessives.His;
        audioContext.Possessive = character1Possessive;

        GameObject male = possessivesManager.ActivateObject(character1Possessive.Value + "Character", Positions.MalePosition, Positions.ObjectsRotation);
        //  Introduce object with context
        yield return ShowObjectsWithContext(possessivesManager.PossessivesObjects[Possessives.His.Value]);
        sceneManager.DeactivateObject(male.gameObject.name);

        //  Do the same for the female
        Possessives character2Possessive = Possessives.Her;
        audioContext.Possessive = character2Possessive;
        GameObject female = possessivesManager.ActivateObject(character2Possessive.Value + "Character", Positions.FemalePosition, Positions.ObjectsRotation);
        yield return ShowObjectsWithContext(possessivesManager.PossessivesObjects[Possessives.Her.Value]);
        possessivesManager.DeactivateObject(female.gameObject.name);
        //  End the learning phase
        End();
    }
}