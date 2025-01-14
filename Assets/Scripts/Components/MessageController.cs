using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MessageController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI messageTextField;
    [SerializeField] string messageText;

    public void MessageInit()
    {
        messageTextField.text = messageText;
    }

    public void MessageAnimationEnd()
    {
        gameObject.SetActive(false);
    }
}
