using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvestingLogic : MonoBehaviour
{

    private int fruitCounter = 0;
    private string expectedFruit;


    // Start is called before the first frame update
    void Start()
    {
        expectedFruit = "apple";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        //Check whether the gameObject is the one that needs to be put in the basket
        if (other.gameObject.tag == this.expectedFruit )
        {
            fruitCounter++;
            Debug.Log("Counting " + fruitCounter);
        }

    }

    void SetExpectedFruit(string expectedFruit)
    {
        this.expectedFruit = expectedFruit;
    }

}
