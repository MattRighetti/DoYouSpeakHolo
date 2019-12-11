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