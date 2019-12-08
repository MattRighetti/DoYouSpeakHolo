using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Positions
{

    public static readonly Vector3 Central = new Vector3(0, 0, 1.2f);
    public static readonly Vector3 AsideLeft = new Vector3(-0.2f, 0, 1);
    public static readonly Vector3 AsideRight = new Vector3(0.2f, 0, 1);

    //Scene 3
    public static readonly Vector3 TreePosition = new Vector3(0, 0, 1.8f);
    public static readonly Vector3 HousePosition = new Vector3(-1.3f, 0, 1.9f);
    public static readonly Vector3 MalePosition = new Vector3(-0.425f, 0, 1.05f);
    public static readonly Vector3 MaleBasket = new Vector3(-0.425f, 0, 0.9f);
    public static readonly Vector3 FemalePosition = new Vector3(0.425f, 0, 1.05f);
    public static readonly Vector3 FemaleBasket = new Vector3(0.425f, 0, 0.9f);
    public static readonly Vector3 VAPosition = new Vector3(-0.82f, 0, 0.5f);

    //Default position for non active objects
    public static readonly Vector3 hiddenPosition = new Vector3(0, 0, -3);

    //Start position for spawning 4 objects aligned in scene1
    public static readonly Vector3 startPositionInlineFour = new Vector3(-0.75f, 0, 0.8f);
}
