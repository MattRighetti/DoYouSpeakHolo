using System;
using UnityEngine;

public class CheckingPhaseActivity2 : CheckingPhaseManager {

    protected CandSManager candSManager;
    private AudioContext2 audioContext;

    private GameObject targetAnimal;

	protected override void CheckingPhase() {
        audioContext = (AudioContext2)sceneManager.AudioContext;
        candSManager = (CandSManager)sceneManager;


    }

}
