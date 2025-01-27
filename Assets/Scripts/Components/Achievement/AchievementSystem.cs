using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class AchievementSystem : MonoBehaviour
{
    [SerializeField] private List<Achieve> achieveList;
    [SerializeField] private AchieveMessageData achieveMessage;
    [SerializeField] private LeaderBoard leaderBoard;

    private void OnEnable() => Saver.AchieveSaveEvent += ShowAchieveMessage;
    private void OnDisable() => Saver.AchieveSaveEvent -= ShowAchieveMessage;

    public void UpdateAchievement()
    {
        for (int i = 0; i < achieveList.Count; i++)
        {
            if(Saver.AchieveKeysString.Contains(achieveList[i].AchieveKey))
            {
                achieveList[i].UnmuteAchieve();
            }
        }
    }

    public void CheckAchievement(int FieldLength, int dictionaryLength, int CurrentLevel, float GameTime)
    {
        switch(FieldLength)
        {
            case 9:
                ThreeOnThreeAchieve(dictionaryLength, CurrentLevel, GameTime);
                break;
            case 16:
                FoureOnFoureAchieve(dictionaryLength, CurrentLevel, GameTime);
                break;
            case 25:
                FiveOnFiveAchieve(dictionaryLength, CurrentLevel, GameTime);
                break;
            case 36:
                SixOnSixAchieve(dictionaryLength, CurrentLevel, GameTime);
                break;
        }

        if (Saver.AchieveKeysString.Contains("10") && Saver.AchieveKeysString.Contains("11") && Saver.AchieveKeysString.Contains("12") && Saver.AchieveKeysString.Contains("13"))
            Saver.SaveAchieve("14");

        if (leaderBoard.playerPositionInRate == 3)
            Saver.SaveAchieve("15");
        if (leaderBoard.playerPositionInRate == 2)
        {
            Saver.SaveAchieve("15");
            Saver.SaveAchieve("16");
        }
        if (leaderBoard.playerPositionInRate == 1)
        {
            Saver.SaveAchieve("15");
            Saver.SaveAchieve("16");
            Saver.SaveAchieve("17");
        }


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

    //���������� ��� �� ��������� � ��������� ������ ����� ��������
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
            achieveMessage.transform.localScale = new Vector3((float)Math.Round(messageScale.x, 1) - scaleRate, (float)Math.Round(messageScale.y, 1) - scaleRate, 0);//���������� ����� �� ������ �� �� ���������� �� ��������� 0.1, � 0.09 (
            messageScale = achieveMessage.transform.localScale;

            yield return new WaitForSeconds(timeRate);
        }
    }

    private void ThreeOnThreeAchieve(int dictionaryLength, int CurrentLevel, float GameTime)
    {
        //���������� 3�3
        if (CurrentLevel == 1)
            Saver.SaveAchieve("2");
        if (GameTime <= 30)
            Saver.SaveAchieve("6");
        if(CurrentLevel * 3 == dictionaryLength)
            Saver.SaveAchieve("10");
    }

    private void FoureOnFoureAchieve(int dictionaryLength, int CurrentLevel, float GameTime)
    {
        //���������� 4�4
        if (CurrentLevel == 1)
            Saver.SaveAchieve("3");
        if (GameTime <= 60)
            Saver.SaveAchieve("7");
        if (CurrentLevel * 4 == dictionaryLength)
            Saver.SaveAchieve("11");
    }

    private void FiveOnFiveAchieve(int dictionaryLength, int CurrentLevel, float GameTime)
    {
        //���������� 5�5    
        if (CurrentLevel == 1)
            Saver.SaveAchieve("4");
        if (GameTime <= 90)
            Saver.SaveAchieve("8");
        if (CurrentLevel * 5 == dictionaryLength)
            Saver.SaveAchieve("12");
    }

    private void SixOnSixAchieve(int dictionaryLength, int CurrentLevel, float GameTime)
    {
        //���������� 6�6
        if (CurrentLevel == 1)
            Saver.SaveAchieve("5");
        if (GameTime <= 120)
            Saver.SaveAchieve("9");
        if (CurrentLevel * 6 == dictionaryLength)
            Saver.SaveAchieve("13");
    }
}
