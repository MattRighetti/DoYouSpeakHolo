using System.Collections;


public class LearningPhaseActivity1 : LearningPhaseManager {

    CandSManager candSManager;

    protected override void LearningPhase() {
        candSManager = (CandSManager)sceneManager;
        StartCoroutine(Spawn());
    }

    private IEnumerator Spawn() {
        yield return StartCoroutine(ShowObjectsWithContext(SceneObjects));

        //End the learning phase
        End();
    }
}
