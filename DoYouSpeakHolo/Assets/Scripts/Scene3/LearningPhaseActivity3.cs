using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LearningPhaseActivity3 : LearningPhaseManager {
    private List<string> maleObjects;
    private List<string> femaleObjects;
    private PossessivesManager possessivesManager;
    private AudioContext3 audioContext;

    //Start the Learning Phase of Activity 3
    protected override void LearningPhase() {
        audioContext = (AudioContext3)sceneManager.AudioContext;
        possessivesManager = (PossessivesManager)sceneManager;
        StartCoroutine(SceneIntroduction());
    }

    //Brief introduction of the scene made by the VA
    protected override IEnumerator SceneIntroduction() {

        //Introduce the different character and their baskets
        yield return IntroducePeopleAndBaskets();

        //Show objects one at time introducing them
        yield return ShowObjects(SceneObjects);

        //Trigger the Checking Phase
        yield return End();
    }

    private IEnumerator IntroducePeopleAndBaskets() {

        //Introduce the first character
        audioContext.Possessive = Possessives.His;
        Character character = Character.Male;
        yield return ShowCharacterAndBasket(character.Value, character.Value + "Basket");

        //Introduce the second character
        audioContext.Possessive = Possessives.Her;
        character = Character.Female;
        yield return ShowCharacterAndBasket(character.Value, character.Value + "Basket");
    }

    public IEnumerator ShowCharacterAndBasket(string object1, string object2) {

        //Activate the first object
        GameObject objectToCreate1 = sceneManager.ActivateObject(object1, Positions.Central);

        //Introduce it without context
        yield return sceneManager.IntroduceObject(objectToCreate1.name);

        //Activate the second object
        GameObject objectToCreate2 = sceneManager.ActivateObject(object2, Positions.CentralNear);

        //Introduce it with context
        yield return sceneManager.IntroduceObject(objectToCreate2.name);

        //Deactivate the objects
        sceneManager.DeactivateObject(object1);
        sceneManager.DeactivateObject(object2);
    }
}