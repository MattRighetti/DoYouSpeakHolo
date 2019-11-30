using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PossessivesManager : MonoBehaviour
{
    private int counter = 0;

    // Start is called before the first frame update
    void Start()
    {
        EventManager.StartListening("PickedFruitEvent", CountFruits);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CountFruits()
    {
        counter++;
    }
}
