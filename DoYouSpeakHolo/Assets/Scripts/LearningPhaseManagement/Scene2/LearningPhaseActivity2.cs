using System.Collections;
using UnityEngine;

public class LearningPhaseActivity2 : LearningPhaseManager {

	private CandSManager candSManager;
	private AudioContext2 audioContext;
	private Positions positions;

	protected override void LearningPhase() {
		audioContext = (AudioContext2)sceneManager.AudioContext;
		candSManager = (CandSManager)sceneManager;
        positions = new Positions();
        StartCoroutine(SceneIntroduction());
	}

	private IEnumerator SceneIntroduction() {

        //1) Show all the animals
        DisplayInlineObjects();

        //2) Introduce animals with audio and outline them
        foreach (string objectkKey in SceneObjects) {
            yield return IntroduceObject(objectkKey);
        }

        //3) Introduce the smallest animal
        yield return IntroduceWithSuperlative(SceneObjects[0], Superlatives.Smallest);

        //4) Introduce animals with with comparatives
        yield return IntroduceWithComparatives();

        //5) Introduce the biggest animal
        yield return IntroduceWithSuperlative(SceneObjects[SceneObjects.Count - 1], Superlatives.Biggest);


        End();
	}

    /// <summary>
    /// Select every pair of animals, highlight them and let the VA explains their relation with comparatives
    /// </summary>
    /// <returns></returns>
    private IEnumerator IntroduceWithComparatives() {
        for (int firstAnimalIndex = 0; firstAnimalIndex < SceneObjects.Count; firstAnimalIndex++) {
            for(int secondAnimalIndex = 0; secondAnimalIndex < SceneObjects.Count; secondAnimalIndex++) {
                if (firstAnimalIndex != secondAnimalIndex) {
                    string firstAnimal = SceneObjects[firstAnimalIndex];
                    string secondAnimal = SceneObjects[secondAnimalIndex];
                    candSManager.EnableOutline(firstAnimal);
                    candSManager.EnableOutline(secondAnimal);
                    yield return candSManager.IntroduceObjectWithComparatives(firstAnimal, secondAnimal);
                    candSManager.DisableOutline(firstAnimal);
                    candSManager.DisableOutline(secondAnimal);
                    yield return new WaitForSeconds(0.5f);
                }
                    
            }
        }
    }

    private IEnumerator IntroduceWithSuperlative(string objectKey, Superlatives superlative) {
        candSManager.EnableOutline(objectKey);
        yield return candSManager.IntroduceObjectWithSuperlatives(objectKey, superlative.Value);
        candSManager.DisableOutline(objectKey);
    }

    private IEnumerator IntroduceObject(string objectKey) {
        candSManager.EnableOutline(objectKey);
        yield return candSManager.IntroduceObject(objectKey);
        candSManager.DisableOutline(objectKey);
    }

    private void DisplayInlineObjects() {
		Vector3 startingPosition = Positions.startPositionInlineThree;

		foreach (string obj in SceneObjects) {
			candSManager.ActivateObject(obj, positions.GetPosition(startingPosition), Positions.ObjectsRotation);
			startingPosition += new Vector3(0.2f, 0, 0);
		}
	}

}
