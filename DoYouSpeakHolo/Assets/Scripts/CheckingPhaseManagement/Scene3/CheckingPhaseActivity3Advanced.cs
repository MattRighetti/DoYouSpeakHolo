using UnityEngine;

/// <summary>
/// Class responsible of the Advanced Checking Phase version of Activity 3.
/// </summary>
class CheckingPhaseActivity3Advanced : CheckingPhaseActivity3 {

    /// <summary>
    /// Creates people, their baskets and spawn the fruits inline
    /// </summary>
    protected override void CheckingPhase() {
        possessivesManager = (PossessivesManager)sceneManager;

        //  Create the baskets and attach to them the script
        CreatePeopleAndBaskets();

        //  Spawn Fruits in random order
        CreateAllObjectsAndDisplayInRandomOrder();
    }

    /// <summary>
    /// Creates the fruits, shuffle them and then spawn inline
    /// </summary>
    protected void CreateAllObjectsAndDisplayInRandomOrder() {
        //  Shuffle the collection
        SceneObjects = AbstractSceneManager.Shuffle(SceneObjects);
        GameObject gameObj;

        //  Define initial spawning position
        Vector3 startPosition = Positions.startPositionInlineFour;
        foreach (string obj in SceneObjects) {
            //  Activate the object and attach to it the script for the task
            gameObj = possessivesManager.ActivateObject(obj, startPosition, Positions.ObjectsRotation);
            //  Determine the new fruit position
            startPosition += new Vector3(0.2f, 0, 0);
        }
    }
}