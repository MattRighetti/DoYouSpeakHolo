using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ObjectPooler : MonoBehaviour {
    public static ObjectPooler SharedInstance;
    public Dictionary<string, GameObject> pooledObjectsDictionary;
    public int amountToPool;

    void Awake() {
        SharedInstance = this;
        setup();

    }

    // Start is called before the first frame update
    void Start() {
       
    }

    void setup() {
        pooledObjectsDictionary = new Dictionary<string, GameObject>();
        CreateObjects();
        Debug.Log("Testing");
        foreach (string key in GetAllKeyInObjectDictionary()) {
            Debug.Log(string.Format("Key={0}", key));
        }

        foreach (GameObject obj in GetAllValuesInObjectDictionary()) {
            Debug.Log(string.Format("Object={0}", obj.name));
        }

        if (GetPooledObject("medium_house") != null) {
            Debug.Log("Retrieved!");
        }
    }

    private void CreateObjects() {
        ObjectsEnum objects = ReadJSONFromFile();
        Dictionary<string, string> dict = objects.GetDictionary();

        foreach (KeyValuePair<string, string> entry in dict) {
            GameObject obj = Instantiate(Resources.Load(entry.Value, typeof(GameObject))) as GameObject;
            obj.SetActive(false);
            pooledObjectsDictionary.Add(entry.Key, obj);
            amountToPool++;
        }
    }

    public List<string> GetAllKeyInObjectDictionary() {
        List<string> keyValues = new List<string>(pooledObjectsDictionary.Keys);
        return keyValues;
    }

    public List<GameObject> GetAllValuesInObjectDictionary() {
        List<GameObject> values = new List<GameObject>(pooledObjectsDictionary.Values);
        return values;
    }

    public GameObject GetPooledObject(string key) {
        try {
            GameObject obj = pooledObjectsDictionary[key];
            return obj;
        }
        catch (KeyNotFoundException) {
            Console.WriteLine("Dictionary doesn't have Key={0}", key);
            return null;
        }
      
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

}
