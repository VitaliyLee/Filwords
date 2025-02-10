using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OurGamesControl : MonoBehaviour
{
    public void OpenOtherGame(string URL)
    {
        Application.OpenURL(URL);
    }
}
