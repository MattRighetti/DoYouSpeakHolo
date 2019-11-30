using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LearningPhaseManager : MonoBehaviour
{
    private GameObject Key;
    private GameObject House;
    private GameObject Tree;
    private GameObject Apple;

    private List<GameObject> GameObjects;

    private ObjectPooler Pooler;

    Vector3 CentralPosition;

    void Start()
    {
        Pooler = new ObjectPooler();
        Key = Pooler.GetPooledObject("Key");
        House = Pooler.GetPooledObject("House");
        Tree = Pooler.GetPooledObject("Tree");
        Apple = Pooler.GetPooledObject("Apple");
        CentralPosition = new Vector3(0, 0, 2);
        EventManager.StartListening("LearningPhaseStart", HandleStartOfLearningPhase);
        EventManager.StartListening("LearningPhaseSpawn", HandleSpawn);
    }

    //First phase of the activity, the virtual assistant shows to the user some objects and tells their name
    private void HandleStartOfLearningPhase()
    {
        Debug.Log("Start Learning Phase");
        //Trigger the spawn procedure
        EventManager.TriggerEvent("LearningPhaseSpawn");
    }

    //Handler fot the spawn procedure
    private void HandleSpawn()
    {
        StartCoroutine(ShowObjects());
    }

    //Stop listening to events and trigger the checking phase
    private void End()
    {
        EventManager.StopListening("LearningPhaseStart", HandleStartOfLearningPhase);
        EventManager.StopListening("LearningPhaseSpawn", HandleSpawn);

        //start the checking phase
        //TODO: find a better way to call the method
        Debug.Log("Triggering CheckingPhase");
        EventManager.TriggerEvent("CheckingPhase");
    }

    //Spawn the objects
    IEnumerator ShowObjects()
    {
        foreach (GameObject Obj in GameObjects)
        {
            StartCoroutine(ShowObject(Obj));
            yield return new WaitForSeconds(3);
        }

        //End the activity
        End();
    }

    //Spawn the objects in front of the user and destroy them after a timeout
    IEnumerator ShowObject(GameObject obj)
    {
        GameObject objectToCreate = Instantiate(obj) as GameObject;
        objectToCreate.transform.position = CentralPosition;
        yield return new WaitForSeconds(2);
        Destroy(objectToCreate);
    }

}
