using System;
using System.Collections;
using UnityEngine;
using static EventManager;

public class AnimateAvatar : MonoBehaviour {
    public Animator animator;
    private AudioSource audioSource;
    private AudioClip introduction;
    private AudioClip ok;
    private AudioClip ko;

    // Start is called before the first frame update
    void Start() {
        audioSource = gameObject.AddComponent<AudioSource>();
        introduction = Resources.Load("Audio/VAIntroduce") as AudioClip;
        ok = Resources.Load("Audio/VAOk") as AudioClip;
        ko = Resources.Load("Audio/VAKo") as AudioClip;
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
        StartCoroutine(PlayKoRoutine());
    }

    public void PlayIntroduction() {
        StartCoroutine(PlayIntroductionRoutine());
    }

    public void PlayOk() {
        StartCoroutine(PlayOkRoutine());
    }

    private IEnumerator PlayOkRoutine() {
        audioSource.clip = ok;
        audioSource.PlayDelayed(0.5f);
        animator.Play("OK");
        yield return new WaitForSeconds(audioSource.clip.length + 3);
        Debug.Log("Trigger new event");
    }

    private IEnumerator PlayKoRoutine() {
        audioSource.clip = ko;
        audioSource.PlayDelayed(1f);
        animator.Play("KO");
        yield return new WaitForSeconds(audioSource.clip.length + 3);
        Debug.Log("Trigger new event");
    }

    private IEnumerator PlayIntroductionRoutine() {
        //Set the introduction audio clip and play it
        audioSource.clip = introduction;
        audioSource.Play();
        //Start the corresponding animation
        animator.Play("Talking");
        //Wait until the audio ends
        yield return new WaitForSeconds(audioSource.clip.length + 3);
        Debug.Log("Trigger learning phase");
        //Triggers AbstractSceneManager.LearningPhaseStart()
        EventManager.TriggerEvent(EventManager.Triggers.VAIntroductionEnd);
    }

    //Introduces an object
    internal void IntroduceObject(GameObject gameObject) {
        throw new NotImplementedException();
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