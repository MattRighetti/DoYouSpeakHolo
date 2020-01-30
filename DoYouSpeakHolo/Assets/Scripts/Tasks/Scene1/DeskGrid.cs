using System;
using System.Collections.Generic;
using UnityEngine;
using static EventManager;

/// <summary>
/// Grid wrapper class
/// </summary>
public class DeskGrid {

    /// <summary>
    /// The grid positioned on the table
    /// </summary>
    private readonly Cell[,] grid;

    /// <summary>
    /// The table transform, used to pick the necessary positions and rotations
    /// </summary>
    private readonly Transform tableTransform;

    public DeskGrid(Transform tableTransform, int rows, int columns) {
        this.tableTransform = tableTransform;
        grid = new Cell[rows, columns];
    }

    /// <summary>
    /// Create a new cell and add it to the grid.
    /// </summary>
    /// <param name="cellCenter">The center of the cell. Used to compute the bounds</param>
    /// <param name="row">The grid row in which the cell has to be added</param>
    /// /// <param name="column">The grid column in which the cell has to be added</param>
    /// /// <param name="unitLength">The bounds offset on X axis</param>
    /// <param name="unitWitdh">The bounds offset on Z axis</param>
    internal void AddCell(Vector3 cellCenter, int row, int column, float cellLength, float cellWitdh) {
        grid[row, column] = new Cell(cellCenter, 
                                     cellLength, 
                                     cellWitdh, 
                                     tableTransform.rotation,
                                     this);
    }

    /// <summary>
    /// Single grid cell.
    /// </summary>
    /// <remarks>Contains its buonds with respect to the table transformation and the game object stack</remarks>
    private class Cell : MonoBehaviour {

        /// <summary>
        /// Keeps track of the game objects in the same cell.
        /// </summary>
        private List<string> verticalStack;

        /// <summary>
        /// Box Collider associated to a single cell, used to detect when an object enters/leaves the cell
        /// </summary>
        private BoxCollider boxCollider;

        /// <summary>
        /// Enumeration representing the prepositions of place
        /// </summary>
        private enum Prepositions { InFrontOf, Behind, NextTo, On};

        /// <summary>
        /// The object in the current cell.
        /// </summary>
        private string ReferenceObject { set; get; }

        /// <summary>
        /// The preposition of the task
        /// </summary>
        private Prepositions TargetPreposition { get; set; }

        /// <summary>
        /// The object that must be in the adjacent cell suggested by the preposition 
        /// </summary>
        private string TargetObject { set; get; }

        /// <summary>
        /// Grid reference
        /// </summary>
        private readonly Cell[,] grid;

        /// <summary>
        /// Set up the collider, the grid and creates the vertical stack.
        /// </summary>
        /// <param name="cellCenter">Box Collider center</param>
        /// <param name="cellLength">Box Collider length</param>
        /// <param name="cellWitdh">Box Collider width</param>
        /// <param name="rotation">Box Collider rotation</param>
        public Cell(Vector3 cellCenter, float cellLength, float cellWitdh, Quaternion rotation, DeskGrid deskGrid) {
            boxCollider = new BoxCollider {
                center = cellCenter + new Vector3(0, 0.02f, 0),
                size = new Vector3(cellLength, 0.01f, cellWitdh),
                isTrigger = true
            };

            grid = deskGrid.grid;

            boxCollider.transform.rotation = rotation;
            
            verticalStack = new List<string>();
        }

        /// <summary>
        /// Add the collider's game object to the stack and move the collider on top of it.
        /// </summary>
        /// <param name="other">the game object collider</param>
        private void OnTriggerEnter(Collider other) {
            BoxCollider otherBoxCollider = other as BoxCollider;
            verticalStack.Add(other.gameObject.name);

            float incrementCenterY = otherBoxCollider.size.y / 2;
            float incrementSizeY = otherBoxCollider.size.y / 2;

            boxCollider.center += new Vector3(0, incrementCenterY, 0);
            boxCollider.size += new Vector3(0, incrementSizeY, 0);


            
        }

        /// <summary>
        /// Remove the collider's game object from the stack and adjust the collider position
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerExit(Collider other) {
            BoxCollider otherBoxCollider = other as BoxCollider;
            verticalStack.Remove(other.gameObject.name);
  
            float decrementCenterY = otherBoxCollider.size.y / 2;
            float decrementSizeY = otherBoxCollider.size.y / 2;

            boxCollider.center -= new Vector3(0, decrementCenterY, 0);
            boxCollider.size -= new Vector3(0, decrementSizeY, 0);
        }

        /// <summary>
        /// Listen to object's movement to a new grid cell
        /// </summary>
        public void ListenForPositioning() => StartListening(Triggers.ObjectPositioning, CheckObjectPosition);

        /// <summary>
        /// Stop listen to object's movement
        /// </summary>
        public void StopListenForPositioning() => StopListening(Triggers.ObjectPositioning, CheckObjectPosition);

        /// <summary>
        /// Check if in the cells suggested by the preposition there is the target object and triggers the VA resposne.
        /// </summary>
        private void CheckObjectPosition() {
            List<Tuple<int, int>> cellsToCheck = FindCellsToCheck(TargetPreposition);

            //For each possible offset
            cellsToCheck.ForEach(cellOffset => {
                //If the corresponding cell contains the target object
                if (grid[cellOffset.Item1,cellOffset.Item2].Contains(TargetObject)) {
                    //Trigger the VA position
                    TriggerEvent(Triggers.VAOk);
                    //Trigger the handler for the next iteration
                    TriggerEvent(Triggers.CorrectPositioning);
                    return;
                }
            });

            //Trigger the VA negative reaction: the object has not been found
            TriggerEvent(Triggers.VAKo);
        }

        /// <summary>
        /// Checks wheter the object is in current cell
        /// </summary>
        /// <param name="targetObject"></param>
        /// <returns>True if it is in the cell, otherwise false</returns>
        private bool Contains(string targetObject) {
            return verticalStack.Contains(targetObject);
        }

        /// <summary>
        /// Finds the adjacent cell suggested by the preposition
        /// </summary>
        /// <param name="targetPreposition">The prepositionn</param>
        /// <returns>A list of tuple containing the cell's offset to check from the curent cell</returns>
        private List<Tuple<int, int>> FindCellsToCheck(Prepositions targetPreposition) {
            List<Tuple<int, int>> offsets = new List<Tuple<int, int>>();
            switch (targetPreposition) {
                case Prepositions.Behind:
                    offsets.Add(new Tuple<int, int>(-1, 0));
                    break;
                case Prepositions.InFrontOf:
                    offsets.Add(new Tuple<int, int>(+1, 0));
                    break;
                case Prepositions.NextTo:
                    offsets.Add(new Tuple<int, int>(0, +1));
                    offsets.Add(new Tuple<int, int>(0, -1));
                    break;
                case Prepositions.On:
                    offsets.Add(new Tuple<int, int>(-1, 0));
                    break;
                default: throw new NotImplementedException();
            }

            return offsets;
        }
    }
}
