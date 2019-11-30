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

    public enum ScenesEnum { Scene1, Scene2, Scene3 };
    public ScenesEnum Scene;
    Vector3 CentralPosition;

    void Start()
    {
        
    }

    void setup() {

        Pooler = gameObject.AddComponent<ObjectPooler>();
        Key = Pooler.GetPooledObject("medium_key");
        House = Pooler.GetPooledObject("medium_house");
        Tree = Pooler.GetPooledObject("tree");
        Apple = Pooler.GetPooledObject("apple");
        CentralPosition = new Vector3(0, 0, 2);
        EventManager.StartListening("LearningPhaseStart", HandleStartOfLearningPhase);
        EventManager.StartListening("LearningPhaseSingleSpawn", HandleSpawn);
        EventManager.StartListening("LearningPhasePairSpawn", HandleSpawnPairs);
    }

    //First phase of the activity, the virtual assistant shows to the user some objects and tells their name
    private void HandleStartOfLearningPhase()
    {
        Debug.Log("Start Learning Phase");
        //Trigger the spawn procedure
        EventManager.TriggerEvent("LearningPhaseSpawn");
    }

    internal void SetScene(ScenesEnum scene)
    {
        Scene = scene;
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


        //Trigger the spawning of the object pairs
        EventManager.TriggerEvent("LearningPhasePairSpawn");
    }

    //Spawn the objects in front of the user and destroy them after a timeout
    IEnumerator ShowObject(GameObject obj)
    {
        GameObject objectToCreate = Instantiate(obj) as GameObject;
        objectToCreate.transform.position = CentralPosition;
        yield return new WaitForSeconds(2);
        Destroy(objectToCreate);
    }

    void HandleSpawnPairs()
    { 

        //End the activity
        End();
    }
}
