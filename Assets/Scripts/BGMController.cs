using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BGMController : MonoBehaviour
{
    public AudioClip[] myBGMClip; // 背景音乐
    public AudioClip[] myButtonClip; // 按钮背景音乐

    [HideInInspector] public AudioSource myAudioSource; // 音乐组件
    private void Awake()
    {
        myAudioSource = GetComponent<AudioSource>();

        string levelName = SceneManager.GetActiveScene().name; // 获取当前场景
        if (levelName == "MainMenu")
        {
            myAudioSource.clip = myBGMClip[0];
            // 开启循环播放背景音乐
            myAudioSource.loop = true;
            // 设置播放音量
            myAudioSource.volume = 0.7f;
            myAudioSource.Play();
        }
        else if (levelName == "LevelSelect")
        {
            myAudioSource.clip = myBGMClip[1];
            myAudioSource.loop = true;
            myAudioSource.volume = 0.7f;
            myAudioSource.Play();
        }
        else if (levelName == "Level1" || levelName == "Level2")
        {
            myAudioSource.clip = myBGMClip[2];
            myAudioSource.loop = true;
            myAudioSource.volume = 0.7f;
            myAudioSource.Play();
        }
        else if (levelName == "Level3")
        {
            myAudioSource.clip = myBGMClip[3];
            myAudioSource.loop = true;
            myAudioSource.volume = 0.7f;
            myAudioSource.Play();
        }
    }
}
