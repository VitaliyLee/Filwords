using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private string toMenuAnimName = "";

    public void PlayAnimation()
    {
        animator.Play(toMenuAnimName);
    }

    public void InitAnimation(string ToMenuAnimName)
    {
        toMenuAnimName = ToMenuAnimName;
    }
}
