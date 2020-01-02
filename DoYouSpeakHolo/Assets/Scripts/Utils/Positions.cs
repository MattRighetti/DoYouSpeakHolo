using UnityEngine;

public class Positions
{
    public static readonly float Floor = -0.6f;
    public static readonly float SpaceFrontDistance = 1.5f;

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
    public static readonly Vector3 VAPosition = new Vector3(-0.6f, Floor + 0, SpaceFrontDistance + 0.8f);

    //Default position for non active objects
    public static readonly Vector3 hiddenPosition = new Vector3(0, Floor + 0, SpaceFrontDistance - 3);

    //Start position for spawning 4 objects aligned in scene1
    public static readonly Vector3 startPositionInlineFour = new Vector3(-0.3f, Floor + 0, SpaceFrontDistance + 0.8f);
    public static readonly Vector3 CentralNear = new Vector3(0, Floor + 0, SpaceFrontDistance + 1);
}
