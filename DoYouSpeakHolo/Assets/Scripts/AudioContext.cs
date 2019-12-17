using UnityEngine;

//The class is responsible of getting the right audio to play related to a Game Object
public abstract class AudioContext {
    protected Scenes Scene;

    //Get audio for a specific activity
    public abstract AudioClip GetAudioWithContext(string objectName);

    //Get audio describing the object
    //i.e. "This is a Tree"
    public AudioClip GetAudio(string objectName) {
        string path = "Audio/" + Scene.Value + "/" + objectName;
        Debug.Log("no context audio " + path);
        return Resources.Load(path) as AudioClip;
    }
}

//Audio Context Scene 1
public class AudioContext1 : AudioContext {

    public AudioContext1() {
        Scene = Scenes.Scene1;
    }

    public override AudioClip GetAudioWithContext(string objectName) {
        throw new System.NotImplementedException();
    }
}

//Audio Context Scene 2
public class AudioContext2 : AudioContext {

    public AudioContext2() {
        Scene = Scenes.Scene1;
    }

    public override AudioClip GetAudioWithContext(string objectName) {
        throw new System.NotImplementedException();
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
    public override AudioClip GetAudioWithContext(string objectName) {
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
public class Character {
    private Character(string value) { Value = value; }

    public string Value { get; set; }

    public static Character Male { get { return new Character("Male"); } }
    public static Character Female { get { return new Character("Female"); } }
}

//Typesafe Enum pattern to do the audio selection
//Additional class for audio selection
//Used in Scene 3
public class Possessives {
    private Possessives(string value) { Value = value; }

    public string Value { get; set; }

    public static Possessives His { get { return new Possessives("his"); } }
    public static Possessives Her { get { return new Possessives("her"); } }
}
