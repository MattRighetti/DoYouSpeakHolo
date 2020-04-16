using System.Collections.Generic;
using static EventManager;

/// <summary>
/// Scene Manager of Activity 3 
/// </summary>
public class PossessivesManager : AbstractSceneManager
{


    /// <summary>
    /// Keeps track of the mapping between an object and its associated possessive
    ///  e.g. (his, [pear, apple])
    /// </summary>
    public Dictionary<string, List<string>> PossessivesObjects { get; set; }

    /// <summary>
    /// Keeps track of the possessives loaded into the scene
    /// </summary>
    public List<Possessives> PossessivesList { get; set; }

    /// <summary>
    /// Set the audio context to scene 3
    /// </summary>
    public override void SetAudioContext() {
        AudioContext = new AudioContext3();
    }

    /// <summary>
    /// Create the scene objects and set all the parameters.
    /// </summary>
    public override void LoadObjects() {
        PossessivesObjects = new Dictionary<string, List<string>>();

        Pooler.CreateStaticObjects(sceneObjects.staticObjects);
        Pooler.CreateDynamicObjects(sceneObjects.dynamicObjects);

        List<string> dynamicObjects = Pooler.GetDynamicObjects();

        bool isCharacter(string x) => x.Contains("Character");

        //Select from the static objects the one referring to scene 
        //characters and put them into PossessivesList
        PossessivesList = Pooler
            .GetStaticObjects()
            .FindAll(x => isCharacter(x))
            .ConvertAll(x => Possessives.AsPossessive(x.Replace("Character", "")));

        PossessivesObjects = SplitObjects(dynamicObjects);

        CreateScene();
        
    }

    /// <summary>
    /// Activate and put the background elements in the scene
    /// </summary>
    private void CreateScene() {
        ActivateObject("House", Positions.HousePosition, Positions.ObjectsRotation);
        ActivateObject("Tree", Positions.TreePosition, Positions.ObjectsRotation);
    }

    /// <summary>
    /// Start Listening to custom events.
    ///  Triggered whenever a fruit is put into the correct basket
    /// </summary>
    public override void StartListeningToCustomEvents() {
        StartListening(Triggers.PickedFruit, PickedFruit);
    }

    /// <summary>
    /// Executes the handler of the PickedFruit event.
    /// </summary>
    private void PickedFruit() {
        CheckingPhaseActivity3 checkingManager = (CheckingPhaseActivity3)CheckingPhaseManager;
        checkingManager.PickedFruit();
    }

    /// <summary>
    /// Stop listening to custom events
    /// </summary>
    public override void StopListeningToCustomEvents() {
        StopListening(Triggers.PickedFruit, PickedFruit);
    }

    /// <summary>
    /// Split the category of the objects into two list, one for each possessive
    /// </summary>
    /// <param name="listToSplit"></param>
    /// <returns>Dictionary having as key the possessive and as value the fruit list</returns>
    private Dictionary<string, List<string>> SplitObjects(List<string> listToSplit) {
        Dictionary<string, List<string>> possessivesObjects = new Dictionary<string, List<string>>();
        List<string> objects = Shuffle(listToSplit);
        int half = objects.Count / 2;
        var character1Objects = objects.GetRange(0, half);
        var character2Objects = objects.GetRange(half, half);
        possessivesObjects.Add(PossessivesList[0].Value, character1Objects);
        possessivesObjects.Add(PossessivesList[1].Value, character2Objects);
        return possessivesObjects;
    }
}