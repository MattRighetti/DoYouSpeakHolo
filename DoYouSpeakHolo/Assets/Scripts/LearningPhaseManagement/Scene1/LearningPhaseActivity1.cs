using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LearningPhaseActivity1 : LearningPhaseManager {

    PoPManager poPManager;
    private AudioContext1 audioContext;
    private DeskGrid grid;

    protected override void LearningPhase() {
        poPManager = (PoPManager)sceneManager;
        audioContext = (AudioContext1)poPManager.AudioContext;
        grid = poPManager.Grid;
        StartCoroutine(SceneIntroduction());
    }

    protected IEnumerator SceneIntroduction() {
        //1) Introduce desk objects
        foreach (string objectkKey in SceneObjects) {
            GameObject gameObject = poPManager.ActivateObject(objectkKey, grid.Grid[1,2].Center,Positions.ObjectsRotation);
            yield return poPManager.IntroduceObject(objectkKey);
            poPManager.DeactivateObject(objectkKey);
        }

        // Reference for a 4x3 matrix
        Tuple<int, int> referencePosition = new Tuple<int, int>(1, 2);
        
        foreach(DeskGrid.Cell.Prepositions preposition in Enum.GetValues(typeof(DeskGrid.Cell.Prepositions))) {
            List<Tuple<int, int>> offsets =  DeskGrid.Cell.FindCellsToCheck(preposition);
            
            foreach (Tuple<int, int> offset in offsets) {
                GameObject gameObject = poPManager.ActivateObject("Pencil", grid.Grid[1 + offset.Item1, 2 + offset.Item2].Center, Positions.ObjectsRotation);
                yield return new WaitForSeconds(2);
                poPManager.DeactivateObject("Pencil");
            }
    
        }



        //

        //End the learning phase
        End();
    }
}
