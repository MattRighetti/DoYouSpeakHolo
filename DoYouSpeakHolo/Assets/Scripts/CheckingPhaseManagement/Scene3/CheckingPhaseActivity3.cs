
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Abstract class responsible of the Activity 3 Checking Phase.
/// </summary>
public abstract class CheckingPhaseActivity3 : CheckingPhaseManager
{
    protected PossessivesManager possessivesManager;
    Possessives character1Possessive;
    Possessives character2Possessive;


    /// <summary>
    /// Create and configure the characters with their baskets.
    /// </summary>
    protected void CreatePeopleAndBaskets() {
        character1Possessive = possessivesManager.PossessivesList[0];
        character2Possessive = possessivesManager.PossessivesList[1];
        sceneManager.ActivateObject(character1Possessive.Value + "Character", Positions.Character1Position, Positions.ObjectsRotation);
        sceneManager.ActivateObject(character2Possessive.Value + "Character", Positions.Character2Position, Positions.ObjectsRotation);
        CreateAndConfigureBaskets();
    }


    /// <summary>
    /// Create baskets and attach the correct fruit list to each basket
    /// </summary>
    protected void CreateAndConfigureBaskets() {
        GameObject basket1 = sceneManager.ActivateObject(character1Possessive.Value + "Basket", Positions.Character1Basket, Positions.ObjectsRotation);
        GameObject basket2 = sceneManager.ActivateObject(character2Possessive.Value + "Basket", Positions.Character2Basket, Positions.ObjectsRotation);


        basket1.GetComponent<BasketLogic>().SetFruitList(possessivesManager.PossessivesObjects[character1Possessive.Value]);
        basket2.GetComponent<BasketLogic>().SetFruitList(possessivesManager.PossessivesObjects[character2Possessive.Value]);
    }
 
    /// <summary>
    /// Executed every time the user puts a fruit into the correct basket
    /// 1) Check if all the fruits of a specific possessives have already been putted into the right basket
    /// 2) If there are no more fruits trigger the end of the Activity
    /// </summary>
    public virtual void PickedFruit() {
        DeleteEmptyPossessives();

        if (possessivesManager.PossessivesObjects.Count == 0) {
            End();
        }
    }

    /// <summary>
    /// Checks whether a list of fruits associated to a possessive is empty
    /// and in case delete the entry from the dictonary.
    /// </summary>
    private void DeleteEmptyPossessives() {
        List<string> possessivesToRemove = CheckForEmptyPossessives(possessivesManager.PossessivesObjects);

        foreach (string possessive in possessivesToRemove) {
            possessivesManager.PossessivesObjects.Remove(possessive);
        }
    }

    /// <summary>
    /// Checks whether a list of fruits associated to a possessive is empty and return its key
    /// </summary>
    /// <param name="possessivesObjects"></param>
    /// <returns>A list containing the keys of the possessives to be removed from the original dictionary</returns>
    private List<string> CheckForEmptyPossessives(Dictionary<string, List<string>> possessivesObjects) {
        List<string> possessivesToRemove = new List<string>();
        foreach (KeyValuePair<string, List<string>> tuple in possessivesObjects) {
            if (tuple.Value.Count == 0)
                possessivesToRemove.Add(tuple.Key);
        }
        return possessivesToRemove;
    }
}
