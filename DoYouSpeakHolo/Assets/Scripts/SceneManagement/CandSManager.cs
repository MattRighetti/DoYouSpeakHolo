using System.Collections.Generic;
//  Scene Manager of Activity 1
public class CandSManager : AbstractSceneManager {

    //  Scene objects information
    private SceneObjectsToLoad sceneObjects;

    public List<string> TargetObjects { get; set; }

    //  Set the audio context to scene 2
    public override void SetAudioContext() {
        AudioContext = new AudioContext2();
    }

    //  Create the scene objects
    public override void LoadObjects() {
        sceneObjects = settings.scenes[1];

        Pooler.CreateStaticObjects(sceneObjects.staticObjects);
        Pooler.CreateDynamicObjects(sceneObjects.dynamicObjects);
        TargetObjects = Shuffle(Pooler.GetDynamicObjects());

        CreateScene();
    }

    private void CreateScene() {
        ActivateObject("Ark", Positions.ArkPosition, Positions.ObjectsRotation);
    }

    public override void StartListeningToCustomEvents() {
    }

    public override void StopListeningToCustomEvents() {
    }
}

