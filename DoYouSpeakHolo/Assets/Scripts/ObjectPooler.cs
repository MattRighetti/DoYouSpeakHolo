using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler SharedInstance;
    public List<GameObject> pooledObjects;
    public int amountToPool;

    void Awake()
    {
        SharedInstance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        CreateObjects();

        Debug.Log("Loading Prefabs");
        pooledObjects = new List<GameObject>();
        foreach (GameObject obj in pooledObjects)
        {
            obj.SetActive(false);
            Debug.Log(obj.gameObject.tag);
            pooledObjects.Add(obj);
        }
    }

    private void CreateObjects()
    {
        ObjectsEnum objects = ReadJSONFromFile();
        Debug.Log(objects);
        Dictionary<string, string> dict = objects.GetDictionary();

        foreach (KeyValuePair<string, string> entry in dict)
        {
            Debug.Log("loading " + entry.Value);
            var x = Resources.Load(entry.Value) as GameObject;
            Debug.Log(x);
            GameObject obj = (GameObject)Instantiate(x);
            obj.gameObject.tag = entry.Key;
            obj.SetActive(false);
            pooledObjects.Add(obj);
            amountToPool++;
            Debug.Log("Created " + entry.Key);
        }
    }

    //Read and parse the JSON file
    private ObjectsEnum ReadJSONFromFile()
    {
        //Load the objects from JSON
        string path = @"Assets\Resources\Prefab\objects.json";
        Debug.Log("Reading file at " + path);
        string jsonToParse = File.ReadAllText(path);
        Debug.Log("Json to parse " + jsonToParse);
        return JsonConvert.DeserializeObject<ObjectsEnum>(jsonToParse);
    }

    public GameObject GetPooledObject(string tag)
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy && tag == pooledObjects[i].tag)
            {
                Debug.Log("Returning " + tag);
                return pooledObjects[i];
            }
        }
        return null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
