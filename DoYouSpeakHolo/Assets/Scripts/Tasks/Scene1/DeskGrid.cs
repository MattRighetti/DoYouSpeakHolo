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

    private Bounds tableBounds;

    /// <summary>
    /// The table transform, used to pick the necessary positions and rotations
    /// </summary>
    private readonly Transform tableTransform;
    private readonly int rows;
    private readonly int columns;

    public DeskGrid(Transform tableTransform, int rows, int columns, float cellLength, float cellWidth) {
        this.cellLength = cellLength;
        this.cellWidth = cellWidth;
        this.tableTransform = tableTransform;
        this.rows = rows;
        this.columns = columns;
        Grid = new Cell[rows, columns];
    }

    internal void ActivateCells() {
        for (int row = 0; row < rows; row++) {
            for (int column = 0; column < columns; column++) {
                Grid[row, column].ActivateCell();
            }
        }
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
        GameObject gameObject = new GameObject {
            name = "grid cell"
        };
        Grid[row, column] = gameObject.AddComponent<Cell>();
        Grid[row, column].Setup(cellCenter,
                                     cellLength,
                                     cellWidth,
                                     tableTransform,
                                     this);
    }

    internal Cell FindCellOf(string deskObject) {
        Debug.Log("Finding cell of " + deskObject);
        for (int row = 0; row < rows; row++) {
            for (int column = 0; column < columns; column++) {
                if (Grid[row, column].Contains(deskObject))
                    return Grid[row, column];
            }
        }
        return null;
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
        //private GameObject cellObject;

        /// <summary>
        /// Box Collider associated to a single cell, used to detect when an object enters/leaves the cell
        /// </summary>
        private BoxCollider boxCollider;

        /// <summary>
        /// Enumeration representing the prepositions of place
        /// </summary>
        public enum Prepositions { InFrontOf, Behind, NextTo, /*On*/};

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
        /// The cell grid center in local coordinates
        /// </summary>
        public Vector3 CenterCoordinates {
            get => gameObject.transform.position;
            private set => gameObject.transform.position = value; }

        public Vector3 StackSize {
            get => boxCollider.size;
            private set => boxCollider.size = value;
        }

        public Transform TableTransform { get; private set; }


        /// <summary>
        /// Grid reference
        /// </summary>
        private Cell[,] grid;

        /// <summary>
        /// Set up the new Game Object, creates the collider and the vertical stack.
        /// </summary>
        /// <param name="cellCenter">Cell grid center in local coordinates</param>
        /// <param name="cellLength">Cell and Box Collider length</param>
        /// <param name="cellWitdh">Cell and Box Collider width</param>
        /// <param name="tableTransform"></param>
        /// <param name="deskGrid"></param>
        public void Setup(Vector3 cellCenter, float cellLength, float cellWitdh, Transform tableTransform, DeskGrid deskGrid) {

            TableTransform = tableTransform;
            
            gameObject.transform.rotation = Quaternion.Euler(0f, tableTransform.rotation.eulerAngles.y, 0f);
            gameObject.transform.localScale = new Vector3(tableTransform.localScale.x, tableTransform.localScale.y,1);
            //Add the Box Collider to the Game Object
            boxCollider = gameObject.AddComponent<BoxCollider>();
            

            //Compute the cell center in local coordinates starting from the global ones
            CenterCoordinates = cellCenter + new Vector3(0, 0.005f, 0);

            //Resize the collider in order to keep each cell grid separate from the others
            StackSize = 0.8f * (new Vector3(cellLength, 0.02f, cellWitdh));

            boxCollider.isTrigger = true;
            boxCollider.enabled = false;
            
            grid = deskGrid.Grid;
            
            verticalStack = new List<string>();
        }

        public void SetMove(string referenceObject, Prepositions preposition, string targetObject) {
            ReferenceObject = referenceObject;
            TargetPreposition = preposition;
            TargetObject = targetObject;
        }

        internal void ActivateCell() {
            boxCollider.enabled = true;
        }

        /// <summary>
        /// Add the collider's game object to the stack and move the collider on top of it.
        /// </summary>
        /// <param name="other">the game object collider</param>
        private void OnTriggerEnter(Collider other) {
            if (!verticalStack.Contains(other.gameObject.name) && !Equals(other.gameObject.name, "Cube")) {
                Debug.Log(other.gameObject.name + " Entered the cell");
               
                verticalStack.Add(other.gameObject.name);

                Bounds objectBounds = other.gameObject.GetComponent<Renderer>().bounds;

                float incrementCenterY = objectBounds.size.y;
                //float incrementSizeY = objectBounds.size.y / 2;

                CenterCoordinates += new Vector3(0, incrementCenterY, 0);
                //boxCollider.size += new Vector3(0, incrementSizeY, 0);

                TriggerEvent(Triggers.ObjectPositioning);
            }
            
        }

        /// <summary>
        /// Remove the collider's game object from the stack and adjust the collider position
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerExit(Collider other) {
            if (verticalStack.Contains(other.gameObject.name) && !Equals(other.gameObject.name, "Cube")) {
                Debug.Log(other.gameObject.name + " Exited the cell");
                verticalStack.Remove(other.gameObject.name);

                Bounds objectBounds = other.gameObject.GetComponent<Renderer>().bounds;

                float decrementCenterY = objectBounds.size.y;
                //float decrementSizeY = objectBounds.size.y / 2;

                CenterCoordinates -= new Vector3(0, decrementCenterY, 0);
                //boxCollider.size -= new Vector3(0, decrementSizeY, 0);
            }
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
        public bool Contains(string targetObject) {
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
                /*case Prepositions.On:
                    offsets.Add(new Tuple<int, int>(-1, 0));
                    break;*/
                default: throw new NotImplementedException();
            }

            return offsets;
        }

        public static string PrepositionAsString(Prepositions targetPreposition) {
            switch (targetPreposition) {
                case Prepositions.Behind:
                    return "Behind";
                case Prepositions.InFrontOf:
                    return "InFrontOf";
                case Prepositions.NextTo:
                    return "NextTo";
                /*case Prepositions.On:
                    offsets.Add(new Tuple<int, int>(-1, 0));
                    break;*/
                default: throw new NotImplementedException();
            }

        }
    }
}
