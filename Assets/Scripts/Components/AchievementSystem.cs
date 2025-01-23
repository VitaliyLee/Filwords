using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class AchievementSystem : MonoBehaviour
{
    [SerializeField] private List<Achieve> achieveList;
    [SerializeField] private AchieveMessageData achieveMessage;

    private void OnEnable() => Saver.AchieveSaveEvent += ShowAchieveMessage;
    private void OnDisable() => Saver.AchieveSaveEvent -= ShowAchieveMessage;

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

    public void ShowAchieveMessage(string AchieveKey)
    {
        for (int i = 0; i < achieveList.Count; i++)
        {
            if (achieveList[i].AchieveKey == AchieveKey)
            {
                achieveMessage.SetMessageData(
                    achieveList[i].TitleText.text, 
                    achieveList[i].BodyText.text, 
                    achieveList[i].MessageImage.sprite);
            }
        }

        StartCoroutine(PopUpAchieveMessage());
    }

    private IEnumerator PopUpAchieveMessage()
    {
        Vector3 messageScale = achieveMessage.transform.localScale;
        float scaleRate = 0.1f;
        float timeRate = 0.03f;

        while (messageScale.x < 1 && messageScale.y < 1)
        {
            achieveMessage.transform.localScale = new Vector3(messageScale.x + scaleRate, messageScale.y + scaleRate, 0);
            messageScale = achieveMessage.transform.localScale;

            yield return new WaitForSeconds(timeRate);
        }

        yield return new WaitForSeconds(2);

        while (messageScale.x > 0 && messageScale.y > 0)
        {
            achieveMessage.transform.localScale = new Vector3((float)Math.Round(messageScale.x, 1) - scaleRate, (float)Math.Round(messageScale.y, 1) - scaleRate, 0);//Округление нужно тк почему то он прибавляет не идеальные 0.1, а 0.09 (
            messageScale = achieveMessage.transform.localScale;

            yield return new WaitForSeconds(timeRate);
        }
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
