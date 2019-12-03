using UnityEngine;

public class CandSManager : MonoBehaviour {


    // Start is called before the first frame update
    void Start() {
        //Set the current scene to 1 in the LearningPhaseManager
        gameObject.GetComponent<LearningPhaseManager>().SetScene(LearningPhaseManager.ScenesEnum.Scene1);
    }

    public void StartLearningPhase() {
        EventManager.TriggerEvent("LearningPhaseStart");
    }

    // Update is called once per frame
    void Update()
    {

    }
}

