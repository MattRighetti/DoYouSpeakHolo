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
    public Dictionary<string, Dictionary<string, GameObject>> pooledObjectsDictionary;
    private Vector3 hiddenPosition;

    void Awake() {
        SharedInstance = this;
        hiddenPosition = new Vector3(0, 0, -3);
        Setup();
    }

    void Setup() {
        pooledObjectsDictionary = new Dictionary<string, Dictionary<string, GameObject>>();
        CreateObjects();
    }

    //Create the objects, deactivate and store them into the data structure
    private void CreateObjects() {
        ObjectsEnum objects = ReadJSONFromFile();
        Dictionary<string, Dictionary<string, string>> dict = objects.GetDictionary();

        foreach (KeyValuePair<string, Dictionary<string, string>> outer_entry in dict) {
            Dictionary<string, GameObject> internal_dict = new Dictionary<string, GameObject>();
            foreach (KeyValuePair<string, string> inner_entry in outer_entry.Value) {
                GameObject obj = Instantiate(Resources.Load(inner_entry.Value, typeof(GameObject))) as GameObject;
                obj.SetActive(false);
                internal_dict.Add(inner_entry.Key, obj);
            }
            pooledObjectsDictionary.Add(outer_entry.Key, internal_dict);
        }
    }

    public List<string> GetObjectsByCategory(string category) {
        Dictionary<string, GameObject> dict = new Dictionary<string, GameObject>();
        if (pooledObjectsDictionary.TryGetValue(category, out dict)) {
            return new List<string>(dict.Keys);
        }
        return null;
    }


    public List<string> GetObjectCategories() {
        List<string> keyValues = new List<string>(pooledObjectsDictionary.Keys);
        return keyValues;
    }

    public GameObject GetPooledObject(string key) {
        GameObject obj;
        foreach (KeyValuePair<string, Dictionary<string, GameObject>> outer_entry in pooledObjectsDictionary) {
            if (outer_entry.Value.TryGetValue(key, out obj)) {
                return obj;

            }
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

    internal GameObject ActivateObject(string objKey, Vector3 centralPosition) {
        GameObject objectToCreate = GetPooledObject(objKey);
        objectToCreate.transform.position = centralPosition;
        objectToCreate.SetActive(true);
        return objectToCreate;
    }

    internal void DeactivateObject(string objKey) {
        GameObject objectToCreate = GetPooledObject(objKey);
        objectToCreate.transform.position = hiddenPosition;
        objectToCreate.SetActive(false);
    }
}
