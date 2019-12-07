using UnityEngine;

public class SceneStarter : MonoBehaviour {
    //Start the learning phase at the first frame and then destroy the object itself
    void Update() {
        GetComponent<AbstractSceneManager>().StartIntroduction();
        Destroy(this);
    }
}
