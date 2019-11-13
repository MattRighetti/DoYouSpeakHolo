using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoNotFall : MonoBehaviour
{

    public GameObject gameObject;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.transform.position.y < 0)
        {
            Vector3 temp = new Vector3(0, - gameObject.transform.position.y, 0);
            gameObject.transform.position += temp;
            Debug.Log("y coordinate " + gameObject.transform.position.y);
            
        }
    }
}
