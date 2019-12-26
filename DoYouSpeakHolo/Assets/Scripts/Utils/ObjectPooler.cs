using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour {
    public static ObjectPooler SharedInstance;

    private Dictionary<string, GameObject> staticObjectsDictionary;
    private Dictionary<string, GameObject> dynamicObjectsDictionary;
    public Positions Positions;

    void Awake() {
        SharedInstance = this;
        staticObjectsDictionary = new Dictionary<string, GameObject>();
        dynamicObjectsDictionary = new Dictionary<string, GameObject>();
        Positions = new Positions();
    }

    public static ObjectPooler GetPooler() {
        GameObject obj = GameObject.Find("Pooler");
        return obj.GetComponent<ObjectPooler>();
    }

    internal void FindFloor() {
        Positions.FindFloor();
    }

    //Create the objects, deactivate and store them into the data structure
    public void CreateStaticObjects(Dictionary<string, string> objectsDict) {
        if (staticObjectsDictionary == null) {
            staticObjectsDictionary = new Dictionary<string, GameObject>();
        }
        foreach (KeyValuePair<string, string> keyValuePair in objectsDict) {
        
            Debug.Log(keyValuePair);
            GameObject obj = Instantiate(Resources.Load(keyValuePair.Value, typeof(GameObject))) as GameObject;
            obj.transform.position = Positions.hiddenPosition;
            obj.SetActive(false);

            staticObjectsDictionary.Add(keyValuePair.Key, obj);

           
        }
    }

    //Create the objects, deactivate and store them into the data structure
    public void CreateDynamicObjects(Dictionary<string, string> objectsDict) {
        if (dynamicObjectsDictionary == null) {
            dynamicObjectsDictionary = new Dictionary<string, GameObject>();
        }
        foreach (KeyValuePair<string, string> keyValuePair in objectsDict) {
            Debug.Log(keyValuePair);
            GameObject obj = Instantiate(Resources.Load(keyValuePair.Value, typeof(GameObject))) as GameObject;
            obj.transform.position = Positions.hiddenPosition;
            obj.SetActive(false);


            dynamicObjectsDictionary.Add(keyValuePair.Key, obj);
            
        }
    }

    internal List<string> GetStaticObjects() {
        return new List<string>(staticObjectsDictionary.Keys);
    }

    internal List<string> GetDynamicObjects() {
        return new List<string>(dynamicObjectsDictionary.Keys);
    }

    public GameObject GetPooledObject(string key) {
        if (dynamicObjectsDictionary.TryGetValue(key, out GameObject dobj)) {
            return dobj;
        }
        if (staticObjectsDictionary.TryGetValue(key, out GameObject sobj)) {
            return sobj;
        }

        return null;
    }

    public GameObject ActivateObject(string objKey, Vector3 position) { 
        GameObject objectToCreate = GetPooledObject(objKey);
        objectToCreate.transform.position = Positions.GetPosition(position);
        objectToCreate.name = objKey;
        objectToCreate.SetActive(true);
        return objectToCreate;
    }

    public void DeactivateObject(string objKey) {
        GameObject objectToCreate = GetPooledObject(objKey);
        objectToCreate.transform.position = Positions.hiddenPosition;
        objectToCreate.SetActive(false);
    }
}
