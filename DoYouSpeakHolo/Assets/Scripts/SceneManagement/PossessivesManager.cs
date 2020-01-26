using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using static EventManager;

//  Scene Manager of Activity 3 
public class PossessivesManager : AbstractSceneManager
{


    //  Keeps track of the mapping between an object and its associated possessive
    //  e.g. (his, [pear, apple])
    public Dictionary<string, List<string>> PossessivesObjects { get; set; }

    public List<Possessives> PossessivesList { get; set; }

    //  Keeps track of the basket with no more target fruits
    private int basketFull = 0;

    //  Set the audio context to scene 3
    public override void SetAudioContext() {
        AudioContext = new AudioContext3();
    }

    //Load scene objects
    public override void LoadObjects() {
        PossessivesObjects = new Dictionary<string, List<string>>();

        Pooler.CreateStaticObjects(sceneObjects.staticObjects);
        Pooler.CreateDynamicObjects(sceneObjects.dynamicObjects);

        List<string> dynamicObjects = Pooler.GetDynamicObjects();
        PossessivesList = new List<Possessives>();

        foreach(string obj in Pooler.GetStaticObjects()) {
            if (obj.Contains("Character")) {
                PossessivesList.Add(Possessives.AsPossessive(obj.Replace("Character", "")));
            }
        }

        PossessivesObjects = SplitObjects(dynamicObjects);

        CreateScene();
        
    }

    //  The Virtual Assitant introduces the Checking Phase of the Activity
    internal IEnumerator IntroduceCheckingPhase() { 
        yield return VirtualAssistant.PlayCheckingPhaseIntroduction(AudioContext);
    }

    //  Activate and put the static elements in the scene
    private void CreateScene() {
        ActivateObject("House", Positions.HousePosition, Positions.ObjectsRotation);
        ActivateObject("Tree", Positions.TreePosition, Positions.ObjectsRotation);
    }

    //  Start Listening to the PickedFruit event.
    //  Triggered whenever a fruit is put into the correct basket
    public override void StartListeningToCustomEvents() {
        StartListening(Triggers.PickedFruit, PickedFruit);
    }

    //  Executes the handler of the PickedFruit event
    private void PickedFruit() {
        CheckingPhaseActivity3 checkingManager = (CheckingPhaseActivity3)CheckingPhaseManager;
        checkingManager.PickedFruit();
    }

    public override void StopListeningToCustomEvents() {
        StopListening(Triggers.PickedFruit, PickedFruit);
    }

    //Split the category of the objects into two list, one for each possessive
    private Dictionary<string, List<string>> SplitObjects(List<string> dynamicObjects) {
        Dictionary<string, List<string>> possessivesObjects = new Dictionary<string, List<string>>();
        List<string> objects = Shuffle(dynamicObjects);
        int half = objects.Count / 2;
        var character1Objects = objects.GetRange(0, half);
        var character2Objects = objects.GetRange(half, half);
        possessivesObjects.Add(PossessivesList[0].Value, character1Objects);
        possessivesObjects.Add(PossessivesList[1].Value, character2Objects);
        return possessivesObjects;
    }
}