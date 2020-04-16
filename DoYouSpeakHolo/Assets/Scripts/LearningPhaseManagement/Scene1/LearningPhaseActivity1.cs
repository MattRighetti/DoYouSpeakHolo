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

        // Reference for a 4x3 matrix
        Tuple<int, int> referencePosition = new Tuple<int, int>(1, 2);

        //1) Introduce desk objects
        foreach (string objectkKey in SceneObjects) {
            GameObject gameObject = poPManager.ActivateObject(objectkKey, grid.Grid[referencePosition.Item1, referencePosition.Item2].CenterCoordinates,Positions.ObjectsRotation);
            yield return poPManager.IntroduceObject(objectkKey);
            poPManager.DeactivateObject(objectkKey);
        }
        
        GameObject referenceObject = poPManager.ActivateObject("Book", grid.Grid[referencePosition.Item1, referencePosition.Item2].CenterCoordinates, Positions.ObjectsRotation);

        foreach (DeskGrid.Cell.Prepositions preposition in Enum.GetValues(typeof(DeskGrid.Cell.Prepositions))) {
            List<Tuple<int, int>> offsets =  DeskGrid.Cell.FindCellsToCheck(preposition);
            Tuple<string, DeskGrid.Cell.Prepositions, string> move = new Tuple<string, DeskGrid.Cell.Prepositions, string>(referenceObject.name,preposition, "Rubber");
            foreach (Tuple<int, int> offset in offsets) {
                GameObject gameObject = poPManager.ActivateObject("Rubber", grid.Grid[1 + offset.Item1, 2 + offset.Item2].CenterCoordinates + new Vector3(0,0.1f,0), Positions.ObjectsRotation);
                poPManager.EnableOutline("Rubber");
                yield return poPManager.IntroducePreposition(move);
                yield return new WaitForSeconds(2);
                poPManager.DisableOutline("Rubber");
                poPManager.DeactivateObject("Rubber");

            }
    
        }

        poPManager.DeactivateObject("Book");

        //End the learning phase
        End();
    }
}
