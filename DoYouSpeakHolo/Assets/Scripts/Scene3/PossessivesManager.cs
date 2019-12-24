using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static EventManager;

public class PossessivesManager : AbstractSceneManager
{

    private readonly SceneObjectsToLoad sceneObjects = SceneSwitcher.settings.scenes[2];
   
    public Dictionary<string, List<string>> PossessivesObjects { get; set; }
    public List<Possessives> PossessivesList { get; set; }

    //Keeps track of the basket with no more target fruits
    private int basketFull = 0;

    //Set the audio context to scene 3
    public override void SetAudioContext() {
        AudioContext = new AudioContext3();
    }

    //Load scene objects
    public override void LoadObjects() {

        PossessivesObjects = new Dictionary<string, List<string>>();

        Pooler.CreateStaticObjects(sceneObjects.staticObjects);
        Pooler.CreateDynamicObjects(sceneObjects.dynamicObjects);

        List<string> dynamicObjects = Pooler.GetDynamicObjects();

        SplitObjects(dynamicObjects);

        PossessivesList = new List<Possessives>();
        PossessivesList.Add(Possessives.His);
        PossessivesList.Add(Possessives.Her);

        CreateScene();
    }

    internal IEnumerator IntroduceCheckingPhase() { 
        yield return VirtualAssistant.PlayCheckingPhaseIntroduction(AudioContext);
    }

    //Activate and put the static elements in the scene
    private void CreateScene() {
        Pooler.ActivateObject("House", Positions.HousePosition);
        Pooler.ActivateObject("Tree", Positions.TreePosition);
        Pooler.ActivateObject("VA", Positions.VAPosition);
    }

    public override void StartListeningToCustomEvents() {
        StartListening(Triggers.PickedFruit, PickedFruit);
    }

    private void PickedFruit() {
        CheckingPhaseActivity3 checkingManager = (CheckingPhaseActivity3)CheckingPhaseManager;
        checkingManager.PickedFruit();
    }

    public override void StopListeningToCustomEvents() {
        StopListening(Triggers.PickedFruit, PickedFruit);
    }

    internal void changeLevel() {
        SceneManager.LoadScene("Scene3_bis");
    }

    //Split the category of the objects creating two list, one for each character
    private void SplitObjects(List<string> dynamicObjects) {
        Debug.Log("creating obj" + Possessives.His.Value + " " + Possessives.Her.Value);
        List<string> objects = Shuffle(dynamicObjects);
        int half = objects.Count / 2;
        var maleObjects = objects.GetRange(0, half);
        var femaleObjects = objects.GetRange(half, half);
        PossessivesObjects.Add(Possessives.His.Value,maleObjects);
        PossessivesObjects.Add(Possessives.Her.Value, femaleObjects);
    }
}