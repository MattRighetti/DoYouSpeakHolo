using UnityEngine;

//  Wrapper for the Virtual Assitant audio contained into the animations:
//  - OK
//  - KO
public class TaskReactionSound : MonoBehaviour
{
    private AudioSource audioSource;
    private AudioClip ok;
    private AudioClip ko;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        ok = Resources.Load("Audio/VAOk") as AudioClip;
        ko = Resources.Load("Audio/VAKo") as AudioClip;
    }

    //PLay ok sound triggered by the animation event
    void OkSound() {
        audioSource.PlayOneShot(ok);
    }

    //PLay ko sound triggered by the animation event
    void KoSound() {
        audioSource.PlayOneShot(ko);
    }
}
