using System;
using System.Collections;
using UnityEngine;

public class LearningPhaseActivity3 : LearningPhaseManager {

    protected override void StartSpawn() {

        StartCoroutine(ShowObjectsWithPossessives());
    }

    private IEnumerator ShowObjectsWithPossessives() {
        yield return new WaitForSeconds(3);
    }
}