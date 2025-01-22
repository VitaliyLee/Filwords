using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Achieve : MonoBehaviour
{
    public string AchieveKey;
    public GameObject mutePanel;

    [Space(10)]
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI bodyText;
    [SerializeField] private Image messageImage;

    public TextMeshProUGUI TitleText => titleText;
    public TextMeshProUGUI BodyText => bodyText;
    public Image MessageImage => messageImage;

    public void OpenAchieve()
    {
        mutePanel.SetActive(false);
    }
}
