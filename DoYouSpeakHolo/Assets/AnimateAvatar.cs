using System;
using System.Collections;
using UnityEngine;
using static EventManager;

public class AnimateAvatar : MonoBehaviour {
    public Animator animator;
    private AudioSource audio;
    private AudioClip introduction;
    private AudioClip ok;
    private AudioClip ko;
    public GameObject ObjectToIntroduce;

    // Start is called before the first frame update
    void Start() {
        audio = gameObject.AddComponent<AudioSource>();
        introduction = Resources.Load("Audio/VAIntroduce") as AudioClip;
        ok = Resources.Load("Audio/VAOk") as AudioClip;
        ko = Resources.Load("Audio/VAKo") as AudioClip;
        animator = GetComponent<Animator>();
        animator.Play("Idle");
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
        audio.clip = ok;
        audio.PlayDelayed(0.5f);
        animator.Play("OK");
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length + animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
        Debug.Log("Trigger new event");
    }

    private IEnumerator PlayKoRoutine() {
        audio.clip = ko;
        audio.PlayDelayed(1f);
        animator.Play("KO");
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length + animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
        Debug.Log("Trigger new event");
    }

    private IEnumerator PlayIntroductionRoutine() {
        //Set the introduction audio clip and play it
        audio.clip = introduction;
        audio.Play();
        //Start the corresponding animation
        animator.Play("Talking");
        //Wait until the animation ends
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length + animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
        
        //Triggers AbstractSceneManager.LearningPhaseStart()
        EventManager.TriggerEvent(EventManager.Triggers.VAIntroductionEnd);
    }

    //Introduces an object
    internal void IntroduceObject(GameObject gameObject) {
        throw new NotImplementedException();
    }

    private void StartListening() {
        EventManager.StartListening(Triggers.VAIntroduce, PlayIntroduction);
        EventManager.StartListening(Triggers.VAOk, PlayOk);
        EventManager.StartListening(Triggers.VAKo, PlayKo);
    }
}