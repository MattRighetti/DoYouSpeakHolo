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
        audioContext.Possessive = Possessives.His;
        Character character = Character.Male;
        yield return ShowCharacterAndBasket(character.Value, character.Value + "Basket");

        //Introduce the second character without the scene context
        audioContext.Possessive = Possessives.Her;
        character = Character.Female;
        yield return ShowCharacterAndBasket(character.Value, character.Value + "Basket");
    }
}