using System;
using UnityEngine;
using static EventManager;

public class VAManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    //Introduction of VA to the activity
    private void Introduce() {
        //TODO: Play audio instead of Debug.Log
        Debug.Log("Hi! I’m xxx. Today we are going to play together.In this first game I’ll ask you to find some objects and to do some actions with them.First I’ll show you some objects.Let’s start!");

        //Trigger the Learning Phase execution
        TriggerEvent(Triggers.LearningPhaseStart);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
