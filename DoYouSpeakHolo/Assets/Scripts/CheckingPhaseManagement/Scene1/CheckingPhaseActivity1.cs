using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class CheckingPhaseActivity1 : CheckingPhaseManager {

    private PoPManager poPManager;
    private AudioContext1 audioContext;
    private DeskGrid deskGrid;
    private System.Random rnd;
    private List<Tuple<string, DeskGrid.Cell.Prepositions, string>> moves;
    private Tuple<string, DeskGrid.Cell.Prepositions, string> move;


    //Spawn the objects in random order and ask the user to pick a specific one
    protected override void CheckingPhase() {
        poPManager = (PoPManager)sceneManager;
        audioContext = (AudioContext1)poPManager.AudioContext;
        moves = new List<Tuple<string, DeskGrid.Cell.Prepositions, string>>();
        rnd = new System.Random();
        deskGrid = poPManager.Grid;
        PutObjectsInInitialPosition();
        SetTargetPositions();

        StartCoroutine(TidyUpTheDesk());
    }

    private void PutObjectsInInitialPosition() {

        deskGrid.ActivateCells();

        poPManager.ActivateObject(SceneObjects[0], deskGrid.Grid[1,2].CenterCoordinates, Positions.ObjectsRotation).GetComponent<Rigidbody>().useGravity = true;
        poPManager.ActivateObject(SceneObjects[1], deskGrid.Grid[0,1].CenterCoordinates, Positions.ObjectsRotation).GetComponent<Rigidbody>().useGravity = true;
        poPManager.ActivateObject(SceneObjects[2], deskGrid.Grid[1,0].CenterCoordinates, Positions.ObjectsRotation).GetComponent<Rigidbody>().useGravity = true;
        poPManager.ActivateObject(SceneObjects[3], deskGrid.Grid[2,0].CenterCoordinates, Positions.ObjectsRotation).GetComponent<Rigidbody>().useGravity = true;
        poPManager.ActivateObject(SceneObjects[4], deskGrid.Grid[2,1].CenterCoordinates, Positions.ObjectsRotation).GetComponent<Rigidbody>().useGravity = true;
        


    }

    private void SetTargetPositions() {
        string reference = "Book";
        moves.Add(new Tuple<string, DeskGrid.Cell.Prepositions, string>(reference,DeskGrid.Cell.Prepositions.Behind, "Lamp"));
        moves.Add(new Tuple<string, DeskGrid.Cell.Prepositions, string>(reference, DeskGrid.Cell.Prepositions.NextTo, "Mug"));
        moves.Add(new Tuple<string, DeskGrid.Cell.Prepositions, string>(reference, DeskGrid.Cell.Prepositions.InFrontOf, "Rubber"));
        moves.Add(new Tuple<string, DeskGrid.Cell.Prepositions, string>(reference, DeskGrid.Cell.Prepositions.NextTo, "PencilSharpener"));
    }

    internal void FoundObject() {
        moves.Remove(move);
        StartCoroutine(TidyUpTheDeskIteration());
    }

    private IEnumerator TidyUpTheDesk() {
        yield return poPManager.IntroduceCheckingPhase();
        yield return TidyUpTheDeskIteration();
    }

    private IEnumerator TidyUpTheDeskIteration() {
        if (moves.Count == 0)
            End();
        else
            yield return TargetNextMove();
    }

    private IEnumerator TargetNextMove() {
        move = moves[rnd.Next(moves.Count)];
        yield return poPManager.IntroduceMove(move);
        Debug.Log("move " + move.ToString());

        DeskGrid.Cell referenceCell = deskGrid.FindCellOf(move.Item1);
        
        referenceCell.SetMove(move.Item2, move.Item3);
        referenceCell.ListenForPositioning();
    }
}

