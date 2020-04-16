using System.Collections;
using UnityEngine;

//  Abstract class responsible of the Activity 3 Learning Phase
public abstract class LearningPhaseActivity3 : LearningPhaseManager
{
    protected PossessivesManager possessivesManager;
    protected AudioContext3 audioContext;

    protected override void LearningPhase() {
        audioContext = (AudioContext3)sceneManager.AudioContext;
        possessivesManager = (PossessivesManager)sceneManager;
        StartCoroutine(SceneIntroduction());
    }

    public IEnumerator ShowCharacterAndBasket(string character, string basket) {

        //  Activate the first object
        GameObject objectToCreate1 = sceneManager.ActivateObject(character, Positions.Central, Positions.ObjectsRotation);

        //  Introduce it without context
        yield return sceneManager.IntroduceObject(objectToCreate1.name);

        //  Activate the second object
        GameObject objectToCreate2 = sceneManager.ActivateObject(basket, Positions.CentralNear, Positions.ObjectsRotation);

        //  Introduce it with context
        yield return sceneManager.IntroduceObject(objectToCreate2.name);

        //  Deactivate the objects
        sceneManager.DeactivateObject(character);
        sceneManager.DeactivateObject(basket);
    }

    // --------------------------------- ABSTRACT ---------------------------------

    protected abstract IEnumerator SceneIntroduction();
}
