using HoloToolkit.Unity.SpatialMapping;
using HoloToolkit.Unity.SpatialMapping.Tests;
using UnityEngine;

//  Class responsible of object placement in the scene with respect to 
//  the initial position given by the user tap
public class Positions
{
    // User gaze coordinates after the Spatial processing scan
    Vector3 gazePosition;

    //  Floor coordinates
    public Vector3 floorPosition;

    public static readonly float FrontDistance = 0.1f;

    public static readonly Vector3 Central = new Vector3(0, 0, FrontDistance + 1.2f);
    public static readonly Vector3 AsideLeft = new Vector3(-0.2f, 0, FrontDistance + 1);
    public static readonly Vector3 AsideRight = new Vector3(0.2f, 0, FrontDistance + 1);

    //Scene 3
    public static readonly Vector3 TreePosition = new Vector3( 0, 0, FrontDistance + 1.9f);
    public static readonly Vector3 HousePosition = new Vector3(-1.3f, 0, FrontDistance + 2);
    public static readonly Vector3 MalePosition = new Vector3(-0.425f, 0, FrontDistance + 1.1f);
    public static readonly Vector3 MaleBasket = new Vector3(-0.425f, 0, FrontDistance + 1);
    public static readonly Vector3 FemalePosition = new Vector3(0.425f, 0, FrontDistance + 1.1f);
    public static readonly Vector3 FemaleBasket = new Vector3(0.425f, 0, FrontDistance + 1);
    public static readonly Vector3 VAPosition = new Vector3(-0.6f, 0.3f, FrontDistance + 0.8f);

    //Default position for non active objects
    public static readonly Vector3 hiddenPosition = new Vector3(0, 0, FrontDistance - 3);

    //Start position for spawning 4 objects aligned in scene1
    public static readonly Vector3 startPositionInlineFour = new Vector3(-0.3f, 0, FrontDistance + 0.8f);
    public static readonly Vector3 CentralNear = new Vector3(0, 0, FrontDistance + 1);

    // Compute the object position with respect to the gazePosition and the floorPosition
    public Vector3 GetPosition(Vector3 position) {
        position = gazePosition + position;
        position.y = floorPosition.y;
        return position;
    }

    //  Determine gazePosition and FloorPosition according to the user gaze
    public void FindFloor() {
        Transform floor = SpatialProcessingTest.Instance.floors[0].transform;
        SurfacePlane plane = floor.GetComponent<SurfacePlane>();
        System.Random rnd = new System.Random();
        floorPosition = floor.transform.position + (plane.PlaneThickness * plane.SurfaceNormal);
        floorPosition = AdjustPositionWithSpatialMap(floorPosition, plane.SurfaceNormal);
        gazePosition = new Vector3(0f, 0f, 0f);
        RaycastHit hitInfo;

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hitInfo, 20f, Physics.DefaultRaycastLayers)) {
            gazePosition = hitInfo.point;
        }       
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
}
