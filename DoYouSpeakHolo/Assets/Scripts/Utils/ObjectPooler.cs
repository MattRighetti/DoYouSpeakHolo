using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ObjectPooler : MonoBehaviour {
    public static ObjectPooler SharedInstance;

    public Dictionary<string, GameObject> pooledObjectsDictionary;

    void Awake() {
        SharedInstance = this;
        pooledObjectsDictionary = new Dictionary<string, GameObject>();
    }

    public static ObjectPooler GetPooler() {
        GameObject obj = GameObject.Find("Pooler");
        return obj.GetComponent<ObjectPooler>();
    }

    //Create the objects, deactivate and store them into the data structure
    public void CreateObjects(Dictionary<string, string> objectsDict) {
        foreach (KeyValuePair<string, string> keyValuePair in objectsDict) {
            Debug.Log(keyValuePair);
            GameObject obj = Instantiate(Resources.Load(keyValuePair.Value, typeof(GameObject))) as GameObject;
            obj.transform.position = Positions.hiddenPosition;
            obj.SetActive(false);
            pooledObjectsDictionary.Add(keyValuePair.Key, obj);
        }
    }

    internal List<string> GetObjects() {
        return new List<string>(pooledObjectsDictionary.Keys);
    }

    public GameObject GetPooledObject(string key) {
        if (pooledObjectsDictionary.TryGetValue(key, out GameObject obj)) {
            return obj;
        }

        return null;
    }

    public GameObject ActivateObject(string objKey, Vector3 position) {
        GameObject objectToCreate = GetPooledObject(objKey);
        objectToCreate.transform.position = position;
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
