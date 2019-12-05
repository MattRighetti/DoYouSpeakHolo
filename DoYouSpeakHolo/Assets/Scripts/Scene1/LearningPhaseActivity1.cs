using System.Collections;
using UnityEngine;

public class LearningPhaseActivity1 : LearningPhaseManager {

    protected override void StartSpawn() {
        StartCoroutine(ShowObjects(SceneObjects));
    }
}
