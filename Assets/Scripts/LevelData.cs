using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelData : MonoBehaviour
{
    [SerializeField] private string dictionaryName;
    [SerializeField] private List<CardData> cardsList;

    private int level;

    public List<CardData> CardsList { get => cardsList; }
    public string DictionaryName { get => dictionaryName; }
    public int Level { get => level; }

    public void SetLevel()
    {
        level = 0;
    }
}
