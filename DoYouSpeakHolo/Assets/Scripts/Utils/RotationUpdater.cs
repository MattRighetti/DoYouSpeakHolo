using UnityEngine;
using System.Collections;

public class RotationUpdater : MonoBehaviour {
    // Update is called once per frame
    void Update() {
        Vector3 relativePosition = Camera.main.transform.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(relativePosition);
        rotation.x = 0f;
        rotation.z = 0f;
        transform.rotation = rotation;
    }
}
