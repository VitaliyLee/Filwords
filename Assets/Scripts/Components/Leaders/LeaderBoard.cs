using GamePush;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

[System.Serializable]
public class LeaderboardFetchData
{
    public string avatar;
    public int id;
    public int score;
    public string name;
    public int position;

    public int Score;
}

public class LeaderBoard : MonoBehaviour
{
    [SerializeField] private List<LeaderItem> leadersList;
    //public TextMeshProUGUI ttt; //Это поле заменяет консоль вывода в сборке, тк GP не протестить кроме как на платформе
    [SerializeField] private Color currentPersonBgColor;

    [HideInInspector]
    public int playerPositionInRate = 0;

    private void OnEnable()
    {
        GP_Leaderboard.OnFetchSuccess += OnFetchSuccess;
    }
    //Отписка от события
    private void OnDisable()
    {
        GP_Leaderboard.OnFetchSuccess -= OnFetchSuccess;
    }

// Результат получения
    public void OnFetchSuccess(string fetchTag, GP_Data data)
    {
        var players = data.GetList<LeaderboardFetchData>();
        
        for (int i = 0; i < leadersList.Count; i++)
        {
            leadersList[i].PositionText.text = players[i].position.ToString();
            leadersList[i].ScoreText.text = players[i].Score.ToString();
            leadersList[i].ScoreText.color = Color.black;
            //ttt.text = $"players count: {players.Count}, Pos txt 1: {leadersList[leadersList.Count - 1].PositionText.text}, Pos txt 2: {players[i].position} --- {data.Data}";

            if (players[i].avatar != null && players[i].avatar != "")
                StartCoroutine(SetAvatar(players[i].avatar, leadersList[i].PersonAvatar));

            //В этот момент уже известно, что текущий игрок точно не отрисуется, если ему не помочь
            if(players[i].id == GP_Player.GetID())
            {
                if(players[i].position > 3)
                    leadersList[i].PersonAvatarBg.color = currentPersonBgColor;
                else
                    leadersList[i].PersonAvatarBg.color = leadersList[i].BgColorDefoult;

                leadersList[i].CurrentPlayerBorder.gameObject.SetActive(true);
                leadersList[i].ScoreText.color = Color.white;
                playerPositionInRate = players[i].position;
            }

            else
            {
                leadersList[i].PersonAvatarBg.color = leadersList[i].BgColorDefoult;
                leadersList[i].CurrentPlayerBorder.gameObject.SetActive(false);
            }
        }

        //Помогаю текущему юзеру отрисоваться если он конце списка "players"
        if (players[players.Count - 1].id == GP_Player.GetID())
        {
            int index = leadersList.Count - 1;

            leadersList[index].PositionText.text = players[players.Count - 1].position.ToString();
            leadersList[index].ScoreText.text = players[players.Count - 1].Score.ToString();

            if (players[players.Count - 1].avatar != null && players[players.Count - 1].avatar != "")
                StartCoroutine(SetAvatar(players[players.Count - 1].avatar, leadersList[index].PersonAvatar));

            leadersList[index].PersonAvatarBg.color = currentPersonBgColor;
            leadersList[index].CurrentPlayerBorder.gameObject.SetActive(true);
            leadersList[index].ScoreText.color = Color.white;
            playerPositionInRate = players[players.Count - 1].position;
            return;
        }
    }

    public void Fetch()
    {
        GP_Leaderboard.Fetch("PlayerScore", "Score", Order.DESC, 6, 0, WithMe.last, "");
    }

    private IEnumerator SetAvatar(string url, Image sourceAvatar)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);

        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            Debug.LogWarning(request.error);
            yield break;
        }

        var texture = DownloadHandlerTexture.GetContent(request);
        var sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), Vector2.one * 0.5f);

        sourceAvatar.sprite = sprite;
    }
}
