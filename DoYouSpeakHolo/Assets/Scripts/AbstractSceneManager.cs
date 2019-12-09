using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class AbstractSceneManager : MonoBehaviour {

    protected ObjectPooler Pooler;
    public SceneSettings sceneSettings;
    public AnimateAvatar VirtualAssistant;

    void OnEnable() {
        Pooler = ObjectPooler.GetPooler();
        LoadObjects();
        StartListening();
        VirtualAssistant = ActivateObject("VA", Positions.VAPosition).GetComponent<AnimateAvatar>();
    }

    private void StartListening() {
        EventManager.StartListening(EventManager.Triggers.VAIntroductionEnd, LearningPhaseStart);
        EventManager.StartListening(EventManager.Triggers.LearningPhaseEnd, LearningPhaseEnd);
        EventManager.StartListening(EventManager.Triggers.CheckingPhaseEnd, CheckingPhaseEnd);
    }

    //The VA introduces the activity
    //Triggers the method AnimateAvatar.PlayIntroduction
    public void StartIntroduction() {
        VirtualAssistant.PlayIntroduction();
    }

    //Start the learning phase
    //Triggers the event LearningPhaseManager.StartLearningPhase()
    private void LearningPhaseStart() {
        EventManager.TriggerEvent(EventManager.Triggers.LearningPhaseStart);
    }

    private void CheckingPhaseEnd() {
        //Check if there are more levels to load;
        throw new NotImplementedException();
    }

    private void LearningPhaseEnd() {
        EventManager.TriggerEvent(EventManager.Triggers.CheckingPhaseStart);
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

    protected void EndActivity() {
        SceneManager.LoadScene("Menu");
    }

    // -------------------------- ABSTRACT --------------------------------

    public abstract void LoadObjects();

    public abstract void StartLearningPhase();

    public abstract void IntroduceObject(string objKey);

}
