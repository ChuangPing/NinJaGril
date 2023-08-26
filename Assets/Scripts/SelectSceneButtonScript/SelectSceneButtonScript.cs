using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI; // UI
public class SelectSceneButtonScript : MonoBehaviour
{
    public Sprite buttonSprite; // 需要替换的button图片，有unity客户端进行赋值

    Image imageBtn1, imageBtn2, imageBtn3; // 给个关卡按钮的图片
    int clearLevel; // 关卡
    private void Awake()
    {
        imageBtn1 = GameObject.Find("Canvas/SafeAreaPanel/SelectpanelBGImage/Level1Button").GetComponent<Image>();
        imageBtn2 = GameObject.Find("Canvas/SafeAreaPanel/SelectpanelBGImage/Level2Button").GetComponent<Image>();
        imageBtn3 = GameObject.Find("Canvas/SafeAreaPanel/SelectpanelBGImage/Level3Button").GetComponent<Image>();
        clearLevel = PlayerPrefs.GetInt("clearLevel", 0); // 获取通过的管卡数，当取不到值时使用默认的0
        //Debug.Log(clearLevel);

        if (clearLevel == 0) // 还没有通过任何管卡，第一个管卡按钮图片替换为高亮表示可以玩
        {
            imageBtn1.sprite = buttonSprite;
        }
        if (clearLevel >= 1) // 通过第一关，可以玩第一关和第二关(两个按钮高亮）
        {
            imageBtn1.sprite = buttonSprite;
            imageBtn2.sprite = buttonSprite;
        }
        if (clearLevel >= 2)
        {
            imageBtn1.sprite = buttonSprite;
            imageBtn2.sprite = buttonSprite;
            imageBtn3.sprite = buttonSprite;
        }
    }

    public void GoToLevel1()
    {
        //SceneManager.LoadScene("Level1"); // 第一关进入不需要判断，都可以进（初始关卡）
        FadeInOut.instance.SceneFadeInOut("Level1");

        // 点击按钮播放声音
        BGMController myBGM = GameObject.Find("BGMController").GetComponent<BGMController>();
        myBGM.myAudioSource.PlayOneShot(myBGM.myButtonClip[0]);

    }

    public void GoToLevel2()
    {
        if (clearLevel >= 1) // 通过第一关可以进行第二关,只有这样例如：当通过了第三关，此时clearLevel=3，玩家点击第二关时也能加载第二关场景
        {
            //SceneManager.LoadScene("Level2")
            // 带转场动画的场景切换
            FadeInOut.instance.SceneFadeInOut("Level2");

            BGMController myBGM = GameObject.Find("BGMController").GetComponent<BGMController>();
            myBGM.myAudioSource.PlayOneShot(myBGM.myButtonClip[0]);
        }
        else
        {
            // 第二关未开放，点击了第二关按钮，播放不同的音效
            BGMController myBGM = GameObject.Find("BGMController").GetComponent<BGMController>();
            myBGM.myAudioSource.PlayOneShot(myBGM.myButtonClip[1]);
        }
    }

    // 加载第三关
    public void GoToLevel3()
    {
        if (clearLevel >= 2) // 通过前2关，可以进行第三关，当然也可以继续前面通过的关卡
        {
            //SceneManager.LoadScene("Level3");
            FadeInOut.instance.SceneFadeInOut("Level3");

            BGMController myBGM = GameObject.Find("BGMController").GetComponent<BGMController>();
            myBGM.myAudioSource.PlayOneShot(myBGM.myButtonClip[0]);
        }
        else
        {
            BGMController myBGM = GameObject.Find("BGMController").GetComponent<BGMController>();
            myBGM.myAudioSource.PlayOneShot(myBGM.myButtonClip[1]);
        }
    }

    // 加载MainMenu：游戏主界面
    public void GoToMainMenu()
    {
        //SceneManager.LoadScene("MainMenu");
        FadeInOut.instance.SceneFadeInOut("MainMenu");

        BGMController myBGM = GameObject.Find("BGMController").GetComponent<BGMController>();
        myBGM.myAudioSource.PlayOneShot(myBGM.myButtonClip[0]);

    }
}
