using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateAvatar : MonoBehaviour {
    public Animator animator;
    // Start is called before the first frame update
    void Start() {
        animator = GetComponent<Animator>();
        animator.Play("Idle");
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.M)) {
            animator.Play("OK");
        }
        else if (Input.GetKeyDown(KeyCode.N)) {
            animator.Play("Talking");
            Restart();  
        }
        else if (Input.GetKeyDown(KeyCode.O)) {
            animator.Play("KO");
        }
            
    }

    void Restart() {
        animator.Play("Idle");
    }
}