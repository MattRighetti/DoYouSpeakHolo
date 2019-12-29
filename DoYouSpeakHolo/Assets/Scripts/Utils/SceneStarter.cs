using System.Collections;
using UnityEngine;

// After the room scan, invite the user to tap on a surface detected by the Spatial Processing to start the activity
public class SceneStarter : MonoBehaviour {

    private GameObject tapButton;

    //  Tap Gesture Handler. Destroys the button and starts the StartActivity Coroutine
    public void StartActivity() {
        Debug.Log("Tapped");
        Destroy(tapButton);
        StartCoroutine(StartActivityCoroutine());
    }

    //  Configure:
    //      - Scene Manager
    //      - Learning Phase Manager
    //      - Checking Phase Manager
    //  Start the Activity
    private IEnumerator StartActivityCoroutine() {
        yield return new WaitForSeconds(2);
        GetComponent<AbstractSceneManager>().ConfigureScene();
        GetComponent<LearningPhaseManager>().Setup();
        GetComponent<CheckingPhaseManager>().Setup();
        GetComponent<AbstractSceneManager>().StartIntroduction();
    }

    // Display a button 
    internal void WaitForUserTap() {
        //Load the new button
        tapButton =(GameObject) Instantiate(Resources.Load("Prefab/Buttons/TapButton"));
    }
}
