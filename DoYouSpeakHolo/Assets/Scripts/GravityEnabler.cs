using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityEnabler : MonoBehaviour
{

    public Collider Coll;

    void Start()
    {
        Coll = gameObject.GetComponent<Collider>();
    }

    public void EnableGravity()
    {
        Coll.attachedRigidbody.useGravity = true;
        Coll.attachedRigidbody.isKinematic = false;
    }

    public void DisableGravity()
    {
        Coll.attachedRigidbody.useGravity = false;
        Coll.attachedRigidbody.isKinematic = true;
    }
}
