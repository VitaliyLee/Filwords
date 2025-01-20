using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AchievementSystem : MonoBehaviour
{
    [SerializeField] private List<Achieve> achieveList;

    public void UpdateAchievement()
    {
        for (int i = 0; i < achieveList.Count; i++)
        {
            if(Saver.AchieveKeysString.Contains(achieveList[i].AchieveKey))
            {
                achieveList[i].OpenAchieve();
            }
        }
    }

    public void CheckAchievement(int FieldLength, int CurrentLevel, float GameTime)
    {
        //Достижения 3х3
        if(FieldLength == 9 && CurrentLevel == 1)
            Saver.SaveAchieve("2");
        if(FieldLength == 9 && GameTime <= 30)
            Saver.SaveAchieve("6");

        //Достижения 4х4
        if (FieldLength == 16 && CurrentLevel == 1)
            Saver.SaveAchieve("3");
        if (FieldLength == 16 && GameTime <= 60)
            Saver.SaveAchieve("7");

        //Достижения 5х5
        if (FieldLength == 25 && CurrentLevel == 1)
            Saver.SaveAchieve("4");
        if (FieldLength == 25 && GameTime <= 90)
            Saver.SaveAchieve("8");

        //Достижения 6х6
        if (FieldLength == 36 && CurrentLevel == 1)
            Saver.SaveAchieve("5");
        if (FieldLength == 36 && GameTime <= 120)
            Saver.SaveAchieve("9");
    }

    private void ThreeOnThreeAchieve()
    {

    }

    private void FoureOnFoureAchieve()
    {

    }

    private void FiveOnFiveAchieve()
    {

    }

    private void SixOnSixAchieve()
    {

    }
}
