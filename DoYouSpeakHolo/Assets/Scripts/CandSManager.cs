using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.SpatialAwareness;
using Microsoft.MixedReality.Toolkit.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CandSManager : MonoBehaviour {
    //Maybe not needed anymore
    private bool Learning = false;

    //TODO: add MRTK V1 to do spatial mapping
    private IMixedRealitySpatialAwarenessMeshObserver SpatialObjectMeshObserver;
    private static int _meshPhysicsLayer = 0;

    private bool IsObserverRunning {
        get {
            var providers =
              ((IMixedRealityDataProviderAccess)CoreServices.SpatialAwarenessSystem)
                .GetDataProviders<IMixedRealitySpatialAwarenessObserver>();
            return providers.FirstOrDefault()?.IsRunning == true;
        }
    }

    // Start is called before the first frame update
    void Start() {
        gameObject.GetComponent<LearningPhaseManager>().SetScene(LearningPhaseManager.ScenesEnum.Scene1);
        //Enable spatial mapping
        //ToggleSpatialMap();

        //TODO: add Spatial Mapping
        //GenerateAndPlaceObjects();
    }

    //  Enable/Disable Spatial Mapping
    public void ToggleSpatialMap() {
        if (CoreServices.SpatialAwarenessSystem != null) {
            if (IsObserverRunning) {
                Debug.Log("Disabling spatial mapping");
                CoreServices.SpatialAwarenessSystem.SuspendObservers();
                CoreServices.SpatialAwarenessSystem.ClearObservations();
            }
            else {
                Debug.Log("Enabling spatial mapping");
                CoreServices.SpatialAwarenessSystem.ResumeObservers();
            }
        }
    }

    // Get the position on the spatial map using a Raycast that hits the mesh
    public static Vector3? GetPositionOnSpatialMap(float maxDistance = 2) {
        RaycastHit hitInfo;
        var transform = CameraCache.Main.transform;
        var headRay = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(headRay, out hitInfo, maxDistance, GetSpatialMeshMask())) {
            Debug.Log("Point found");
            Debug.Log(hitInfo.point);
            return hitInfo.point;
        }
        return null;
    }

    // Retrieve the mesh mask needed by the Raycast
    private static int GetSpatialMeshMask() {
        if (_meshPhysicsLayer == 0) {
            var spatialMappingConfig =
              CoreServices.SpatialAwarenessSystem.ConfigurationProfile as
                MixedRealitySpatialAwarenessSystemProfile;
            if (spatialMappingConfig != null) {
                foreach (var config in spatialMappingConfig.ObserverConfigurations) {
                    var observerProfile = config.ObserverProfile
                        as MixedRealitySpatialAwarenessMeshObserverProfile;
                    if (observerProfile != null) {
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

        //TODO: find another way to start the flow of the activity
        if (!Learning)
        {
            EventManager.TriggerEvent("LearningPhaseStart");
            Learning = true;
        }
    }

    private void initiateScene() {
        //Disable spatial mapping
        ToggleSpatialMap();
    }
}

