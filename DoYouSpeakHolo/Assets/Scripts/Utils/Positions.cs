using HoloToolkit.Unity.SpatialMapping;
using HoloToolkit.Unity.SpatialMapping.Tests;
using UnityEngine;

public class Positions
{
    Vector3 gazePosition;
    public Vector3 floorPosition;
    private bool foundFloor = false;

    public static readonly float Floor = 0;
    public static readonly float SpaceFrontDistance = 0.1f;

    public static readonly Vector3 Central = new Vector3(0, Floor + 0, SpaceFrontDistance + 1.2f);
    public static readonly Vector3 AsideLeft = new Vector3(-0.2f, Floor + 0, SpaceFrontDistance + 1);
    public static readonly Vector3 AsideRight = new Vector3(0.2f, Floor + 0, SpaceFrontDistance + 1);

    //Scene 3
    public static readonly Vector3 TreePosition = new Vector3( 0, Floor + 0, SpaceFrontDistance + 1.9f);
    public static readonly Vector3 HousePosition = new Vector3(-1.3f, Floor + 0, SpaceFrontDistance + 2);
    public static readonly Vector3 MalePosition = new Vector3(-0.425f, Floor + 0, SpaceFrontDistance + 1.1f);
    public static readonly Vector3 MaleBasket = new Vector3(-0.425f, Floor + 0, SpaceFrontDistance + 1);
    public static readonly Vector3 FemalePosition = new Vector3(0.425f, Floor + 0, SpaceFrontDistance + 1.1f);
    public static readonly Vector3 FemaleBasket = new Vector3(0.425f, Floor + 0, SpaceFrontDistance + 1);
    public static readonly Vector3 VAPosition = new Vector3(-0.6f, Floor + 0.3f, SpaceFrontDistance + 0.8f);

    //Default position for non active objects
    public static readonly Vector3 hiddenPosition = new Vector3(0, Floor + 0, SpaceFrontDistance - 3);

    //Start position for spawning 4 objects aligned in scene1
    public static readonly Vector3 startPositionInlineFour = new Vector3(-0.3f, Floor + 0, SpaceFrontDistance + 0.8f);
    public static readonly Vector3 CentralNear = new Vector3(0, Floor + 0, SpaceFrontDistance + 1);

    public Vector3 GetPosition(Vector3 position) {
        position = gazePosition + position;
        position.y = floorPosition.y;
        return position;
    }
    
    protected virtual Vector3 AdjustPositionWithSpatialMap(Vector3 position, Vector3 surfaceNormal) {
        Vector3 newPosition = position;
        RaycastHit hitInfo;
        float distance = 0.5f;

        // Check to see if there is a SpatialMapping mesh occluding the object at its current position.
        if (Physics.Raycast(position, surfaceNormal, out hitInfo, distance, SpatialMappingManager.Instance.LayerMask)) {
            // If the object is occluded, reset its position.
            newPosition = hitInfo.point;
        }

        return newPosition;
    }

    public void FindFloor() {
        Transform floor = SpatialProcessingTest.Instance.floors[0].transform;
        SurfacePlane plane = floor.GetComponent<SurfacePlane>();

        System.Random rnd = new System.Random();
        floorPosition = floor.transform.position + (plane.PlaneThickness * plane.SurfaceNormal);
        floorPosition = AdjustPositionWithSpatialMap(floorPosition, plane.SurfaceNormal);
        foundFloor = true;
        gazePosition = new Vector3(0f, 0f, 0f);
        RaycastHit hitInfo;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hitInfo, 20f, Physics.DefaultRaycastLayers)) {
            gazePosition = hitInfo.point;
        }       
    }
    
}
