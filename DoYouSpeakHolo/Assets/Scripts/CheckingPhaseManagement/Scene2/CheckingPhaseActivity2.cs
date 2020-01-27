using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckingPhaseActivity2 : CheckingPhaseManager {

    protected CandSManager candSManager;
    private AudioContext2 audioContext;

    private int targetAnimalIndex;

    System.Random rnd;

    protected override void CheckingPhase() {
        audioContext = (AudioContext2)sceneManager.AudioContext;
        candSManager = (CandSManager)sceneManager;
        rnd = new System.Random();

        StartCoroutine(IntroductionAndSaveAnimals());
    }

    internal void PickedAnimal() {
        if (SceneObjects.Count == 0)
            End();
        else
            TriggerAnimalIteration();
    }

    private void TriggerAnimalIteration() {
        StartCoroutine(SaveAnimalsIteration());
    }

    private IEnumerator IntroductionAndSaveAnimals() {
        yield return candSManager.IntroduceCheckingPhase();
        GameObject.Find("Ark").GetComponent<ArkLogic>().AnimalList = SceneObjects;
        yield return SaveAnimalsIteration();
    }

    private IEnumerator SaveAnimalsIteration() {
        targetAnimalIndex = rnd.Next(0, SceneObjects.Count);

        yield return TartgetNextAnimal();
    }

    private IEnumerator TartgetNextAnimal() {

        string targetAnimal = SceneObjects[targetAnimalIndex];

        GameObject.Find("Ark").GetComponent<ArkLogic>().SetTargetAnimal(targetAnimal);
        
        //If there are more than one animal use the comparatives and superlatives audios
        if (SceneObjects.Count > 1) {

            yield return AnalyzeRelatioships(targetAnimal);

        } else {
            //Last remaining animal
            throw new NotImplementedException();
        }
    }

    private IEnumerator AnalyzeRelatioships(string targetAnimal) {

        bool canUseSuperlative = (targetAnimalIndex == 0) || (targetAnimalIndex == SceneObjects.Count - 1); 
        
        if (canUseSuperlative) {
            bool useSuperlative = (rnd.Next(0, 2)) == 0;

            if (useSuperlative) {
                if (targetAnimalIndex == 0)
                    yield return TargetNextSuperlative(targetAnimal, Superlatives.Smallest);
                else
                    yield return TargetNextSuperlative(targetAnimal, Superlatives.Biggest);
            }
            else
                yield return TargetNextComparative(targetAnimal);

        } else
            yield return TargetNextComparative(targetAnimal);

    }

    private IEnumerator TargetNextComparative(string targetAnimal) {
        yield return candSManager.IntroduceTasktWithComparatives(targetAnimal);
    }

    private IEnumerator TargetNextSuperlative(string targetAnimal, Superlatives superlative) {
        yield return candSManager.IntroduceTaskWithSuperlatives(targetAnimal, superlative.Value);
    }
}