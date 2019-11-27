using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsEnum : MonoBehaviour
{

    List<GameObject> ObjectsList;
    Dictionary<string, string> ObjectsEnumeration;
    // Start is called before the first frame update
    void Start()
    {
        ObjectsEnumeration = new Dictionary<string, string>
        {
            { "House", "Assets/Prefab/MediumHouse.prefab" },
            { "Key", "Assets/Prefab/MediumKey.prefab" },
            { "Tree", "Assets/Prefab/Tree_3parts.prefab" },
            { "Apple", "Assets/Prefab/Apple_LowPoly.prefab" }
        };

        ObjectsList = new List<GameObject>();

        foreach(KeyValuePair<string, string> entry in ObjectsEnumeration)
        {
            GameObject obj = (GameObject)Instantiate(Resources.Load(entry.Value));
            obj.gameObject.tag = entry.Key;
            ObjectsList.Add(obj);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Dictionary<string, string> GetDictionary()
    {
        return ObjectsEnumeration;
    }

    public List<GameObject> GetGameObjects()
    {
        return ObjectsList;
    }
}
