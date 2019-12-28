using Microsoft.MixedReality.Toolkit.Input;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.WSA.Input;

public class SceneStarter : MonoBehaviour {

    private GameObject tapButton;
    

    public void StartActivity() {
        Debug.Log("Tapped");
        Destroy(tapButton);
        StartCoroutine(StartActivityCoroutine());
    }

    private IEnumerator StartActivityCoroutine() {
        yield return new WaitForSeconds(2);
        GetComponent<AbstractSceneManager>().ConfigureScene();
        GetComponent<LearningPhaseManager>().Setup();
        GetComponent<CheckingPhaseManager>().Setup();
        GetComponent<AbstractSceneManager>().StartIntroduction();
    }

    internal void WaitForUserTap() {
        //Load the new button
        tapButton =(GameObject) Instantiate(Resources.Load("Prefab/Buttons/TapButton"));
    }
}
