using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractSceneManager : MonoBehaviour {

    protected ObjectPooler Pooler;
    public SceneSettings sceneSettings;

    // Start is called before the first frame update
    void Start() {
        Pooler = ObjectPooler.GetPooler();
        LoadObjects();
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
        return Pooler.GetObjects();
    }


    // -------------------------- ABSTRACT --------------------------------

    public abstract void LoadObjects();

    public abstract void StartLearningPhase();

}
