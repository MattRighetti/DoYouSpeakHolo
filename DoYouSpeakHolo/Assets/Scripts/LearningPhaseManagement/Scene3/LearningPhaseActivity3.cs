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

    public IEnumerator ShowCharacterAndBasket(string object1, string object2) {

        //  Activate the first object
        GameObject objectToCreate1 = sceneManager.ActivateObject(object1, Positions.Central, Positions.ObjectsRotation);

        //  Introduce it without context
        yield return sceneManager.IntroduceObject(objectToCreate1.name);

        //  Activate the second object
        GameObject objectToCreate2 = sceneManager.ActivateObject(object2, Positions.CentralNear, Positions.ObjectsRotation);

        //  Introduce it with context
        yield return sceneManager.IntroduceObject(objectToCreate2.name);

        //  Deactivate the objects
        sceneManager.DeactivateObject(object1);
        sceneManager.DeactivateObject(object2);
    }

    // --------------------------------- ABSTRACT ---------------------------------

    protected abstract IEnumerator SceneIntroduction();
}
