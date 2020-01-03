// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Microsoft.MixedReality.Toolkit.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HoloToolkit.Unity.SpatialMapping.Tests
{
    /// <summary>
    /// The SpatialProcessingTest class allows applications to scan the environment for a specified amount of time 
    /// and then process the Spatial Mapping Mesh (find planes, remove vertices) after that time has expired.
    /// </summary>
    public class SpatialProcessingTest : Singleton<SpatialProcessingTest>
    {
        [Tooltip("How much time (in seconds) that the SurfaceObserver will run after being started; used when 'Limit Scanning By Time' is checked.")]
        public float scanTime = 30.0f;

        [Tooltip("Material to use when rendering Spatial Mapping meshes while the observer is running.")]
        public Material defaultMaterial;

        [Tooltip("Optional Material to use when rendering Spatial Mapping meshes after the observer has been stopped.")]
        public Material secondaryMaterial;

        [Tooltip("Minimum number of floor planes required in order to exit scanning/processing mode.")]
        public uint minimumFloors = 1;

        [Tooltip("Minimum number of table planes required in order to exit scanning/processing mode.")]
        public uint minimumTables = 0;

        // LISTA DI PIANI
        public List<GameObject> floors = new List<GameObject>();
        public List<GameObject> tables = new List<GameObject>();

        //Wait Button
        private GameObject waitButton;

        /// <summary>
        /// Indicates if processing of the surface meshes is complete.
        /// </summary>
        private bool meshesProcessed = false;

        /// <summary>
        /// GameObject initialization.
        /// </summary>
        private void Start()
        {
            // Update surfaceObserver and storedMeshes to use the same material during scanning.
            SpatialMappingManager.Instance.SetSurfaceMaterial(defaultMaterial);

            // Register for the MakePlanesComplete event.
            SurfaceMeshesToPlanes.Instance.MakePlanesComplete += SurfaceMeshesToPlanes_MakePlanesComplete;

            //Create the button
            waitButton = (GameObject)Instantiate(Resources.Load("Prefab/Buttons/WaitButton"));
        }

        /// <summary>
        /// Called once per frame.
        /// </summary>
        private void Update()
        {
            // Check to see if the spatial mapping data has been processed yet.
            if (!meshesProcessed)
            {
                // Check to see if enough scanning time has passed
                // since starting the observer.
                if ((Time.unscaledTime - SpatialMappingManager.Instance.StartTime) < scanTime)
                {
                    // If we have a limited scanning time, then we should wait until
                    // enough time has passed before processing the mesh.
                }
                else
                {
                    // The user should be done scanning their environment,
                    // so start processing the spatial mapping data...

                    if (SpatialMappingManager.Instance.IsObserverRunning())
                    {
                        // Stop the observer.
                        SpatialMappingManager.Instance.StopObserver();
                    }

                    // Call CreatePlanes() to generate planes.
                    CreatePlanes();

                    // Set meshesProcessed to true.
                    meshesProcessed = true;
                }
            }
        }

        /// <summary>
        /// Handler for the SurfaceMeshesToPlanes MakePlanesComplete event.
        /// </summary>
        /// <param name="source">Source of the event.</param>
        /// <param name="args">Args for the event.</param>
        private void SurfaceMeshesToPlanes_MakePlanesComplete(object source, System.EventArgs args)
        {
            // Collection of floor planes that we can use to set horizontal items on.
            floors = SurfaceMeshesToPlanes.Instance.GetActivePlanes(PlaneTypes.Floor);

            // Collection of table planes that we can use to set horizontal items on.
            tables = SurfaceMeshesToPlanes.Instance.GetActivePlanes(PlaneTypes.Table);

            // Check to see if we have enough floors (minimumFloors) to start processing.
            if (floors.Count >= minimumFloors && tables.Count >= minimumTables)
            {
                // Reduce our triangle count by removing any triangles
                // from SpatialMapping meshes that intersect with active planes.
                RemoveVertices(SurfaceMeshesToPlanes.Instance.ActivePlanes);

                // After scanning is over, switch to the secondary (occlusion) material.
                SpatialMappingManager.Instance.SetSurfaceMaterial(secondaryMaterial);

                Debug.Log("Scanning complete");

                //Delete the wait button
                Destroy(waitButton);
                //Setup each table and floor in the scene in order to capture a tap event
                SetupFloorsAndTables();
                GameObject.Find("SceneManager").GetComponent<SceneStarter>().WaitForUserTap();
            }
            else
            {
                // Re-enter scanning mode so the user can find more surfaces before processing.
                SpatialMappingManager.Instance.StartObserver();

                // Re-process spatial data after scanning completes.
                meshesProcessed = false;
            }
        }

        //For each table and floor in the scene add an Interactable component and execute handleTap() whenever the surface is tapped
        private void SetupFloorsAndTables()
        {
            foreach (GameObject floor in floors)
            {
                Interactable interactable = floor.AddComponent<Interactable>();
                interactable.AddReceiver<InteractableOnPressReceiver>().OnPress.AddListener(() => handleTap(floor));
            }
            foreach (GameObject table in tables)
            {
                Interactable interactable = table.AddComponent<Interactable>();
                interactable.AddReceiver<InteractableOnPressReceiver>().OnPress.AddListener(() => handleTap(table));
            }
        }

        //When the user taps on a surface:
        //1) Start the Activity
        //2) Remove the Interactable component from the surfaces
        private void handleTap(GameObject floor)
        {
            GameObject.Find("SceneManager").GetComponent<SceneStarter>().StartActivity();
            RemoveReceiversFromFloorsAndTables();
        }

        // Remove the Interactable component from the surfaces
        private void RemoveReceiversFromFloorsAndTables()
        {
            foreach (GameObject floor in floors)
            {
                Destroy(floor.GetComponent<Interactable>());
            }
            foreach (GameObject table in tables)
            {
                Destroy(table.GetComponent<Interactable>());
            }
        }

        /// <summary>
        /// Creates planes from the spatial mapping surfaces.
        /// </summary>
        private void CreatePlanes()
        {
            // Generate planes based on the spatial map.
            SurfaceMeshesToPlanes surfaceToPlanes = SurfaceMeshesToPlanes.Instance;
            if (surfaceToPlanes != null && surfaceToPlanes.enabled)
            {
                surfaceToPlanes.MakePlanes();
            }
        }

        /// <summary>
        /// Removes triangles from the spatial mapping surfaces.
        /// </summary>
        /// <param name="boundingObjects"></param>
        private void RemoveVertices(IEnumerable<GameObject> boundingObjects)
        {
            RemoveSurfaceVertices removeVerts = RemoveSurfaceVertices.Instance;
            if (removeVerts != null && removeVerts.enabled)
            {
                removeVerts.RemoveSurfaceVerticesWithinBounds(boundingObjects);
            }
        }

        /// <summary>
        /// Called when the GameObject is unloaded.
        /// </summary>
        protected override void OnDestroy()
        {
            if (SurfaceMeshesToPlanes.Instance != null)
            {
                SurfaceMeshesToPlanes.Instance.MakePlanesComplete -= SurfaceMeshesToPlanes_MakePlanesComplete;
            }

            base.OnDestroy();
        }
    }
}