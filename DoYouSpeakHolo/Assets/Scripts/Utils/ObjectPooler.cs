using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour {
    public static ObjectPooler SharedInstance;

    private Dictionary<string, GameObject> staticObjectsDictionary;
    private Dictionary<string, GameObject> dynamicObjectsDictionary;

    void Awake() {
        SharedInstance = this;
        staticObjectsDictionary = new Dictionary<string, GameObject>();
        dynamicObjectsDictionary = new Dictionary<string, GameObject>();
    }

    public static ObjectPooler GetPooler() {
        GameObject obj = GameObject.Find("Pooler");
        return obj.GetComponent<ObjectPooler>();
    }

    //Create the objects, deactivate and store them into the data structure
    public void CreateStaticObjects(List<SingleObjectToLoad> staticObjects) {
        foreach (SingleObjectToLoad objectString in staticObjects) {
            Debug.Log(objectString);
            GameObject obj = Instantiate(Resources.Load(objectString.path, typeof(GameObject))) as GameObject;
            obj.transform.position = Positions.hiddenPosition;
            obj.SetActive(false);

            staticObjectsDictionary.Add(objectString.type, obj);
        }
    }

    //Create the objects, deactivate and store them into the data structure
    public void CreateDynamicObjects(List<SingleObjectToLoad> dynamicObjects) {
        foreach (SingleObjectToLoad objectString in dynamicObjects) {
            Debug.Log(objectString);
            GameObject obj = Instantiate(Resources.Load(objectString.path, typeof(GameObject))) as GameObject;
            obj.transform.position = Positions.hiddenPosition;
            obj.SetActive(false);

            dynamicObjectsDictionary.Add(objectString.type, obj);
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
