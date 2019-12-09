using System.Collections;
using UnityEngine;
using static EventManager;

public class LearningPhaseActivity1 : LearningPhaseManager {

    protected override void StartSpawn() {
        StartCoroutine(Spawn());
    }

    private IEnumerator Spawn() {
        yield return StartCoroutine(ShowObjects(SceneObjects));

        //End the learning phase
        TriggerEvent(Triggers.LearningPhaseEnd);
    }
}
