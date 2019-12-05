using System.Collections;
using UnityEngine;

public class LearningPhaseActivity1 : LearningPhaseManager {

    protected override void StartSpawn() {
        StartCoroutine(ShowObjects());
    }

    //Spawn the objects one at time
    protected IEnumerator ShowObjects() {
        foreach (string objectKey in SceneObjects) {
            StartCoroutine(ShowObject(objectKey));
            yield return new WaitForSeconds(3);
        }

        //End of Learning Phase
        End();
    }

}
