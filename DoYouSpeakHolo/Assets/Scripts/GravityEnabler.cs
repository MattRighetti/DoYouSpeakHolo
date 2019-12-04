using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityEnabler : MonoBehaviour
{

    public Collider coll;

    void Start()
    {
        coll = gameObject.GetComponent<Collider>();
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
