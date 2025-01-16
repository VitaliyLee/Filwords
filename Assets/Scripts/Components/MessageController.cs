using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MessageController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI messageTextField;
    [SerializeField] private string messageText;

    public string MessageText { set => messageText = value; }

    public void MessageInit()
    {
        messageTextField.text = messageText;
    }

    public void MessageAnimationEnd()
    {
        gameObject.SetActive(false);
    }
}
