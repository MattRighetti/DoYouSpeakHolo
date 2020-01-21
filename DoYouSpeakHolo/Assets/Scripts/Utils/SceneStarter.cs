using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using System.Collections;
using UnityEngine;

// After the room scan, invite the user to tap on a surface detected by the Spatial Processing to start the activity
public class SceneStarter : MonoBehaviour, IMixedRealityPointerHandler, IMixedRealitySourceStateHandler {

    private GameObject tapButton;

    private void OnEnable() {
        // Instruct Input System that we would like to receive all input events of type
        // IMixedRealitySourceStateHandler and IMixedRealityHandJointHandler
        CoreServices.InputSystem?.RegisterHandler<IMixedRealitySourceStateHandler>(this);
        CoreServices.InputSystem?.RegisterHandler<IMixedRealityPointerHandler>(this);
    }

    private void UnregisterToGlobalEvents() {
        // This component is being destroyed
        // Instruct the Input System to disregard us for input event handling
        CoreServices.InputSystem?.UnregisterHandler<IMixedRealitySourceStateHandler>(this);
        CoreServices.InputSystem?.UnregisterHandler<IMixedRealityPointerHandler>(this);
    }

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
        UnregisterToGlobalEvents();
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

    public void OnSourceDetected(SourceStateEventData eventData) {
        //throw new System.NotImplementedException();
    }

    public void OnSourceLost(SourceStateEventData eventData) {
        //throw new System.NotImplementedException();
    }

    public void OnPointerDown(MixedRealityPointerEventData eventData) {
        //throw new System.NotImplementedException();
    }

    public void OnPointerDragged(MixedRealityPointerEventData eventData) {
        //throw new System.NotImplementedException();
    }

    public void OnPointerUp(MixedRealityPointerEventData eventData) {
        //throw new System.NotImplementedException();
    }

    public void OnPointerClicked(MixedRealityPointerEventData eventData) {
        StartActivity();
    }
}
