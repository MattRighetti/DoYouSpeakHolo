//Set the current context used by the VA in order to pick the right audio
using UnityEngine;

public abstract class AudioContext {
    protected Scenes Scene;

    public abstract AudioClip GetAudioWithContext(string objectName);

    public AudioClip GetAudio(string objectName) {
        string path = "Audio/" + Scene.Value + "/" + objectName;
        Debug.Log("no context audio " + path);
        return Resources.Load(path) as AudioClip;
    }
}

public class AudioContext1 : AudioContext {

    public AudioContext1() {
        Scene = Scenes.Scene1;
    }

    public override AudioClip GetAudioWithContext(string objectName) {
        throw new System.NotImplementedException();
    }
}
public class AudioContext2 : AudioContext {

    public AudioContext2() {
        Scene = Scenes.Scene1;
    }

    public override AudioClip GetAudioWithContext(string objectName) {
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
    public override AudioClip GetAudioWithContext(string objectName) {
        string path = "Audio/" + Scene.Value + "/" + Possessive.Value + objectName;
        Debug.Log("context audio " + path);
        return Resources.Load(path) as AudioClip;
    }
}