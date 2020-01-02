using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static EventManager;

public class PossessivesManager : AbstractSceneManager
{

    private readonly SceneObjectsToLoad sceneObjects = SceneSwitcher.settings[2];

    //Target fruits of the male basket
    public List<string> MaleObjects { get; set; }

    //Target fruits of the female basket
    public List<string> FemaleObjects { get; set; }

    //Keeps track of the basket with no more target fruits
    private int basketFull = 0;

    //Set the audio context to scene 3
    public override void SetAudioContext() {
        AudioContext = new AudioContext3();
    }

    //Load scene objects
    public override void LoadObjects() {
        Dictionary<string,string> staticObjects = new Dictionary<string, string>();
        Dictionary<string, string> dynamicObjects = new Dictionary<string, string>();
        staticObjects.Add("House", "Prefab/objects/House_right");
        staticObjects.Add("Tree", "Prefab/objects/Tree_right");
        staticObjects.Add("MaleBasket", "Prefab/objects/Basket");
        staticObjects.Add("FemaleBasket", "Prefab/objects/Basket");
        staticObjects.Add("Male", "Prefab/people/VA_MaleCorrect");
        staticObjects.Add("VA", "Prefab/people/Groot");
        staticObjects.Add("Female", "Prefab/people/VA_FemaleCorrect");
        dynamicObjects.Add("Apple", "Prefab/Fruits/Apple");
        dynamicObjects.Add("Banana", "Prefab/Fruits/Banana");
        dynamicObjects.Add("Orange", "Prefab/Fruits/Orange");
        dynamicObjects.Add("Pear", "Prefab/Fruits/Pear");
        Pooler.CreateStaticObjects(staticObjects);
        Pooler.CreateDynamicObjects(dynamicObjects);
        CreateScene();
    }

    //Chek if all the baskets are full
    private void CheckBaskets() {
        basketFull++;

        if (basketFull == 2) {
            EndActivity();
        }
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

    internal void SetMaleObjects(List<string> maleObjects) {
        MaleObjects = maleObjects;
    }

    internal void SetFemaleObjects(List<string> femaleObjects) {
        FemaleObjects = femaleObjects;
    }

    public override void StartListeningToCustomEvents() {
        StartListening(Triggers.BasketEmpty, CheckBaskets);
    }

    public override void StopListeningToCustomEvents() {
        StopListening(Triggers.BasketEmpty, CheckBaskets);
    }

    internal void changeLevel() {
        SceneManager.LoadScene("Scene3_bis");
    }
}

//Typesafe Enum pattern to do the audio selection
public class Possessives {
    private Possessives(string value) { Value = value; }

    public string Value { get; set; }

    public static Possessives His { get { return new Possessives("his"); } }
    public static Possessives Her { get { return new Possessives("her"); } }
}

//Typesafe Enum pattern to do the audio selection
public class Character {
    private Character(string value) { Value = value; }

    public string Value { get; set; }

    public static Character Male { get { return new Character("Male"); } }
    public static Character Female { get { return new Character("Female"); } }
}
