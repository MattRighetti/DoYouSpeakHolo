using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  Class responsible of the Basic Checking Phase version of Activity 3
public class CheckingPhaseActivity3Basic : CheckingPhaseActivity3
{
    private AudioContext3 audioContext;

    protected override void CheckingPhase() {
        audioContext = (AudioContext3)sceneManager.AudioContext;
        possessivesManager = (PossessivesManager)sceneManager;
        
        //Create the baskets and attach to them the script
        CreatePeopleAndBaskets();

        StartCoroutine(IntroductionAndHarvesting());
    }

    private IEnumerator IntroductionAndHarvesting() { 
        yield return possessivesManager.IntroduceCheckingPhase();
        yield return HarvestingIteration();
    }

    private void TriggerHarvesting() {
        StartCoroutine(HarvestingIteration());
    }

    //Basic step of the checking pase: the VA tells to put a fruit in a specific basket and the user has to do it
    private IEnumerator HarvestingIteration() {

        //Pick a fruit among 
        System.Random rnd = new System.Random();
        int choice = rnd.Next(0, possessivesManager.PossessivesObjects.Count);
        yield return TargetNextFruit(choice);
    }

    private IEnumerator TargetNextFruit(int choice) {
        string possessive = (new List<string>(possessivesManager.PossessivesObjects.Keys))[choice];
        if (possessivesManager.PossessivesObjects[possessive].Count > 0) {
            //Trigger the male objects
            audioContext.Possessive = Possessives.FromString(possessive);

            GameObject objectToCreate = sceneManager.ActivateObject(possessivesManager.PossessivesObjects[possessive][0], Positions.Central);

            SetFruitScripts(objectToCreate);

            //The VA introduces the object
            //Wait until the end of the introduction
            yield return possessivesManager.IntroduceObjectWithContext(objectToCreate.name);
        }
    }

    public override void PickedFruit() {
        //Call the superclass method
        base.PickedFruit();

        //Only if there are fruits left trigger the harvesting next step
        if (possessivesManager.PossessivesObjects.Count > 0) 
            TriggerHarvesting();
    }
}
