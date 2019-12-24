using System.Collections;


public class LearningPhaseActivity1 : LearningPhaseManager {

    CandSManager candSManager;

    protected override void LearningPhase() {
        candSManager = (CandSManager)sceneManager;
        StartCoroutine(SceneIntroduction());
    }

    protected IEnumerator SceneIntroduction() {
        yield return StartCoroutine(ShowObjectsWithContext(SceneObjects));

        //End the learning phase
        End();
    }
}
