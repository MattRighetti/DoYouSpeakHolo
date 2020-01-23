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
        SceneIntroduction();
	}

	private void SceneIntroduction() {
		//yield return ShowObjects(SceneObjects);
		DisplayInlineObjects();
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
