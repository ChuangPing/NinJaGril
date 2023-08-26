using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PanelButtonScript : MonoBehaviour
{
    public GameObject selectPanel; // unity编辑器赋值，panel（UI）
    public GameObject stopButton; // 暂停按钮
    public GameObject levelSelectButton; // 选择关卡按钮（功能展示UI内）
    public GameObject mainMenuButton; // 回到主场京
    public GameObject replayButton; // 重玩按钮



    // 点击暂停游戏按钮显示UI
    public void setSelectPanelOn()
    {
        selectPanel.SetActive(true); // 展示功能UI，可以选择重玩当前关卡、回到mainMenu、回到选择关卡场景
        Time.timeScale = 0f; // 暂停游戏
    }

    // 点击取消暂停游戏按钮隐藏UI，开始游戏
    public void setSelectPanelOff()
    {
        selectPanel.SetActive(false);
        Time.timeScale = 1.0f; // 暂停游戏
    }

    // 显示暂停按钮
    public void setStopButtonOn()
    {
        stopButton.SetActive(true);

        BGMController myBGM = GameObject.Find("BGMController").GetComponent<BGMController>();
        myBGM.myAudioSource.PlayOneShot(myBGM.myButtonClip[0]);
    }

    // 隐藏暂停按钮
    public void setStopBUttonOff()
    {
        stopButton.SetActive(false);
        BGMController myBGM = GameObject.Find("BGMController").GetComponent<BGMController>();
        myBGM.myAudioSource.PlayOneShot(myBGM.myButtonClip[0]);
    }

    // 点击重玩按钮，重玩当前关卡
    public void ReplayButton()
    {
        // 获取当前场景的名称
        string scenName = SceneManager.GetActiveScene().name;
        // 重新加载当前场景
        //SceneManager.LoadScene(scenName);
        //Time.timeScale = 1.0f; // 重新设置时间，因为点击暂停按钮将Time.timeScale设置为0，而这三个按钮在暂停按钮点击后才显示，因此要重置否则画面是暂停状态

        FadeInOut.instance.SceneFadeInOut(scenName);

        BGMController myBGM = GameObject.Find("BGMController").GetComponent<BGMController>();
        myBGM.myAudioSource.PlayOneShot(myBGM.myButtonClip[0]);

    }

    // 回到MainMenu场景
    public void MainMenu()
    {
        //SceneManager.LoadScene("MainMenu");
        //Time.timeScale = 1.0f;

        // 具有转场动画的场景切换
        FadeInOut.instance.SceneFadeInOut("MainMenu");

        BGMController myBGM = GameObject.Find("BGMController").GetComponent<BGMController>();
        myBGM.myAudioSource.PlayOneShot(myBGM.myButtonClip[0]);
    }

    // 回到关卡场景（游戏暂停后返回到游戏按钮事件）
    public void LevelSelectButton()
    {
        //SceneManager.LoadScene("LevelSelect");
        //Time.timeScale = 1.0f;
        FadeInOut.instance.SceneFadeInOut("LevelSelect");

        BGMController myBGM = GameObject.Find("BGMController").GetComponent<BGMController>();
        myBGM.myAudioSource.PlayOneShot(myBGM.myButtonClip[0]);
    }

    // 点击play按钮，播放player向右跑的动画
    public void MainMenuPlayButton()
    {
        GameObject mainMenuPlayer = GameObject.Find("MainMenuPlayer"); // 获取pay界面player人物
        Animator myAnim = mainMenuPlayer.GetComponent<Animator>(); // 获取pay的动画控制器
        myAnim.SetBool("Run", true); // 播放动画

        // 获取player按钮
        GameObject playerButton = GameObject.Find("Canvas/SafeAreaPanel/PlayerButton");
        playerButton.SetActive(false); // 点击player开始游戏，按钮消失

        // 播放点击按钮声音
        BGMController myBGM = GameObject.Find("BGMController").GetComponent<BGMController>();
        myBGM.myAudioSource.PlayOneShot(myBGM.myButtonClip[0]);

        // 进入关卡选择场景
        //SceneManager.LoadScene("LevelSelect");
        FadeInOut.instance.SceneFadeInOut("LevelSelect");
    }

    //  点击删除游戏数据按钮，显示提示是否删除UI
    public void DataDeleteButton()
    {
        RectTransform DataDeleImage = GameObject.Find("Canvas/SafeAreaPanel/DataDeleteImage").GetComponent<RectTransform>();
        DataDeleImage.anchoredPosition = new Vector2(0f, -100f); // 初始询问是否删除UI在场景之外

        BGMController myBGM = GameObject.Find("BGMController").GetComponent<BGMController>();
        myBGM.myAudioSource.PlayOneShot(myBGM.myButtonClip[0]);
    }

    // 点击确定删除按钮，删除数据，隐藏询问删除UI（移到场景之外）
    public void YesButton()
    {
        PlayerPrefs.DeleteAll();
        RectTransform DataDeleImage = GameObject.Find("Canvas/SafeAreaPanel/DataDeleteImage").GetComponent<RectTransform>();
        DataDeleImage.anchoredPosition = new Vector2(0f, 1500f); // 询问删除UI移动场景之外

        // 删除PlayerPrefs所有数据后，重置游戏初始数据
        IsFirstTimePlayCheck checkScript = GameObject.Find("IsFirstTimePlayCheck").GetComponent<IsFirstTimePlayCheck>();
        checkScript.FirstTimePlayState();

        BGMController myBGM = GameObject.Find("BGMController").GetComponent<BGMController>();
        myBGM.myAudioSource.PlayOneShot(myBGM.myButtonClip[0]);
    }

    // 点击取消删除按钮，隐藏询问删除UI
    public void NoButton()
    {
        RectTransform DataDeleImage = GameObject.Find("Canvas/SafeAreaPanel/DataDeleteImage").GetComponent<RectTransform>();
        DataDeleImage.anchoredPosition = new Vector2(0f, 1500f); // 初始询问是否删除UI在场景之外

        BGMController myBGM = GameObject.Find("BGMController").GetComponent<BGMController>();
        myBGM.myAudioSource.PlayOneShot(myBGM.myButtonClip[0]);
    }


}
