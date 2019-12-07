using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractSceneManager : MonoBehaviour {

    protected ObjectPooler Pooler;
    public SceneSettings sceneSettings;
    public AnimateAvatar VirtualAssistant;

    // Start is called before the first frame update
    void OnEnable() {
        Pooler = ObjectPooler.GetPooler();
        LoadObjects();
        StartListening();
        VirtualAssistant = GameObject.Find("VA").GetComponent<AnimateAvatar>();
    }

    private void StartListening() {
        EventManager.StartListening(EventManager.Triggers.VAIntroductionEnd, LearningPhaseStart);
        EventManager.StartListening(EventManager.Triggers.LearningPhaseEnd, LearningPhaseEnd);
        EventManager.StartListening(EventManager.Triggers.CheckingPhaseEnd, CheckingPhaseEnd);
    }

    //The VA introduces the activity
    //Triggers the method AnimateAvatar.PlayIntroduction
    public void StartIntroduction() {
        EventManager.TriggerEvent(EventManager.Triggers.VAIntroduce);
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

    internal void IntroduceObject(string objKey) {
        VirtualAssistant.IntroduceObject(Pooler.GetPooledObject(objKey));
    }

    public GameObject GetPooledObject(string key) {
        return Pooler.GetPooledObject(key);
    }

    public List<string> GetObjects() {
        return Pooler.GetDynamicObjects();
    }

    // -------------------------- ABSTRACT --------------------------------

    public abstract void LoadObjects();

    public abstract void StartLearningPhase();

}
