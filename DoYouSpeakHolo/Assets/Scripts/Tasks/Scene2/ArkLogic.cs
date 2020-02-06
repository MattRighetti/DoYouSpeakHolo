using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArkLogic : MonoBehaviour
{
    private BoxCollider objectCollider;
    private CandSManager candsManager;
    private string targetAnimal;
    public List<string> AnimalList { get; set; }

    void Start() {
        objectCollider = gameObject.GetComponent<BoxCollider>();
        candsManager = GameObject.Find("SceneManager").GetComponent<CandSManager>();
    }

    public void SetTargetAnimal(string targetAnimal) {
        this.targetAnimal = targetAnimal;
    }

    private void OnTriggerEnter(Collider collider) {
        Debug.Log("TARGET " + targetAnimal + " INCOMING " + collider.gameObject.name);
        //If the collider belongs to a target animal
        if (Equals(collider.gameObject.name, targetAnimal)) {
            //Remove it from the list of target animals
            AnimalList.Remove(collider.gameObject.name);

            collider.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            Destroy(collider.gameObject.GetComponent<ManipulationHandler>());
            Destroy(collider.gameObject.GetComponent<NearInteractionGrabbable>());
            //Trigger the positive reaction of the Virtual assistant
            EventManager.TriggerEvent(EventManager.Triggers.VAOk);
            Wait(3);
        }
        else {
            //Trigger the negative reaction of the Virtual assistant
            EventManager.TriggerEvent(EventManager.Triggers.VAKo);
        }

        void Wait(float seconds) {
            StartCoroutine(_wait(seconds));
        }

        IEnumerator _wait(float time) {
            yield return new WaitForSeconds(time);
            EventManager.TriggerEvent(EventManager.Triggers.PickedAnimal);
        }
    }
}
