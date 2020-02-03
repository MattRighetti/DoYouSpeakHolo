using System.Collections;
using static EventManager;

/// <summary>
/// Scene Manager of Activity 2 (Save the animals)
/// </summary>
public class CandSManager : AbstractSceneManager {

    //  Set the audio context to scene 2
    public override void SetAudioContext() {
        AudioContext = new AudioContext2();
    }

    //  Create the scene objects
    public override void LoadObjects() {

        Pooler.CreateStaticObjects(sceneObjects.staticObjects);
        Pooler.CreateDynamicObjects(sceneObjects.dynamicObjects);

        CreateScene();
    }

    private void CreateScene() {
        ActivateObject("Ark", Positions.ArkPosition, Positions.ObjectsRotation);
    }

    public override void StartListeningToCustomEvents() => StartListening(Triggers.PickedAnimal, PickedAnimal);

    public override void StopListeningToCustomEvents() => StopListening(Triggers.PickedAnimal, PickedAnimal);

    private void PickedAnimal() {
        CheckingPhaseActivity2 checkingManager = (CheckingPhaseActivity2)CheckingPhaseManager;
        checkingManager.PickedAnimal();
    }

    public IEnumerator IntroduceObjectWithComparatives(string firstAnimal, string secondAnimal) {
        yield return IntroduceObjectWithContext("Introduction_" + firstAnimal + "_" + secondAnimal);
    }

    internal IEnumerator IntroduceTasktWithComparatives(string targetAnimal) {
        yield return IntroduceObjectWithContext("Task_" + targetAnimal);
    }

    public IEnumerator IntroduceObjectWithSuperlatives(string animal, string superlative) {
        yield return IntroduceObjectWithContext("Introduction_" + animal + "_" + superlative);
    }

    internal IEnumerator IntroduceTaskWithSuperlatives(string targetAnimal, string superlative) {
        yield return IntroduceObjectWithContext("Task_" + targetAnimal + "_" + superlative);
    }

    internal IEnumerator IntroduceLastAnimal() {
        yield return IntroduceObject("RemainingAnimal");
    }
}

