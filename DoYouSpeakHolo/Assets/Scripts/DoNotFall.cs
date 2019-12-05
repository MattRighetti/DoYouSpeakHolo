using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoNotFall : MonoBehaviour {
    private Collider coll;
    private GravityEnabler gravity;

    // Start is called before the first frame update
    void Start() {
        gravity = gameObject.AddComponent<GravityEnabler>();
        coll = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update() {
        if (gameObject.transform.position.y < 0) {
            Vector3 temp = new Vector3(0, -gameObject.transform.position.y, 0);
            gameObject.transform.position += temp;
        }

        if (gameObject.transform.position.y == 0) {
            gravity.coll = coll;
            gravity.DisableGravity();
        }
    }
}
