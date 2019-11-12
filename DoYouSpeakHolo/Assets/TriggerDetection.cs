using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDetection : MonoBehaviour
{

    /// <summary>
    /// Method that gets executed whenever objects touch each other
    /// This script is only attached to Houses
    /// </summary>
    void OnTriggerEnter(Collider other)
    {
        Debug.Log(gameObject.name + " has been triggered by " + other.gameObject.name);

        // Delete the key from the scene if it's triggered the correct house
        if (gameObject.tag == "big_house" && other.gameObject.tag == "big_key")
        {
            Debug.Log("Destroying " + other.gameObject.name);
            Destroy(other.gameObject);
        }

        if (gameObject.tag == "medium_house" && other.gameObject.tag == "medium_key")
        {
            Debug.Log("Destroying " + other.gameObject.name);
            Destroy(other.gameObject);
        }

        if (gameObject.tag == "small_house" && other.gameObject.tag == "small_key")
        {
            Debug.Log("Destroying " + other.gameObject.name);
            Destroy(other.gameObject);
        }

    }

}
