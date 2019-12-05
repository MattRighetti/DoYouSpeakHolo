using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractSceneManager : MonoBehaviour {

    private ObjectPooler Pooler;

    // Start is called before the first frame update
    void Start() {
        Pooler = ObjectPooler.GetPooler();
    }

    public abstract void StartLearningPhase();
    
    // Update is called once per frame
    void Update() {
    
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
}
