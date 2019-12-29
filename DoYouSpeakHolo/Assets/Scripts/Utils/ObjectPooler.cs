using System.Collections.Generic;
using UnityEngine;

//  Class responsible of active/deactivate objects
public class ObjectPooler : MonoBehaviour {
    public static ObjectPooler SharedInstance;

    //  Static objects (e.g. scene background)
    private Dictionary<string, GameObject> staticObjectsDictionary;

    //  Dynamic objects (everything interacting with the user)
    private Dictionary<string, GameObject> dynamicObjectsDictionary;

    //  Determines the position of the objects
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

    //  Set the floor position
    internal void FindFloor() {
        Positions.FindFloor();
    }

    //Create the objects, deactivate and store them into the data structure
    public void CreateStaticObjects(List<SingleObjectToLoad> staticObjects) {
        foreach (SingleObjectToLoad objectString in staticObjects) {
            GameObject obj = Instantiate(Resources.Load(objectString.path, typeof(GameObject))) as GameObject;
            obj.transform.position = Positions.hiddenPosition;
            obj.SetActive(false);

            staticObjectsDictionary.Add(objectString.type, obj);
        }
    }

    //Create the objects, deactivate and store them into the data structure
    public void CreateDynamicObjects(List<SingleObjectToLoad> dynamicObjects) {
        foreach (SingleObjectToLoad objectString in dynamicObjects) {
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

    //  Activate an object in a given position (it will be adjusted according to the Spatial Mapping scans)
    public GameObject ActivateObject(string objKey, Vector3 position) { 
        GameObject objectToCreate = GetPooledObject(objKey);
        objectToCreate.transform.position = Positions.GetPosition(position);
        objectToCreate.name = objKey;
        objectToCreate.SetActive(true);
        return objectToCreate;
    }

    //  Deactivate an object
    public void DeactivateObject(string objKey) {
        GameObject objectToCreate = GetPooledObject(objKey);
        objectToCreate.transform.position = Positions.hiddenPosition;
        objectToCreate.SetActive(false);
    }
}
