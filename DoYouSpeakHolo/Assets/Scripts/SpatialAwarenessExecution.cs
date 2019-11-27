using Microsoft.MixedReality.Toolkit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpatialAwarenessExecution : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ExecuteSAAfterTime(10));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Enables the Observers for 10 seconds, then it turn them off
    IEnumerator ExecuteSAAfterTime(float time)
    {
        // Resume Mesh Observation from all Observers
        CoreServices.SpatialAwarenessSystem.ResumeObservers();

        //Wait 10 seconds before stopping the Observers
        yield return new WaitForSeconds(time);

        // Suspend Mesh Observation from all Observers
        CoreServices.SpatialAwarenessSystem.SuspendObservers();
    }
}
