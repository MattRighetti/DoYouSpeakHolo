using System.Collections;

//  Class responsible of the Basic Learning Phase version of Activity 3
public class LearningPhaseActivity3Basic : LearningPhaseActivity3 {

    //  Brief introduction of the scene made by the VA
    protected override IEnumerator SceneIntroduction() {

        //  Introduce the different character and their baskets
        yield return IntroducePeopleAndBaskets();

        //  Show objects one at time introducing them without the scene context
        yield return ShowObjects(SceneObjects);

        //  Trigger the Checking Phase
        End();
    }

    private IEnumerator IntroducePeopleAndBaskets() {

        //Introduce the first character without the scene context
        Possessives character1Possessive = possessivesManager.PossessivesList[0];
        audioContext.Possessive = character1Possessive;
        
        yield return ShowCharacterAndBasket(character1Possessive.Value + "Character", character1Possessive.Value + "Basket");

        //Introduce the second character without the scene context
        Possessives character2Possessive = possessivesManager.PossessivesList[1];
        audioContext.Possessive = character2Possessive;
        yield return ShowCharacterAndBasket(character2Possessive.Value + "Character", character2Possessive.Value + "Basket");
    }
}