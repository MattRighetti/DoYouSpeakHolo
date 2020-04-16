using UnityEngine;
using static EventManager;

public class FindObjectTask : MonoBehaviour {

    public bool IsTarget = false;

    internal void Check() {
        if (IsTarget) {
            //Ok + VA reaction
            IsTarget = false;
            TriggerEvent(Triggers.VAOk);
            TriggerEvent(Triggers.FoundObject);
        } else {
            //Va no
            TriggerEvent(Triggers.VAKo);
        }
    }
}