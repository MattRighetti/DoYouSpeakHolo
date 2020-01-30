// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using HoloToolkit.Unity.SpatialMapping;
using UnityEngine;


public class SimpleGridGenerator : MonoBehaviour {

    /// <summary>
    /// Default rows
    /// </summary>
    private const int DefaultRows = 4;

    /// <summary>
    /// Default columns
    /// </summary>
    private const int DefaultColumns = 3;

    /// <summary>
    /// Distance separating two objects on the grid (X axis)
    /// </summary>
    private readonly float cellLength;
    
    /// <summary>
    /// Distance separating two objects on the grid (Z axis)
    /// </summary>
    private readonly float cellWitdh;

    private readonly Transform tableTransform;
    private readonly float tableHeight;
    [Tooltip("Number of rows in the grid.")]
    public int Rows = DefaultRows;

    [Tooltip("Number of columns in the grid.")]
    public int Columns = DefaultColumns;

    [Tooltip("Distance between objects in the grid.")]
    public float ObjectSpacing = 0.2f;

    /// <summary>
    /// Coordinates of the vertice in the upper left corner
    /// </summary>
    private Vector3 upperLeftVertice;


    public SimpleGridGenerator(Transform tableTransform, float tableHeight) {
        this.tableTransform = tableTransform;
        this.tableHeight = tableHeight;
        SurfacePlane plane = tableTransform.GetComponent<SurfacePlane>();
        Mesh mesh = plane.gameObject.GetComponent<MeshFilter>().sharedMesh;
        upperLeftVertice = mesh.vertices[0];
        Bounds tableBounds = mesh.bounds;
        cellLength = (tableBounds.max.x - tableBounds.min.x) / Columns;
        cellWitdh = (tableBounds.max.z - tableBounds.min.z) / Rows;

    }

    public DeskGrid GenerateGrid() {

        DeskGrid grid = new DeskGrid(tableTransform, Rows, Columns);

        float halfLength = cellLength / 2;
        float halfWidth = cellWitdh / 2;

        //Start from the upper left square center
        float rowStart = upperLeftVertice.x + halfLength;
        float columnStart = upperLeftVertice.z + halfWidth;


        for (int row = 0; row < Rows; row++) {
            for (int column = 0; column < Columns; column++) {

                Vector3 cellCenter = new Vector3(rowStart + row * cellLength, columnStart - column * cellWitdh, tableHeight);
                grid.AddCell(cellCenter, row, column, cellLength, cellWitdh);
            }
        }

        return grid;
    }
}
