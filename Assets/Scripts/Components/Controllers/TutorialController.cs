using GamePush;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
    [SerializeField] private GameController gameController;

    [Space(10)]
    [SerializeField] private GameObject wordOne;
    [SerializeField] private GameObject wordTwo;
    [SerializeField] private GameObject wordThree;
    [SerializeField] private GameObject finalWorFinding;

    private int wordIndex = 0;

    private void OnEnable() => Saver.LoadEvent += StartTutorial;
    private void OnDisable() => Saver.LoadEvent -= StartTutorial;

    public void StartTutorial()
    {
        if (Saver.Score > 0)
        {
            gameObject.SetActive(false);
            return;
        }
        gameController.FindingWordEvent += NextWord;
    }

    private void NextWord()
    {
        wordIndex++;
        if (wordIndex == 1)
        {
            wordTwo.SetActive(true);
            wordThree.SetActive(false);
        }
        else if (wordIndex == 2)
        {
            wordTwo.SetActive(true);
            wordThree.SetActive(true);
        }
        else
        {
            wordOne.SetActive(false);
            wordTwo.SetActive(false);
            wordThree.SetActive(false);

            gameController.FindingWordEvent -= NextWord;
            finalWorFinding.SetActive(true);
        }
    }
}
