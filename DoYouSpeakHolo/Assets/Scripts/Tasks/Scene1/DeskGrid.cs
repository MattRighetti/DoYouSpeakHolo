using System;
using System.Collections.Generic;
using UnityEngine;
using static EventManager;

/// <summary>
/// Grid wrapper class
/// </summary>
public class DeskGrid {

    /// <summary>
    /// Lenth of a single grid cell
    /// </summary>
    private readonly float cellLength;
    
    /// <summary>
    /// Width of a single grid cell
    /// </summary>        
    private readonly float cellWidth;

    /// <summary>
    /// The grid positioned on the table
    /// </summary>
    public Cell[,] Grid { get; }

    /// <summary>
    /// The table transform, used to pick the necessary positions and rotations
    /// </summary>
    private readonly Transform tableTransform;

    public DeskGrid(Transform tableTransform, int rows, int columns, float cellLength, float cellWidth) {
        this.cellLength = cellLength;
        this.cellWidth = cellWidth;
        this.tableTransform = tableTransform;
        Grid = new Cell[rows, columns];
    }

    /// <summary>
    /// Create a new cell and add it to the grid.
    /// </summary>
    /// <param name="cellCenter">The center of the cell. Used to compute the bounds</param>
    /// <param name="row">The grid row in which the cell has to be added</param>
    /// /// <param name="column">The grid column in which the cell has to be added</param>
    /// /// <param name="unitLength">The bounds offset on X axis</param>
    /// <param name="unitWitdh">The bounds offset on Z axis</param>
    internal void AddCell(Vector3 cellCenter, int row, int column) {
        Grid[row, column] = tableTransform.gameObject.AddComponent<Cell>();
        Grid[row, column].Setup(cellCenter,
                                     cellLength,
                                     cellWidth,
                                     tableTransform.gameObject,
                                     this);
    }

    /// <summary>
    /// Single grid cell.
    /// </summary>
    /// <remarks>Contains its buonds with respect to the table transformation and the game object stack</remarks>
    public class Cell : MonoBehaviour {

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
        public enum Prepositions { InFrontOf, Behind, NextTo, On};

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

        public Vector3 Center { get; private set; }

        /// <summary>
        /// Grid reference
        /// </summary>
        private Cell[,] grid;

        /// <summary>
        /// Set up the collider, the grid and creates the vertical stack.
        /// </summary>
        /// <param name="cellCenter">Box Collider center</param>
        /// <param name="cellLength">Box Collider length</param>
        /// <param name="cellWitdh">Box Collider width</param>
        /// <param name="rotation">Box Collider rotation</param>
        public void Setup(Vector3 cellCenter, float cellLength, float cellWitdh, GameObject table, DeskGrid deskGrid) {
            boxCollider = table.AddComponent<BoxCollider>();
            //boxCollider.center = cellCenter + new Vector3(0,0,0.02f);
            boxCollider.center = cellCenter;
            boxCollider.size = new Vector3(cellLength, cellWitdh, 0.01f);
            boxCollider.isTrigger = true;

            Center = boxCollider.center;
            
            grid = deskGrid.Grid;
            
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
        public static List<Tuple<int, int>> FindCellsToCheck(Prepositions targetPreposition) {
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
