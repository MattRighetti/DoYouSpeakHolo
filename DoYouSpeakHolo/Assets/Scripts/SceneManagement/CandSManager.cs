using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//  Scene Manager of Activity 1
public class CandSManager : AbstractSceneManager {

    public List<string> TargetObjects { get; set; }

    //  Set the audio context to scene 2
    public override void SetAudioContext() {
        AudioContext = new AudioContext2();
    }

    //  Create the scene objects
    public override void LoadObjects() {

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

    public IEnumerator IntroduceObjectWithComparatives(string firstAnimal, string secondAnimal) {
        yield return IntroduceObjectWithContext(firstAnimal + "_" + secondAnimal);
    }

    public IEnumerator IntroduceObjectWithSuperlatives(string objectKey, string superlative) {
        yield return IntroduceObject(objectKey + superlative);
    }

    internal void EnableOutline(string objectKey) => Pooler.GetPooledObject(objectKey).GetComponent<HighlightEnabler>().EnableOutline();

    internal void DisableOutline(string objectkKey) => Pooler.GetPooledObject(objectkKey).GetComponent<HighlightEnabler>().DisableOutline();
}

