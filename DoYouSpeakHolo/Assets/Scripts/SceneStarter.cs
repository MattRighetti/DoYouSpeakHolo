using System.Collections;
using UnityEngine;

public class SceneStarter : MonoBehaviour {

    public bool ready = false;

    //Start the learning phase at the first frame and then destroy the object itself
    void Update() {
        //        GetComponent<AbstractSceneManager>().ConfigureScene();
        //        GetComponent<LearningPhaseManager>().Setup();
        //        GetComponent<CheckingPhaseManager>().Setup();
        //        GetComponent<AbstractSceneManager>().StartIntroduction();
        //        Destroy(this);
    }



    private IEnumerator Wait() {
        yield return new WaitForSeconds(3);
        ready = true;





        GameObject.Find("PossessivesManager").GetComponent<AbstractSceneManager>().ConfigureScene();
        GameObject.Find("PossessivesManager").GetComponent<LearningPhaseManager>().Setup();
        GameObject.Find("PossessivesManager").GetComponent<CheckingPhaseManager>().Setup();
        GameObject.Find("PossessivesManager").GetComponent<AbstractSceneManager>().StartIntroduction();
    }
}
