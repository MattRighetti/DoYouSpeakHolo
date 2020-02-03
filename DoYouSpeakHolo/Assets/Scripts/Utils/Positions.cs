using HoloToolkit.Unity.SpatialMapping;
using HoloToolkit.Unity.SpatialMapping.Tests;
using UnityEngine;

//  Class responsible of object placement in the scene with respect to 
//  the initial position given by the user tap
public class Positions { 
    // User gaze coordinates after the Spatial processing scan
    Vector3 gazePosition;

    // Used to calculate the offset positions of the objects from the gazePosition
    Transform gazeTransform = new GameObject("HitPoint").transform;

    //  Floor coordinates
    public Vector3 floorPosition;

    //  Table coordinates
    public Vector3 tablePosition;

    public static readonly float FrontDistance = -0.9f;

    public static readonly Vector3 Central = new Vector3(0, 0, FrontDistance + 1.2f);
    public static readonly Vector3 AsideLeft = new Vector3(-0.2f, 0, FrontDistance + 1);
    public static readonly Vector3 AsideRight = new Vector3(0.2f, 0, FrontDistance + 1);

    //  Scene 2
    public static readonly Vector3 ArkPosition = new Vector3(0.018f, 0, 0.6f);
    public static readonly Vector3 startPositionInlineThree = new Vector3(-0.1f, 0, 0.1f);

    //Scene 3
    public static readonly Vector3 TreePosition = new Vector3(0, 0, FrontDistance + 1.6f);
    public static readonly Vector3 HousePosition = new Vector3(-0.7f, 0, FrontDistance + 1.7f);
    public static readonly Vector3 Character1Position = new Vector3(-0.375f, 0, FrontDistance + 1.1f);
    public static readonly Vector3 Character1Basket = new Vector3(-0.375f, 0, FrontDistance + 0.95f);
    public static readonly Vector3 Character2Position = new Vector3(0.375f, 0, FrontDistance + 1.1f);
    public static readonly Vector3 Character2Basket = new Vector3(0.375f, 0, FrontDistance + 0.9f);
    public static readonly Vector3 VAPosition = new Vector3(-0.4f, 0f, FrontDistance + 0.7f);

    //  Default position for non active objects
    public static readonly Vector3 hiddenPosition = new Vector3(0, 0, FrontDistance - 3);

    //  Start position for spawning 4 objects aligned in scene1
    public static readonly Vector3 startPositionInlineFour = new Vector3(-0.3f, 0, FrontDistance + 0.8f);
    public static readonly Vector3 CentralNear = new Vector3(0, 0, FrontDistance + 1);

    //Rotation to make the objects be oriented towards the user
    public static Quaternion ObjectsRotation = new Quaternion();
    private float height;

    public DeskGrid Grid { get; private set; }
    public Transform TableTransform { get; set; }
    public bool UseTable { get; private set; }

    //  Compute the object position with respect to the gazePosition and the floorPosition
    public Vector3 GetPosition(Vector3 position) {
        if (!UseTable) { 
            //  This is the correct way to deal with local coordinates
            position = gazeTransform.TransformPoint(position);
            position.y = height;
        }
        else {
            
            position.y = height + 0.02f;
        }
        return position;
    }

    //  Determine gazePosition and FloorPosition according to the user gaze
    public void FindFloor() {
        UseTable = false;
        Transform floor = SpatialProcessingTest.Instance.floors[0].transform;

        DebugDrawPlanes(floor);

        SurfacePlane plane = floor.GetComponent<SurfacePlane>();
        floorPosition = floor.transform.position + (plane.PlaneThickness * plane.SurfaceNormal);
        floorPosition = AdjustPositionWithSpatialMap(floorPosition, plane.SurfaceNormal);
        gazePosition = new Vector3(0f, 0f, 0f);

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hitInfo, 20f, Physics.DefaultRaycastLayers)) {
            gazePosition = hitInfo.point;
        }

        //  The reference position is the gazePosition
        gazeTransform.position = gazePosition;
        //  The rotation is given by the direction where I'm looking at
        Vector3 relativePos = gazePosition - Camera.main.transform.position;
        Quaternion rotation = Quaternion.LookRotation(relativePos);
        //  Now it's parallel to the floor
        rotation.x = 0f;
        rotation.z = 0f;
        gazeTransform.rotation = rotation;

        //  The objects are oriented towards the user. Same procedure as before, but rotation "flipped"
        relativePos = Camera.main.transform.position - gazePosition;
        rotation = Quaternion.LookRotation(relativePos);
        rotation.x = 0f;
        rotation.z = 0f;
        ObjectsRotation = rotation;
        height = floorPosition.y;
    }

    //  Determine gazePostion and TablePosition according to the user gaze
    public void FindTable() {
        UseTable = true;
        TableTransform = SpatialProcessingTest.Instance.tables[0].transform;

        DebugDrawPlanes(TableTransform);

        SurfacePlane plane = TableTransform.GetComponent<SurfacePlane>();

        tablePosition = TableTransform.transform.position + (plane.PlaneThickness * plane.SurfaceNormal);
        tablePosition = AdjustPositionWithSpatialMap(tablePosition, plane.SurfaceNormal);
        gazePosition = new Vector3(0f, 0f, 0f);


        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hitInfo, 20f, Physics.DefaultRaycastLayers)) {
            gazePosition = hitInfo.point;
        }

        gazeTransform.position = gazePosition;

        Vector3 relativePos = gazePosition - Camera.main.transform.position;
        Quaternion rotation = Quaternion.LookRotation(relativePos);

        rotation.x = 0f;
        rotation.z = 0f;

        gazeTransform.rotation = rotation;

        relativePos = Camera.main.transform.position - gazePosition;

        rotation = Quaternion.LookRotation(relativePos);
        rotation.x = 0f;
        rotation.z = 0f;
        ObjectsRotation = rotation;
        height = tablePosition.y;
        Grid = (new SimpleGridGenerator(TableTransform, height)).Grid;
    }

    private Bounds GetColliderBounds(Transform transform) {
        return transform.GetComponent<Collider>().bounds;
    }

    private void DebugDrawPlanes(Transform transformObject) {
        Bounds tableColliderBounds = GetColliderBounds(transformObject);

        Vector3 tableEdge1 = transformObject.TransformPoint(0.4f, 0f, 0f);
        Vector3 tableEdge2 = transformObject.TransformPoint(-0.4f, 0f, 0f);
        Vector3 tableEdge3 = transformObject.TransformPoint(0f, 0.4f, 0f);
        Vector3 tableEdge4 = transformObject.TransformPoint(0f, -0.4f, 0f);

        Debug.DrawLine(tableEdge1, tableColliderBounds.center, Color.black, 30f);
        Debug.DrawLine(tableEdge2, tableColliderBounds.center, Color.black, 30f);
        Debug.DrawLine(tableEdge3, tableColliderBounds.center, Color.black, 30f);
        Debug.DrawLine(tableEdge4, tableColliderBounds.center, Color.black, 30f);
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