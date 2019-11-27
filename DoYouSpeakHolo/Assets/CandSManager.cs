using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.SpatialAwareness;
using Microsoft.MixedReality.Toolkit.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CandSManager : MonoBehaviour
{
    //Object init
    private Vector3 KeyPosition;
    private Vector3 HousePosition;
    private Vector3 TreePosition;
    private Vector3 ApplePosition;
    
    public GameObject Key;
    public GameObject House;
    public GameObject Tree;
    public GameObject Apple;

    private bool learning = false;

    private List<GameObject> GameObjects;
    
    private IMixedRealitySpatialAwarenessMeshObserver SpatialObjectMeshObserver;
    private static int _meshPhysicsLayer = 0;

    private bool IsObserverRunning
    {
        get
        {
            var providers =
              ((IMixedRealityDataProviderAccess)CoreServices.SpatialAwarenessSystem)
                .GetDataProviders<IMixedRealitySpatialAwarenessObserver>();
            return providers.FirstOrDefault()?.IsRunning == true;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //Enable spatial mapping
        ToggleSpatialMap();
        GenerateAndPlaceObjects();

    }

    //  Enable/Disable Spatial Mapping
    public void ToggleSpatialMap()
    {
        if (CoreServices.SpatialAwarenessSystem != null)
        {
            if (IsObserverRunning)
            {
                Debug.Log("Disabling spatial mapping");
                CoreServices.SpatialAwarenessSystem.SuspendObservers();
                CoreServices.SpatialAwarenessSystem.ClearObservations();
            }
            else
            {
                Debug.Log("Enabling spatial mapping");
                CoreServices.SpatialAwarenessSystem.ResumeObservers();
            }
        }
    }

    // Get the position on the spatial map using a Raycast that hits the mesh
    public static Vector3? GetPositionOnSpatialMap(float maxDistance = 2)
    {
        RaycastHit hitInfo;
        var transform = CameraCache.Main.transform;
        var headRay = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(headRay, out hitInfo, maxDistance, GetSpatialMeshMask()))
        {
            Debug.Log("Point found");
            Debug.Log(hitInfo.point);
            return hitInfo.point;
        }
        return null;
    }

    // Retrieve the mesh mask needed by the Raycast
    private static int GetSpatialMeshMask()
    {
        if (_meshPhysicsLayer == 0)
        {
            var spatialMappingConfig =
              CoreServices.SpatialAwarenessSystem.ConfigurationProfile as
                MixedRealitySpatialAwarenessSystemProfile;
            if (spatialMappingConfig != null)
            {
                foreach (var config in spatialMappingConfig.ObserverConfigurations)
                {
                    var observerProfile = config.ObserverProfile
                        as MixedRealitySpatialAwarenessMeshObserverProfile;
                    if (observerProfile != null)
                    {
                        _meshPhysicsLayer |= (1 << observerProfile.MeshPhysicsLayer);
                    }
                }
            }
        }
        return _meshPhysicsLayer;
    }

    // Update is called once per frame
    void Update()
    {
        //Start the activity only at the first frame
        if (!learning)
        {
            learning = true;
            LearningPhase();
        }
    }

    //Read the data from spatial mapping in order to understand where to place objects
    void GenerateAndPlaceObjects()
    {
        GameObjects = new List<GameObject>();
        GameObjects.Add(House);
        GameObjects.Add(Tree);
        GameObjects.Add(Key);
        GameObjects.Add(Apple);
        initiateScene();
    }

    private void initiateScene()
    {

        //Disable spatial mapping
        ToggleSpatialMap();


    }

    //First phase of the activity, the virtual assistant shows to the user some objects and tells their name
    private void LearningPhase()
    {
        //Let the virtual assistant speak

        
        Vector3 centralPosition = new Vector3(0, 0, 2);

        StartCoroutine(ShowObjects(centralPosition));
        


    }

    IEnumerator ShowObjects(Vector3 centralPosition)
    {
        foreach (GameObject Obj in GameObjects)
        {
            StartCoroutine(ShowObject(Obj, centralPosition));
            yield return new WaitForSeconds(3);
        }
    }

    //Spawn the objects in front of the user and destroy them after a timeout
    IEnumerator ShowObject(GameObject Obj, Vector3 centralPosition)
    {
        GameObject Object = Instantiate(Obj) as GameObject;
        Object.transform.position = centralPosition;
        yield return new WaitForSeconds(2);
        Destroy(Object);
    }


}




