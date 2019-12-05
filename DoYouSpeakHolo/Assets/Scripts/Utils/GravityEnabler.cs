using Microsoft.MixedReality.Toolkit.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityEnabler : MonoBehaviour
{
    public Collider coll;
    public ManipulationHandler man;

    void Start()
    {
        coll = gameObject.GetComponent<Collider>();
        man = gameObject.GetComponent<ManipulationHandler>();
        man.OnManipulationEnded.AddListener(FallDown);
    }

    private void FallDown(ManipulationEventData arg0) {
        coll = man.GetComponent<Collider>();
        EnableGravity();
    }

    public void EnableGravity()
    {
        
        coll.attachedRigidbody.useGravity = true;
        coll.attachedRigidbody.isKinematic = false;
    }

    public void DisableGravity()
    {
        coll.attachedRigidbody.useGravity = false;
        coll.attachedRigidbody.isKinematic = true;  
    }
    
}
