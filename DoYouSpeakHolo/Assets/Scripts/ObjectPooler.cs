using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ObjectPooler : MonoBehaviour {

    public const string Objects = "objects";
    public const string Animals = "animals";
    public const string Fruits = "fruits";
    public const string People = "people";

    public static ObjectPooler SharedInstance;
    public Dictionary<string, GameObject> pooledObjectsDictionary;
    private Vector3 hiddenPosition;


    public enum ScenesEnum { Scene1, Scene2, Scene3 };
    public ScenesEnum Scene = ScenesEnum.Scene1;

    void Awake() {
        SharedInstance = this;
        hiddenPosition = new Vector3(0, 0, -3);
        Setup();
    }

    void Setup() {
        pooledObjectsDictionary = new  Dictionary<string, GameObject>();
        //Get the objects from the pooler depending on the scene
        switch (Scene) {
            case ScenesEnum.Scene1:
                CreateObjects(Animals);
                break;
            case ScenesEnum.Scene2:
                CreateObjects(Animals);
                break;
            case ScenesEnum.Scene3:
                CreateObjects(Fruits);
                break;
        }
    }

    //Create the objects, deactivate and store them into the data structure
    private void CreateObjects(string category) {
        ObjectsEnum objects = ReadJSONFromFile();
        Dictionary<string, Dictionary<string, string>> dict = objects.GetDictionary();

        foreach (KeyValuePair<string, Dictionary<string, string>> outer_entry in dict) {
            if(Equals(outer_entry.Key, category)){
                foreach (KeyValuePair<string, string> inner_entry in outer_entry.Value) {
                    Debug.Log("creating " + inner_entry.Key);
                    GameObject obj = Instantiate(Resources.Load(inner_entry.Value, typeof(GameObject))) as GameObject;
                    obj.SetActive(false);
                    pooledObjectsDictionary.Add(inner_entry.Key, obj);
                }
            }
            
        }
    }

    internal List<string> GetObjects() {
        return new List<string>(pooledObjectsDictionary.Keys);
    }

    public GameObject GetPooledObject(string key) {
        GameObject obj;
        if (pooledObjectsDictionary.TryGetValue(key, out obj)) {
            return obj;
        }
        return null;
    }

    //Read and parse the JSON file
    private ObjectsEnum ReadJSONFromFile() {
        //Load the objects from JSON
        string path = @"Assets/Resources/Prefab/objects.json";
        Debug.Log("Reading file at " + path);
        string jsonToParse = File.ReadAllText(path);
        Debug.Log("Json to parse " + jsonToParse);
        return JsonConvert.DeserializeObject<ObjectsEnum>(jsonToParse);
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
        objectToCreate.transform.position = hiddenPosition;
        objectToCreate.SetActive(false);
    }
}
