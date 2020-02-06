using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.UI;
using System;
using System.Collections;
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

    /// <summary>
    /// Activate all the cells's colliders and add the focus handler to every cell
    /// </summary>
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
                                     row,
                                     column,
                                     tableTransform,
                                     this);
    }

    /// <summary>
    /// Find the cell in which the object is contained
    /// </summary>
    /// <param name="deskObject"></param>
    /// <returns></returns>
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


        /// <summary>
        /// Box Collider associated to a single cell, used to detect when an object enters/leaves the cell
        /// </summary>
        private BoxCollider boxCollider;

        /// <summary>
        /// Detect whether the user is looking at the cell.
        /// </summary>
        private FocusHandler focusHandler;

        /// <summary>
        /// Enumeration representing the prepositions of place
        /// </summary>
        public enum Prepositions { InFrontOf, Behind, NextTo, /*On*/ };

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
            private set => gameObject.transform.position = value;
        }

        /// <summary>
        /// Cell collider dimension.
        /// </summary>
        public Vector3 StackSize {
            get => boxCollider.size;
            private set => boxCollider.size = value;
        }

        /// <summary>
        /// Grid reference
        /// </summary>
        private Cell[,] grid;

        /// <summary>
        /// Cell row index
        /// </summary>
        private int cellRow;

        /// <summary>
        /// Cell column index
        /// </summary>
        private int cellColumn;

        private bool finishedFocusEnter = true;
        private bool isExecuting = false;

        /// <summary>
        /// Set up the new Game Object, creates the collider and the vertical stack.
        /// </summary>
        /// <param name="cellCenter">Cell grid center in world coordinates</param>
        /// <param name="cellLength">Cell and Box Collider length</param>
        /// <param name="cellWitdh">Cell and Box Collider width</param>
        /// <param name="cellRow">Cell row index</param>
        /// <param name="cellColumn">Cell column index</param>
        /// <param name="tableTransform"></param>
        /// <param name="deskGrid"></param>        
        public void Setup(Vector3 cellCenter, float cellLength, float cellWitdh, int cellRow, int cellColumn, Transform tableTransform, DeskGrid deskGrid) {

            //Set the row and the column index
            this.cellRow = cellRow;
            this.cellColumn = cellColumn;

            //Set the cell rotation
            gameObject.transform.rotation = Quaternion.Euler(0f, tableTransform.rotation.eulerAngles.y, 0f);

            //Set the cell scale
            gameObject.transform.localScale = new Vector3(tableTransform.localScale.x, tableTransform.localScale.y, 1);

            //Add the Box Collider to the Game Object
            boxCollider = gameObject.AddComponent<BoxCollider>();

            //Add the FocusHandler to detect whether the user is looking at the cell
            focusHandler = gameObject.AddComponent<FocusHandler>();

            //Compute the cell center in local coordinates starting from the global ones
            CenterCoordinates = cellCenter + new Vector3(0, 0.005f, 0);

            //Resize the game object and the collider in order to keep each cell grid separate from the others
            StackSize = 0.9f * new Vector3(cellLength, 0.005f, 1.5f * cellWitdh);

            //Make the collider detect collision in order to trigger the methods OnTriggerEnter and OnTriggerExit
            boxCollider.isTrigger = true;

            //Disable the collider by default
            boxCollider.enabled = false;

            grid = deskGrid.Grid;

            verticalStack = new List<string>();
        }

        /// <summary>
        /// Set the current move to the cell
        /// </summary>
        /// <param name="preposition">The preposition representing the offset from the cell</param>
        /// <param name="targetObject">The object to place</param>
        public void SetMove(Prepositions preposition, string targetObject) {
            TargetPreposition = preposition;
            TargetObject = targetObject;
        }

        /// <summary>
        /// Start to listen for collisions.
        /// </summary>
        internal void ActivateCell() {
            //Activate the collider
            boxCollider.enabled = true;
            //Add the listener to the OnFocusEvent in order to allow the user to pick the objects
            focusHandler.OnFocusEnterEvent.AddListener(() => StartCoroutine(MoveDownCollider()));
        }

        /// <summary>
        /// Reduce the collider position in order to pick the last object on the stack.
        /// </summary>
        /// <returns></returns>
        private IEnumerator MoveDownCollider() {
            //If the user is not already looking at the cell and the stack contains some object
            if (finishedFocusEnter && verticalStack.Count > 0) {
                //Prevent that the code is executed by other concurrent events
                finishedFocusEnter = false;
                //Compute the new game object position (half of its height)
                float decrementCenterY = ObjectPooler.GetPooler().GetPooledObject(verticalStack[verticalStack.Count - 1]).GetComponent<Renderer>().bounds.size.y / 2;
                //Set the new coordinates
                CenterCoordinates -= new Vector3(0, decrementCenterY, 0);
                //Allow the user to pick up the object before restoring the collider position
                yield return new WaitForSeconds(2);
                //Restore the initial cell position
                CenterCoordinates += new Vector3(0, decrementCenterY, 0);
                //Allow other code executions
                finishedFocusEnter = true;
            }
        }

        /// <summary>
        /// Add the collider's game object to the stack and move the collider on top of it.
        /// </summary>
        /// <param name="other">the entering game object collider</param>
        private void OnTriggerEnter(Collider other) {
            //If the object is not already contained into the stack and it is not the table ("Cube")
            if (!verticalStack.Contains(other.gameObject.name) && !Equals(other.gameObject.name, "Cube")) {
                Debug.Log(other.gameObject.name + " Entered the cell");

                //Add the object to the stack
                verticalStack.Add(other.gameObject.name);

                //Compute the new game object position (add its height)
                float incrementCenterY = other.gameObject.GetComponent<Renderer>().bounds.size.y;

                //Set the new coordinates
                CenterCoordinates += new Vector3(0, incrementCenterY, 0);

                //Trigger the event to check if the object is in the right cell
                TriggerEvent(Triggers.ObjectPositioning);


                //This code is left for future work purposes: modify also the collider size 
                //in order to allow the user only to pick up the latest object on the stack.

                //float incrementSizeY = objectBounds.size.y / 2;
                //boxCollider.size += new Vector3(0, incrementSizeY, 0);
            }

        }

        /// <summary>
        /// Remove the collider's game object from the stack and adjust the collider position
        /// </summary>
        /// <param name="other">the exiting game object collider</param>
        private void OnTriggerExit(Collider other) {
            //If the object is not already contained into the stack and it is not the table ("Cube")
            if (verticalStack.Contains(other.gameObject.name) && !Equals(other.gameObject.name, "Cube")) {
                Debug.Log(other.gameObject.name + " Exited the cell");

                //Remove the object from the stack
                verticalStack.Remove(other.gameObject.name);

                //Compute the new game object position (subtract its height)
                float decrementCenterY = other.gameObject.GetComponent<Renderer>().bounds.size.y;

                //Set the new coordinates
                CenterCoordinates -= new Vector3(0, decrementCenterY, 0);

                //This code is left for future work purposes: modify also the collider size 
                //in order to allow the user only to pick up the latest object on the stack.

                //float decrementSizeY = objectBounds.size.y / 2;
                //boxCollider.size -= new Vector3(0, decrementSizeY, 0);
            }
        }

        /// <summary>
        /// Listen to object's movement to a new grid cell
        /// </summary>
        public void ListenForPositioning() => StartListening(Triggers.ObjectPositioning, CheckObjectPositionCoroutine);

        /// <summary>
        /// Stop listen to object's movement
        /// </summary>
        public void StopListenForPositioning() => StopListening(Triggers.ObjectPositioning, CheckObjectPositionCoroutine);

        /// <summary>
        /// Check if in the cells suggested by the preposition there is the target object and triggers the VA resposne.
        /// </summary>
        private void CheckObjectPositionCoroutine() {
            StartCoroutine(CheckObjectPosition());
        }

        private IEnumerator CheckObjectPosition() {
            if (!isExecuting) {
                isExecuting = true;
                bool found = false;

                //Compute the cells' offset to check
                List<Tuple<int, int>> cellsToCheck = FindCellsToCheck(TargetPreposition);

                //For each possible offset
                foreach (Tuple<int, int> cellOffset in cellsToCheck) {
                    //If the corresponding cell contains the target object
                    if (!found && grid[cellRow + cellOffset.Item1, cellColumn + cellOffset.Item2].Contains(TargetObject)) {
                        //Stop listen for object positioning event
                        StopListenForPositioning();

                        ObjectPooler.GetPooler().GetPooledObject(TargetObject).GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                        Destroy(ObjectPooler.GetPooler().GetPooledObject(TargetObject).GetComponent<ManipulationHandler>());
                        Destroy(ObjectPooler.GetPooler().GetPooledObject(TargetObject).GetComponent<NearInteractionGrabbable>());

                        found = true;
                        //Trigger the VA position
                        TriggerEvent(Triggers.VAOk);
                        yield return new WaitForSeconds(3);
                        //Trigger the handler for the next iteration
                        TriggerEvent(Triggers.CorrectPositioning);
                    }
                }

                if (!found)
                    //Trigger the VA negative reaction: the object has not been found
                    TriggerEvent(Triggers.VAKo);
                isExecuting = false;
            }
        }

        /// <summary>
        /// Checks wheter the object is in current cell
        /// </summary>
        /// <param name="targetObject"></param>
        /// <returns>True if it is in the cell, otherwise false</returns>
        public bool Contains(string targetObject) => verticalStack.Contains(targetObject);

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
                //case Prepositions.On:
                //    offsets.Add(new Tuple<int, int>(0, 0));
                //    break;
                default: throw new NotImplementedException();
            }
            return offsets;
        }

        /// <summary>
        /// Returns the string associated to a prepositon
        /// </summary>
        /// <param name="targetPreposition"></param>
        /// <returns></returns>
        public static string PrepositionAsString(Prepositions targetPreposition) {
            switch (targetPreposition) {
                case Prepositions.Behind:
                    return "Behind";
                case Prepositions.InFrontOf:
                    return "InFrontOf";
                case Prepositions.NextTo:
                    return "NextTo";
                //case Prepositions.On:
                //    return "On";
                default: throw new NotImplementedException();
            }
        }
    }
}
