using System;
using System.Collections;
using UnityEngine;
using static EventManager;

public class AnimateAvatar : MonoBehaviour {
    public Animator animator;
    private AudioSource audioSource;
    private AudioClip introduction;


    // Start is called before the first frame update
    void Start() {
        audioSource = gameObject.AddComponent<AudioSource>();
        introduction = Resources.Load("Audio/VAIntroduce") as AudioClip;
        animator = GetComponent<Animator>();
        StartListening();
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.M)) {
            PlayOk();
        }
        else if (Input.GetKeyDown(KeyCode.N)) {
            PlayIntroduction();
        }
        else if (Input.GetKeyDown(KeyCode.O)) {
            PlayKo();
        }
            
    }

    public void PlayKo() {
        animator.Play("KO");
    }

    public void PlayIntroduction() {
        StartCoroutine(PlayIntroductionRoutine());
    }

    public void PlayOk() {
        animator.Play("OK");
    }

//    public IEnumerator PlayOkRoutine() {
 //       yield return WaitForAudioEnd();
  //      yield return PlayAnimation("OK");
   // }

//    private object PlayAnimation(string animation) {
 //       animator.Play(animation);
 //       
 //   }

    private IEnumerator PlayIntroductionRoutine() {
        //Set the introduction audio clip and play it
        audioSource.clip = introduction;
        audioSource.Play();

        //Start the corresponding animation
        animator.Play("Talking");
        
        //Wait until the audio ends
        yield return new WaitForSeconds(audioSource.clip.length + 3);

        //Triggers AbstractSceneManager.LearningPhaseStart()
        TriggerEvent(Triggers.VAIntroductionEnd);
    }

    internal IEnumerator PlayCheckingPhaseIntroduction(AudioContext context) {
        Debug.Log("VA INTRO " + context);
        audioSource.clip = context.GetAudio("yourTurn");
        animator.Play("TalkingShort");
        yield return PlayAudioSync(audioSource);
    }

    internal IEnumerator IntroduceObject(AudioContext context, string objectName) {

        audioSource.clip = context.GetAudio(objectName);

        animator.Play("TalkingShort");
        yield return PlayAudioSync(audioSource);
    }

    internal IEnumerator IntroduceObjectWithContext(AudioContext context, string objectName) {
        audioSource.clip = context.GetAudioWithContext(objectName);
    
        animator.Play("TalkingShort");
        yield return PlayAudioSync(audioSource);
    }

    //Play audio and wait until it finishes
    private IEnumerator PlayAudioSync(AudioSource audioSource) {
        yield return WaitForAudioEnd();

        audioSource.Play();

        while(audioSource.isPlaying) {
            yield return null;
        }

        yield return new WaitForSeconds(1.5f);
    }

//    private IEnumerator WaitForAnimationEnd() {
 //       animator.GetCurrentAnimatorClipInfo(0).Length;
  //      whi()
   // }

    private IEnumerator WaitForAudioEnd() {
        while (audioSource.isPlaying) {
            yield return null;
        }
    }


    private void StartListening() {
        EventManager.StartListening(Triggers.VAOk, PlayOk);
        EventManager.StartListening(Triggers.VAKo, PlayKo);
    }

    private void StopListening() {
        EventManager.StopListening(Triggers.VAOk, PlayOk);
        EventManager.StartListening(Triggers.VAKo, PlayKo);
    }

}