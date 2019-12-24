using UnityEngine;

public class SceneStarter : MonoBehaviour {
    //Start the learning phase at the first frame and then destroy the object itself
    void Update() {
        GetComponent<AbstractSceneManager>().ConfigureScene();
        GetComponent<LearningPhaseManager>().Setup();
        GetComponent<CheckingPhaseManager>().Setup();
        GetComponent<AbstractSceneManager>().StartIntroduction();
        Destroy(this);
    }
}
