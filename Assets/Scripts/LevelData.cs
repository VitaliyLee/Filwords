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
    public int Level { get { GetLevel(); return level; } }

    private void GetLevel()
    {
        //В качестве ключа передается длина списка т.к. размер игрового поля всегда фиксированый и он ТОЧНО не будет меняться
        switch(cardsList.Count)
        {
            case 9:
                level = Saver.LevelNoob;
                break;
            case 16:
                level = Saver.LevelNormal;
                break;
            case 25:
                level = Saver.LevelVeteran;
                break;
            case 36:
                level = Saver.LevelProfessionsl;
                break;
        }
    }
}
