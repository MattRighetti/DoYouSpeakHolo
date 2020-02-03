// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using HoloToolkit.Unity.SpatialMapping;
using System;
using UnityEngine;


public class SimpleGridGenerator {

    /// <summary>
    /// Default rows
    /// </summary>
    private const int DefaultRows = 3;

    /// <summary>
    /// Default columns
    /// </summary>
    private const int DefaultColumns = 4;

    /// <summary>
    /// Distance separating two objects on the grid (X axis)
    /// </summary>
    private readonly float cellLength;
    
    /// <summary>
    /// Distance separating two objects on the grid (Z axis)
    /// </summary>
    private readonly float cellWitdh;

    /// <summary>
    /// The table transform, used to pick the necessary positions and rotations
    /// </summary>
    private readonly Transform tableTransform;

    /// <summary>
    /// The table height
    /// </summary>
    private readonly float tableHeight;
    private readonly Bounds tableBounds;

    /// <summary>
    /// Contains the desk grid.
    /// </summary>
    public DeskGrid Grid { get; private set; }

    [Tooltip("Number of rows in the grid.")]
    public int Rows = DefaultRows;

    [Tooltip("Number of columns in the grid.")]
    public int Columns = DefaultColumns;

    /// <summary>
    /// Coordinates of the vertice in the upper left corner
    /// </summary>
    private Vector3 NWVertice;

    public Vector3 NEVertice { get; }
    public Vector3 SWVertice { get; }
    public Vector3 SEVertice { get; }

    public SimpleGridGenerator(Transform tableTransform, float tableHeight) {
        this.tableTransform = tableTransform;
        this.tableHeight = tableHeight;
        SurfacePlane plane = tableTransform.GetComponent<SurfacePlane>();

        //Define the grid center in the table local space
        Vector3 center = tableTransform.position;

        //Get the table bounds
        tableBounds = plane.gameObject.GetComponent<MeshFilter>().mesh.bounds;

        //Rescale the bounds to 80% for safety reason (avoid object fall down the table)
        float safetyBoundX = 0.8f * Math.Max(Math.Abs(tableBounds.size.x), Math.Abs(tableBounds.size.z));
        float safetyBoundZ = 0.8f * Math.Min(Math.Abs(tableBounds.size.x), Math.Abs(tableBounds.size.z));

        //Define width and length of a grid cell
        cellLength = safetyBoundX / Columns;
        cellWitdh = safetyBoundZ / Rows;

        //Compute the upper left vertice in the local space
        NWVertice = tableTransform.TransformPoint(new Vector3((-safetyBoundX / 2), safetyBoundZ / 2, tableHeight));
        NEVertice = tableTransform.TransformPoint(new Vector3(safetyBoundX / 2, (safetyBoundZ / 2), tableHeight));
        SWVertice = tableTransform.TransformPoint(new Vector3((-safetyBoundX / 2), -(safetyBoundZ / 2), tableHeight));
        SEVertice = tableTransform.TransformPoint(new Vector3(safetyBoundX / 2, -(safetyBoundZ / 2), tableHeight));

        //NWVertice = new Vector3((-safetyBoundX / 2), safetyBoundZ / 2, tableHeight);
        //NEVertice = new Vector3(safetyBoundX / 2, (safetyBoundZ / 2), tableHeight);
        //SWVertice = new Vector3((-safetyBoundX / 2), -(safetyBoundZ / 2), tableHeight);
        //SEVertice = new Vector3(safetyBoundX / 2, -(safetyBoundZ / 2), tableHeight);


        Debug.Log("nw" + NWVertice.ToString());
        Debug.Log("sw" + SWVertice.ToString());
        Debug.Log("ne" + NEVertice.ToString());
        Debug.Log("SE" + SEVertice.ToString());

        //Generate the grid
        GenerateGrid();
    }

    /// <summary>
    /// Generate a grid on the table
    /// </summary>
    private void GenerateGrid() {

        Grid = new DeskGrid(tableTransform, Rows, Columns, cellLength, cellWitdh);

        float halfLength = cellLength / 2;
        float halfWidth = cellWitdh / 2;

        //Start from the upper left square center
        float rowStart = NWVertice.z - halfWidth;
        float columnStart = NWVertice.x + halfLength;


        for (int row = 0; row < Rows; row++) {
            for (int column = 0; column < Columns; column++) {
                float row_k = (float)column / (Columns - 1);
                float col_k = (float)row / (Rows - 1);
                float row_Pos = Vector3.Lerp(NWVertice, NEVertice, row_k).x;
                float col_pos = Vector3.Lerp(NWVertice, SWVertice, col_k).z;
                //Compute the new cell center in the local space
                Vector3 cellCenter = new Vector3(row_Pos, tableHeight, col_pos);
                Debug.Log("cell center " + cellCenter.ToString());
                Grid.AddCell(cellCenter, row, column);
            }
        }
    }
}
