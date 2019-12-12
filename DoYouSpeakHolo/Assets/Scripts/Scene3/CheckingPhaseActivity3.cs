
using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckingPhaseActivity3 : CheckingPhaseManager
{
    private AudioContext3 audioContext;
    private PossessivesManager possessivesManager;
    private List<string> maleObjects;
    private List<string> femaleObjects;
    private int maleObjectsDone = 0;
    private int femaleObjectsDone = 0;
    private int maleTotal;
    private int femaleTotal;

    protected override void CheckingPhase() {
        audioContext = (AudioContext3)sceneManager.AudioContext;
        possessivesManager = (PossessivesManager)sceneManager;

        SetTargetObjects();

        //Create the baskets and attach to them the script
        CreatePeopleAndBaskets();


        //Start listening to the guided Harvesting
        EventManager.StartListening(EventManager.Triggers.PickedFruit, TriggerHarvesting);

        maleTotal = maleObjects.Count;
        femaleTotal = femaleObjects.Count;

        StartCoroutine(IntroductionAndHarvesting());

    }

    private IEnumerator IntroductionAndHarvesting() { 
        yield return possessivesManager.IntroduceCheckingPhase();
        yield return StartGuidedHarvesting();
    }

    private void TriggerHarvesting() {
        StartCoroutine(StartGuidedHarvesting());
    }

    //Basic step of the checking pase: the VA tells to put a fruit in a specific basket and the user has to do it
    private IEnumerator StartGuidedHarvesting() {
        Debug.Log("male count " + maleObjectsDone + "female count " + femaleObjectsDone);
        Debug.Log("male count total " + maleObjects.Count + "female count total" + femaleObjects.Count);
        if (maleObjectsDone < maleTotal) {
            //Trigger the male objects

            audioContext.Possessive = Possessives.His;

            GameObject objectToCreate = sceneManager.ActivateObject(maleObjects[0], Positions.Central);

            SetFruitScripts(objectToCreate);

            maleObjectsDone++;

            //The VA introduces the object
            //Wait until the end of the introduction
            yield return possessivesManager.IntroduceObjectWithContext(objectToCreate.name);


        } else {
            Debug.Log("Here");
            if ( femaleObjectsDone < femaleTotal) {
                //Trigger the female objects

                audioContext.Possessive = Possessives.Her;

                GameObject objectToCreate = sceneManager.ActivateObject(femaleObjects[0], Positions.Central);

                SetFruitScripts(objectToCreate);

                femaleObjectsDone++;


                //The VA introduces the object
                //Wait until the end of the introduction
                yield return possessivesManager.IntroduceObjectWithContext(objectToCreate.name);

            } else {
                //Activity complete
                yield return null;

                //End the activity
                SceneManager.LoadScene("Menu");
                
            }

        }

    }

    private void SetTargetObjects() {
        SplitObjects();
    }

    private void CreatePeopleAndBaskets() {
        sceneManager.ActivateObject("Male", Positions.MalePosition);
        sceneManager.ActivateObject("Female", Positions.FemalePosition);
        CreateAndConfigureBaskets();
    }

    private void CreateAndConfigureBaskets() {
        GameObject basket1 = sceneManager.ActivateObject("MaleBasket", Positions.MaleBasket);
        GameObject basket2 = sceneManager.ActivateObject("FemaleBasket", Positions.FemaleBasket);

        ConfigureBaskets(basket1);
        ConfigureBaskets(basket2);
        Debug.Log("MALE OBJ BASKET ");
        foreach(string s in maleObjects) {
            Debug.Log(s);
        }
        Debug.Log("FEMALE OBJ BASKET ");
        foreach (string s in femaleObjects) {
            Debug.Log(s);
        }
        basket1.GetComponent<BasketLogic>().SetFruitList(maleObjects);
        basket2.GetComponent<BasketLogic>().SetFruitList(femaleObjects);
    }

    private void ConfigureBaskets(GameObject basket) {
        basket.AddComponent<Rigidbody>();
        basket.AddComponent<BoxCollider>().isTrigger = true;
        basket.AddComponent<ManipulationHandler>();
        basket.AddComponent<DoNotFall>();
        basket.AddComponent<BasketLogic>();
    }

    //Split the category of the objects creating two list, one for each character
    private void SplitObjects() {
        List<string> objects = AbstractSceneManager.Shuffle(SceneObjects);
        int half = objects.Count / 2;
        maleObjects = objects.GetRange(0, half);
        femaleObjects = objects.GetRange(half, half);
    }

    //Add to the object al the scripts needed for the activity
    private void SetFruitScripts(GameObject gameObj) {
        Rigidbody body = gameObj.AddComponent<Rigidbody>();
        body.useGravity = true;
        body.constraints = RigidbodyConstraints.FreezeRotation;
        gameObj.AddComponent<BoxCollider>();
        gameObj.AddComponent<ManipulationHandler>();
        gameObj.AddComponent<NearInteractionGrabbable>();
        gameObj.AddComponent<DoNotFall>();
    }

}
