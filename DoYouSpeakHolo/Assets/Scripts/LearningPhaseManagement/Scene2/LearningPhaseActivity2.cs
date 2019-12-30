using System;
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
		StartCoroutine(SceneIntroduction());
		positions = new Positions();
	}

	private IEnumerator SceneIntroduction() {
		yield return ShowObjects(SceneObjects);
		DisplayInlineObjects();
		End();
	}

	private void DisplayInlineObjects() {

		gameObjects = new List<GameObject>();

		Vector3 startingPosition = Positions.startPositionInlineFour;

		foreach (string obj in SceneObjects) {
			gameObjects.Add(candSManager.ActivateObject(obj, positions.GetPosition(startingPosition)));
			startingPosition += new Vector3(0.5f, 0, 0);
		}
	}

}
