using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AchieveMessageData : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI bodyText;
    [SerializeField] private Image messageImage;

    public void SetMessageData(string TitleText, string BodyText, Sprite MessageImage)
    {
        titleText.text = TitleText;
        bodyText.text = BodyText;
        messageImage.sprite = MessageImage;
    }
}
