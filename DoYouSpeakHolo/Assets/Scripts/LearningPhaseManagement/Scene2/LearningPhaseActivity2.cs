using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LearningPhaseActivity2 : LearningPhaseManager {

	private CandSManager candSManager;
	private AudioContext2 audioContext;
	private List<GameObject> gameObjects;
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

        //2) Introduce animals with audio
        foreach (string objectkKey in SceneObjects) {
            yield return candSManager.IntroduceObject(objectkKey);
        }

        //3) Introduce the smallest animal
        yield return candSManager.IntroduceObjectWithSuperlatives(SceneObjects[0], "Smallest");

        //4) Introduce animals with with comparatives
        foreach (string objectkKey in SceneObjects) {
            yield return candSManager.IntroduceObjectWithComparatives(objectkKey);
        }

        //5) Introduce the biggest animal
        yield return candSManager.IntroduceObjectWithSuperlatives(SceneObjects[SceneObjects.Count - 1], "Biggest");




        //  IntroduceObjectsWithComparativesAndSuperlatives();

        End();
	}

	private void DisplayInlineObjects() {

		gameObjects = new List<GameObject>();

		Vector3 startingPosition = Positions.startPositionInlineThree;

		foreach (string obj in SceneObjects) {
			gameObjects.Add(candSManager.ActivateObject(obj, positions.GetPosition(startingPosition), Positions.ObjectsRotation));
			startingPosition += new Vector3(0.15f, 0, 0);
		}
	}

}
