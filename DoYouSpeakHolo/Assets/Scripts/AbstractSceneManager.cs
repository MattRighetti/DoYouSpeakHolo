using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class AbstractSceneManager : MonoBehaviour {

    protected ObjectPooler Pooler;
    public SceneObjectsToLoad sceneSettings;
    public AnimateAvatar VirtualAssistant;
    public LearningPhaseManager LearningPhaseManager { get; set; }
    public CheckingPhaseManager CheckingPhaseManager { get; set; }
    public AudioContext AudioContext { get; set; }

    void OnEnable() {
        Pooler = ObjectPooler.GetPooler();
        LoadObjects();
        SetAudioContext();
        StartListening();
        VirtualAssistant = ActivateObject("VA", Positions.VAPosition).GetComponent<AnimateAvatar>();
    }

    //The VA introduces the activity
    //Triggers the method AnimateAvatar.PlayIntroduction
    public void StartIntroduction() {
        VirtualAssistant.PlayIntroduction();
    }

    //Start the Learning phase
    protected void StartLearningPhase() {
        LearningPhaseManager.StartLearningPhase();
    }

    //Start the Checking Phase
    protected void StartCheckingPhase() {
        CheckingPhaseManager.StartCheckingPhase();
    }

    protected void EndActivity() {
        //TODO after the beta
        //Check if there are more levels to load;

        StopListening();

        //For the demo load the menu
        SceneManager.LoadScene("Menu");

    }

    internal IEnumerator IntroduceObject(string objectToIntroduce) {
        Debug.Log("VA is going to introduce the object");
        yield return VirtualAssistant.IntroduceObject(AudioContext, objectToIntroduce);
    }

    public GameObject ActivateObject(string key, Vector3 position) {
        return Pooler.ActivateObject(key, position);
    }

    public void DeactivateObject(string key) {
        Pooler.DeactivateObject(key);
    }

    public GameObject GetPooledObject(string key) {
        return Pooler.GetPooledObject(key);
    }

    public List<string> GetObjects() {
        return Pooler.GetDynamicObjects();
    }

    private void StartListening() {
        EventManager.StartListening(EventManager.Triggers.VAIntroductionEnd, StartLearningPhase);
        EventManager.StartListening(EventManager.Triggers.LearningPhaseEnd, StartCheckingPhase);
        EventManager.StartListening(EventManager.Triggers.CheckingPhaseEnd, EndActivity);
        StartListeningToCustomEvents();
    }

    private void StopListening() {
        EventManager.StopListening(EventManager.Triggers.VAIntroductionEnd, StartLearningPhase);
        EventManager.StopListening(EventManager.Triggers.LearningPhaseEnd, StartCheckingPhase);
        EventManager.StopListening(EventManager.Triggers.CheckingPhaseEnd, EndActivity);
        StopListeningToCustomEvents();
    }

    //Randomize a List
    public static List<string> Shuffle(List<string> list) {
        List<string> randomList = new List<string>();

        System.Random random = new System.Random();
        int randomIndex = 0;

        while (list.Count > 0) {
            randomIndex = random.Next(0, list.Count);
            randomList.Add(list[randomIndex]);
            list.RemoveAt(randomIndex);
        }

        return randomList;
    }

    // -------------------------- ABSTRACT --------------------------------

    public abstract void LoadObjects();

    public abstract void SetAudioContext();

    public abstract void StartListeningToCustomEvents();
    
    public abstract void StopListeningToCustomEvents();
    
}

//Typesafe Enum pattern to do the audio selection
public class Scenes {
    private Scenes(string value) { Value = value; }

    public string Value { get; set; }

    public static Scenes Scene1 { get { return new Scenes("Scene1"); } }
    public static Scenes Scene2 { get { return new Scenes("Scene2"); } }
    public static Scenes Scene3 { get { return new Scenes("Scene3"); } }
}
