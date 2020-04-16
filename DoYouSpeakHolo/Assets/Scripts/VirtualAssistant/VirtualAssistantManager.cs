using System.Collections;
using UnityEngine;
using static EventManager;

public class VirtualAssistantManager : MonoBehaviour {
    public Animator animator;
    private AudioSource audioSource;
    private string activeTrigger = "isIdle";

    public void Update() {
        FollowUsergaze();
    }

    public void FollowUsergaze() {
        Vector3 relativePos = Camera.main.transform.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(relativePos);
        rotation.x = 0f;
        rotation.z = 0f;
        transform.rotation = rotation;
    }

    // Start is called before the first frame update
    public void Setup() {
        audioSource = gameObject.AddComponent<AudioSource>();
        animator = GetComponent<Animator>();
        StartListening();
    }

    public void PlayKo() {
        SetTriggerWrapper("isWrong");
    }

    public void PlayIntroduction(AudioContext audioContext) {
        StartCoroutine(PlayIntroductionRoutine(audioContext));
    }

    public void PlayOk() {
        SetTriggerWrapper("isCorrect");
    }

    private IEnumerator PlayIntroductionRoutine(AudioContext context) {
        //Set the introduction audio clip and play it
        audioSource.clip = context.GetAudio("Introduction");
        audioSource.Play();

        //Start the corresponding animation
        SetTriggerWrapper("isTalking");

        //Wait until the audio ends
        yield return new WaitForSeconds(audioSource.clip.length + 3);

        //Triggers AbstractSceneManager.LearningPhaseStart()
        TriggerEvent(Triggers.VAIntroductionEnd);
    }

    internal IEnumerator PlayCheckingPhaseIntroduction(AudioContext context) {
        audioSource.clip = context.GetAudio("yourTurn");

        SetTriggerWrapper("isTalkingShort");

        yield return PlayAudioSync(audioSource);
    }

    internal IEnumerator IntroduceObject(AudioContext context, string objectName) {
        audioSource.clip = context.GetAudio(objectName);

        SetTriggerWrapper("isTalkingShort");

        yield return PlayAudioSync(audioSource);
    }

    internal IEnumerator IntroduceObjectWithContext(AudioContext context, string objectName) {
        audioSource.clip = context.GetContextAudio(objectName);

        SetTriggerWrapper("isTalkingShort");

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

    public void ReplayLatestAudio() {
        StartCoroutine(PlayAudioSync(audioSource));
        SetTriggerWrapper("isTalkingShort");
    }

    private IEnumerator WaitForAudioEnd() {
        while (audioSource.isPlaying) {
            yield return null;
        }
    }

    private void SetTriggerWrapper(string TriggerName) {

        if (!string.Equals("isIdle", activeTrigger)) {
            animator.ResetTrigger(activeTrigger);
            animator.SetTrigger("isIdle");
            Debug.Log("Resetting Trigger: " + activeTrigger);
        }

        animator.ResetTrigger("isIdle");
        animator.SetTrigger(TriggerName);
        Debug.Log("Activated Trigger: " + TriggerName);
        activeTrigger = TriggerName;
        Debug.Log("Active Trigger is now: " + activeTrigger);
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