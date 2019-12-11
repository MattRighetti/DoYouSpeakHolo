//Set the current context used by the VA in order to pick the right audio
using UnityEngine;

public abstract class AudioContext {
    protected Scenes Scene;

    public abstract AudioClip GetAudio(string objectName);
}

public class AudioContext1 : AudioContext {

    public AudioContext1() {
        Scene = Scenes.Scene1;
    }

    public override AudioClip GetAudio(string objectName) {
        throw new System.NotImplementedException();
    }
}
public class AudioContext2 : AudioContext {

    public AudioContext2() {
        Scene = Scenes.Scene1;
    }

    public override AudioClip GetAudio(string objectName) {
        throw new System.NotImplementedException();
    }
}
public class AudioContext3 : AudioContext {

    public Possessives Possessive { get; set; }

    //Set scene3 by default in the constructor
    public AudioContext3() {
        Scene = Scenes.Scene3;
    }

    //Get the correct audio based on the current context
    public override AudioClip GetAudio(string objectName) {
        string path = "Audio/" + Scene.Value + "/" + Possessive.Value + objectName;
        Debug.Log("picking audio " + path);
        AudioClip clip = Resources.Load(path) as AudioClip;

        return clip;
    }
}