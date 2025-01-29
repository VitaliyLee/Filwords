using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SoundController : MonoBehaviour
{
    //Картинки кнопки включения/выключения звука
    [SerializeField] private Image volumeButtonImage;
    [SerializeField] private TextMeshProUGUI volumeButtonText;
    [SerializeField] private Sprite volumeImageOn;
    [SerializeField] private Sprite volumeImageOff;

    [Space(10)]
    [SerializeField] private AudioSource messageAudioSource;
    [SerializeField] private AudioSource mainAudioSource;
    [SerializeField] private AudioSource secondAudioSource;

    [Space(10)]
    [SerializeField] private float pitchRate;
    [SerializeField] private AudioClip buttonClip, HintClip, trueAnswerClip, SelectClip, popUpAchievClip, winClip;
    private float pitchRateDefoult = 1f;

    public void PlayButtonSound()
    {
        mainAudioSource.pitch = pitchRateDefoult;
        mainAudioSource.clip = buttonClip;
        mainAudioSource.Play();
    }

    public void PlayHintSound()
    {
        mainAudioSource.pitch = pitchRateDefoult;
        mainAudioSource.clip = HintClip;
        mainAudioSource.Play();
    }

    public void PlayTrueAnswernSound()
    {
        mainAudioSource.pitch = pitchRateDefoult;
        mainAudioSource.clip = trueAnswerClip;
        mainAudioSource.Play();
    }

    public void PlaySelectSoundWithRaisingTone(int Ratio)
    {
        mainAudioSource.pitch = pitchRateDefoult + Ratio * pitchRate;
        mainAudioSource.clip = SelectClip;
        mainAudioSource.Play();
    }

    public void PlayPopUpAchievSound()
    {
        secondAudioSource.clip = popUpAchievClip;
        secondAudioSource.Play();
    }

    public void PlayWinSound()
    {
        mainAudioSource.pitch = pitchRateDefoult;
        mainAudioSource.clip = winClip;
        mainAudioSource.Play();
    }

    public void VolumeOnOff()
    {
        mainAudioSource.mute = !mainAudioSource.mute;
        secondAudioSource.mute = !secondAudioSource.mute;
        messageAudioSource.mute = secondAudioSource.mute;

        if (mainAudioSource.mute)
        {
            volumeButtonImage.sprite = volumeImageOff;
            volumeButtonText.text = "Звук выключен";
        }
        else
        {
            volumeButtonImage.sprite = volumeImageOn;
            volumeButtonText.text = "Звук включен";
        }
    }
}
