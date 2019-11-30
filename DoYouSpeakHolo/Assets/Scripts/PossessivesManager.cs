using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PossessivesManager : MonoBehaviour
{
    private int counter = 0;
    private bool Learning = false;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<LearningPhaseManager>().SetScene(LearningPhaseManager.ScenesEnum.Scene3);
        EventManager.StartListening("PickedFruitEvent", CountFruits);
    }

    // Update is called once per frame
    void Update()
    {

        //TODO: find another way to start the flow of the activity
        if (!Learning)
        {
            EventManager.TriggerEvent("LearningPhaseStart");
            Learning = true;
        }
    }

    private void CountFruits()
    {
        counter++;
    }
}
