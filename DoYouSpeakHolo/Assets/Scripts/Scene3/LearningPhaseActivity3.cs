using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LearningPhaseActivity3 : LearningPhaseManager {
    private List<string> maleObjects;
    private List<string> femaleObjects;
    private PossessivesManager possessivesManager;
    AudioContext3 audioContext;

    protected override void LearningPhase() {
        audioContext = (AudioContext3)sceneManager.AudioContext;
        possessivesManager = (PossessivesManager)sceneManager;
        StartCoroutine(SceneIntroduction());
    }

    private IEnumerator SceneIntroduction() {
        yield return IntroduceCharacterAndBasket(Character.Male);
        yield return IntroduceCharacterAndBasket(Character.Female);
        yield return ShowObjects(SceneObjects);
        yield return End();
    }

    private IEnumerator IntroduceCharacterAndBasket(Character character) {

        //Set the right possessive
        if (character.Value == "Male") {
            audioContext.Possessive = Possessives.His;
            yield return ShowObjectPair(character.Value, character.Value + "Basket");
        } else {
            audioContext.Possessive = Possessives.Her;
            yield return ShowObjectPair(character.Value, character.Value + "Basket");
        }

    }
}