using GamePush;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OurGamesControl : MonoBehaviour
{
    public void OpenOtherGame()
    {
        GP_GamesCollections.Open("ALL");
    }
}
