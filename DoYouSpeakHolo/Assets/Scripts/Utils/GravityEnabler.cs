using Microsoft.MixedReality.Toolkit.UI;
using UnityEngine;

public class GravityEnabler : MonoBehaviour
{
    public Rigidbody rigidbody;
    //public ManipulationHandler man;

    void Start()
    {
        rigidbody = gameObject.GetComponent<Rigidbody>();
        //man = gameObject.GetComponent<ManipulationHandler>();
        //man.OnManipulationEnded.AddListener(FallDown);
    }

    private void FallDown(ManipulationEventData arg0) {
        EnableGravity();
    }
    
    public void EnableGravity()
    {
        rigidbody.useGravity = true;
        rigidbody.isKinematic = false;
    }

    public void DisableGravity()
    {
        rigidbody.useGravity = false;
        rigidbody.isKinematic = true;  
    }
    
}
