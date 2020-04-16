using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class EventManager : MonoBehaviour
{

    public enum Triggers {
        VAIntroduce,
        LearningPhaseStart,
        LearningPhaseSpawn,
        PickedFruit,
        VAOk,
        VAKo,
        FoundObject,
        LearningPhaseEnd,
        CheckingPhaseEnd,
        CheckingPhaseStart,
        LoadingComplete,
        VAIntroductionEnd,
        BasketEmpty,
        SetTargetObject,
        GuidedHarvesting,
        PickedAnimal,
        ObjectPositioning,
        CorrectPositioning
    };

    Dictionary<Triggers, UnityEvent> eventDictionary;

    static EventManager eventManager;

    public static EventManager instance
    {
        get
        {
            if (!eventManager)
            {
                eventManager = FindObjectOfType(typeof(EventManager)) as EventManager;

                if (!eventManager)
                {
                    Debug.LogError("There needs to be one active EventManger script on a GameObject in your scene.");
                }
                else
                {
                    eventManager.Init();
                }
            }

            return eventManager;
        }
    }

    void Init()
    {
        if (eventDictionary == null)
        {
            eventDictionary = new Dictionary<Triggers, UnityEvent>();
        }
    }

    public static void StartListening(Triggers eventName, UnityAction listener)
    {
        UnityEvent thisEvent = null;
        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else
        {
            thisEvent = new UnityEvent();
            thisEvent.AddListener(listener);
            instance.eventDictionary.Add(eventName, thisEvent);
        }
    }

    public static void StopListening(Triggers eventName, UnityAction listener)
    {
        if (eventManager == null) return;
        UnityEvent thisEvent = null;
        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }

    public static void TriggerEvent(Triggers eventName)
    {
        UnityEvent thisEvent = null;
        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke();
        }
    }
}

