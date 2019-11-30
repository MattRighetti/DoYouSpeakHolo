using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvestingLogic : MonoBehaviour
{
    
    private int FruitCounter = 0;
    private string ExpectedFruit;
    private Collider Chest;

    public HarvestingLogic(string Fruit)
    {
        this.ExpectedFruit = Fruit;
    }


    // Start is called before the first frame update
    void Start()
    {
        //ExpectedFruit = "Apple";
        ExpectedFruit = Chest.gameObject.tag;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider Chest)
    {
        //Check whether the gameObject is the one that needs to be put in the basket
        if (Chest.gameObject.tag == this.ExpectedFruit )
        {
            EventManager.TriggerEvent("PickedFruitEvent");
        }
        

    }

    void SetExpectedFruit(string ExpectedFruit)
    {
        this.ExpectedFruit = ExpectedFruit;
    }

}
