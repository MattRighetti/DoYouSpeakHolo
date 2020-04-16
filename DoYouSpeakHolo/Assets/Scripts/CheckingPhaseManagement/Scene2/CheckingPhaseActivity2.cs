using System.Collections;
using UnityEngine;

/// <summary>
/// Scene 2 Checking phase (Save the animals)
/// </summary>
public class CheckingPhaseActivity2 : CheckingPhaseManager {

    protected CandSManager candSManager;
    private AudioContext2 audioContext;
    private System.Random rnd;

    /// <summary>
    /// Index of the current animal that has to go on the ark
    /// </summary>
    private int targetAnimalIndex;

    /// <summary>
    /// Setup the attributes and starts the Checking phase
    /// </summary>
    protected override void CheckingPhase() {
        audioContext = (AudioContext2)sceneManager.AudioContext;
        candSManager = (CandSManager)sceneManager;
        rnd = new System.Random();

        StartCoroutine(IntroductionAndSaveAnimals());
    }

    /// <summary>
    /// Check whether to do another iteration of the checking phase or end it 
    /// if there are no more animals to save.
    /// </summary>
    internal void PickedAnimal() {
        if (SceneObjects.Count == 0)
            End();
        else
            TriggerAnimalIteration();
    }

    /// <summary>
    /// Wrap a single checking phase iteration in a coroutine
    /// </summary>
    private void TriggerAnimalIteration() {
        StartCoroutine(SaveAnimalsIteration());
    }

    /// <summary>
    /// Makes the Virtual Assistant introduce the checking phase and start the
    /// first iteration of the activity.
    /// </summary>
    /// <returns></returns>
    private IEnumerator IntroductionAndSaveAnimals() {
        yield return candSManager.IntroduceCheckingPhase();
        GameObject.Find("Ark").GetComponent<ArkLogic>().AnimalList = SceneObjects;
        yield return SaveAnimalsIteration();
    }


    /// <summary>
    /// Choose target animal and play the corresponding audio
    /// </summary>
    /// <returns></returns>
    private IEnumerator SaveAnimalsIteration() {
        targetAnimalIndex = rnd.Next(0, SceneObjects.Count);

        yield return TartgetNextAnimal();
    }

    /// <summary>
    /// Set the target animal in ArkLogic script and choose the audio
    /// </summary>
    /// <remarks> Audio choice is between one regarding comparative/superlative and the one 
    /// where there is only one animal left.
    /// </remarks>
    /// <returns></returns>
    private IEnumerator TartgetNextAnimal() {

        string targetAnimal = SceneObjects[targetAnimalIndex];

        GameObject.Find("Ark").GetComponent<ArkLogic>().SetTargetAnimal(targetAnimal);
        
        //If there are more than one animal use the comparatives and superlatives audios
        if (SceneObjects.Count > 1)
            yield return ChooseComparativeOrSuperlative(targetAnimal);
        else
            yield return TargetLastAnimal();
    }


    /// <summary>
    /// Choose the audio to play between the comparative and the superlative one.
    /// </summary>
    /// <remarks>The method heavily relies on the animal position in the SceneObjects list</remarks>
    /// <param name="targetAnimal">The target animal</param>
    /// <returns></returns>
    private IEnumerator ChooseComparativeOrSuperlative(string targetAnimal) {

        //If the animal is the first or the last in the list, the superlative audio can be used
        bool canUseSuperlative = (targetAnimalIndex == 0) || (targetAnimalIndex == SceneObjects.Count - 1); 
        
        if (canUseSuperlative) {
            //Decide to use the superlative audio or not
            bool useSuperlative = (rnd.Next(0, 2)) == 0;

            if (useSuperlative) {
                //Use the superlative audio
                if (targetAnimalIndex == 0)
                    yield return TargetNextSuperlative(targetAnimal, Superlatives.Smallest);
                else
                    yield return TargetNextSuperlative(targetAnimal, Superlatives.Biggest);
            }
            else
                //Use the comparative audio
                yield return TargetNextComparative(targetAnimal);

        } else
            //Otherwise use the comparative audio
            yield return TargetNextComparative(targetAnimal);
    }

    private IEnumerator TargetNextComparative(string targetAnimal) {
        yield return candSManager.IntroduceTasktWithComparatives(targetAnimal);
    }

    private IEnumerator TargetNextSuperlative(string targetAnimal, Superlatives superlative) {
        yield return candSManager.IntroduceTaskWithSuperlatives(targetAnimal, superlative.Value);
    }

    private IEnumerator TargetLastAnimal() {
        yield return candSManager.IntroduceLastAnimal();
    }
}