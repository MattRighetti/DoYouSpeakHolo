using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The class is responsible of getting the right audio to play related to a specific Game Object
/// </summary>
public abstract class AudioContext {
    protected Scenes Scene;

    //Get audio for a specific activity
    public abstract AudioClip GetContextAudio(string objectName);

    //Get audio describing the object
    //i.e. "This is a Tree"
    public AudioClip GetAudio(string objectName) {
        string path = "Audio/" + Scene.Value + "/" + objectName;
        Debug.Log("getting audio " + path);
        return Resources.Load(path) as AudioClip;
    }
}

//Audio Context Scene 1
public class AudioContext1 : AudioContext {

    public AudioContext1() {
        Scene = Scenes.Scene1;
    }

    public override AudioClip GetContextAudio(string objectName) {
        List<string> objects = new List<string>(objectName.Split('_'));

        string path = "Audio/" + Scene.Value + "/" + "Prepositions" + objects[3] + "/";
        Debug.Log("audio path: " + path);
        List<AudioClip> audioFiles = new List<AudioClip>(Resources.LoadAll<AudioClip>(path));
        foreach (AudioClip audio in audioFiles) {
            if (audio.name.Contains(objects[0]) && audio.name.Contains(objects[2]) && audio.name.Contains(objects[2])) {
                return audio;
            }
        }
        return null;
    }
}

//Audio Context Scene 2
public class AudioContext2 : AudioContext {

    public AudioContext2() {
        Scene = Scenes.Scene2;
    }

    public override AudioClip GetContextAudio(string objectName) {
        List<string> objects = new List<string>(objectName.Split('_'));

        switch (objects[0]) {
            case "Task": return PickTaskAudio(objects);
            case "Introduction": return PickIntroductionAudio(objects);
            default: throw new Exception("path bad formed");
        }
    }

    private AudioClip PickTaskAudio(List<string> objects) {
        string path = "Audio/" + Scene.Value + "/" + "Comparative" + objects[1] + "/" + objects[0] + "/";
        List<AudioClip> audioFiles = new List<AudioClip>(Resources.LoadAll<AudioClip>(path));
        System.Random rnd = new System.Random();
        return audioFiles[rnd.Next(audioFiles.Count)];
    }

    private AudioClip PickIntroductionAudio(List<string> objects) {
        string path = "Audio/" + Scene.Value + "/" + "Comparative" + objects[1] + "/" + objects[0] + "/";
        List<AudioClip> audioFiles = new List<AudioClip>(Resources.LoadAll<AudioClip>(path));
        foreach (AudioClip audio in audioFiles) {
            if (audio.name.Contains(objects[2])) {
                return audio;
            }
        }
        return null;
    }
}

//Audio Context Scene 3
//Custom audio: Possessives
//i.e. "this is his/her Tree"
public class AudioContext3 : AudioContext {

    public Possessives Possessive { get; set; }

    //Set scene3 by default in the constructor
    public AudioContext3() {
        Scene = Scenes.Scene3;
    }

    //Get the correct audio based on the current context
    public override AudioClip GetContextAudio(string objectName) {
        string path = "Audio/" + Scene.Value + "/" + Possessive.Value + objectName;
        Debug.Log("context audio " + path);
        return Resources.Load(path) as AudioClip;
    }
}

//Typesafe Enum pattern to do the audio selection
//Additional class for audio selection
public class Scenes {
    private Scenes(string value) { Value = value; }

    public string Value { get; set; }

    public static Scenes Scene1 { get { return new Scenes("Scene1"); } }
    public static Scenes Scene2 { get { return new Scenes("Scene2"); } }
    public static Scenes Scene3 { get { return new Scenes("Scene3"); } }
}

//Typesafe Enum pattern to do the audio selection
//Additional class for audio selection
//Used in Scene 3
public class Possessives {
    private Possessives(string value) { Value = value; }

    public string Value { get; set; }

    public static Possessives His { get { return new Possessives("his"); } }
    public static Possessives Her { get { return new Possessives("her"); } }
    public static Possessives Their { get { return new Possessives("their"); } }


    internal static Possessives AsPossessive(string value) {
        Debug.Log("TO PARSE " + value);
        switch (value) {
            case "his": return His;
            case "her": return Her;
            case "their": return Their;
            default: throw new ArgumentException();
        }
    }
}

public class Superlatives {
    private Superlatives(string value) { Value = value; }

    public string Value { get; set; }

    public static Superlatives Smallest { get { return new Superlatives("Smallest"); } }
    public static Superlatives Biggest { get { return new Superlatives("Biggest"); } }
}

