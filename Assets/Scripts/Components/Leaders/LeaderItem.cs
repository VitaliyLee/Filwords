using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LeaderItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI positionText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private Image personAvatar;
    [SerializeField] private Image personAvatarBg;
    [SerializeField] private Image currentPlayerBorder;
    [SerializeField] private Color bgColorDefoult;

    public TextMeshProUGUI PositionText { get => positionText; set => positionText = value; }
    public TextMeshProUGUI ScoreText { get => scoreText; set => scoreText = value; }
    public Image PersonAvatar { get => personAvatar; set => personAvatar = value; }
    public Image PersonAvatarBg { get => personAvatarBg; set => personAvatar = value; }
    public Image CurrentPlayerBorder { get => currentPlayerBorder; }
    public Color BgColorDefoult { get => bgColorDefoult; }
}
