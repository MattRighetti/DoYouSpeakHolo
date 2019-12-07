using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractSceneManager : MonoBehaviour {

    protected ObjectPooler Pooler;
    public SceneSettings sceneSettings;

    // Start is called before the first frame update
    void OnEnable() {
        Pooler = ObjectPooler.GetPooler();
        LoadObjects();
        StartListening();
    }

    private void StartListening() {
        EventManager.StartListening(EventManager.Triggers.LearningPhaseEnd, LearningPhaseEnd);
        EventManager.StartListening(EventManager.Triggers.CheckingPhaseEnd, CheckingPhaseEnd);
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

    // -------------------------- ABSTRACT --------------------------------

    public abstract void LoadObjects();

    public abstract void StartLearningPhase();

}
