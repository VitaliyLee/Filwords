using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Achieve : MonoBehaviour
{
    public string AchieveKey;
    public GameObject mutePanel;

    public void OpenAchieve()
    {
        mutePanel.SetActive(false);
    }
}
