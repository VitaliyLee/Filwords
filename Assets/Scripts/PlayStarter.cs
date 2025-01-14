using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayStarter : MonoBehaviour
{
    [SerializeField] FieldController fieldController;

    [SerializeField] GridController fiealdFourByFour;
    [SerializeField] GridController fiealdFiveByFive;

    [SerializeField] string fourLetterDictionaryName;
    [SerializeField] string fiveLetterDictionaryName;

    public void StartGame(int DifficultyLevel)
    {
        if (DifficultyLevel == 1)
        {
            fieldController.CardsList = fiealdFourByFour.CardsList;
            fieldController.Init(fourLetterDictionaryName);
        }
        if (DifficultyLevel == 2)
        {
            fieldController.CardsList = fiealdFiveByFive.CardsList;
            fieldController.Init(fiveLetterDictionaryName);
        }

    }
}
