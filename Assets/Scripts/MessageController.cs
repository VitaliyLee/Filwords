using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageController : MonoBehaviour
{
    public void MessageAnimationEnd()
    {
        gameObject.SetActive(false);
    }
}
