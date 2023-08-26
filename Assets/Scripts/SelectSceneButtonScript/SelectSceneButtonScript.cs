using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI; // UI
public class SelectSceneButtonScript : MonoBehaviour
{
    public Sprite buttonSprite; // ��Ҫ�滻��buttonͼƬ����unity�ͻ��˽��и�ֵ

    Image imageBtn1, imageBtn2, imageBtn3; // �����ؿ���ť��ͼƬ
    int clearLevel; // �ؿ�
    private void Awake()
    {
        imageBtn1 = GameObject.Find("Canvas/SafeAreaPanel/SelectpanelBGImage/Level1Button").GetComponent<Image>();
        imageBtn2 = GameObject.Find("Canvas/SafeAreaPanel/SelectpanelBGImage/Level2Button").GetComponent<Image>();
        imageBtn3 = GameObject.Find("Canvas/SafeAreaPanel/SelectpanelBGImage/Level3Button").GetComponent<Image>();
        clearLevel = PlayerPrefs.GetInt("clearLevel", 0); // ��ȡͨ���Ĺܿ�������ȡ����ֵʱʹ��Ĭ�ϵ�0
        //Debug.Log(clearLevel);

        if (clearLevel == 0) // ��û��ͨ���κιܿ�����һ���ܿ���ťͼƬ�滻Ϊ������ʾ������
        {
            imageBtn1.sprite = buttonSprite;
        }
        if (clearLevel >= 1) // ͨ����һ�أ��������һ�غ͵ڶ���(������ť������
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
        //SceneManager.LoadScene("Level1"); // ��һ�ؽ��벻��Ҫ�жϣ������Խ�����ʼ�ؿ���
        FadeInOut.instance.SceneFadeInOut("Level1");

        // �����ť��������
        BGMController myBGM = GameObject.Find("BGMController").GetComponent<BGMController>();
        myBGM.myAudioSource.PlayOneShot(myBGM.myButtonClip[0]);

    }

    public void GoToLevel2()
    {
        if (clearLevel >= 1) // ͨ����һ�ؿ��Խ��еڶ���,ֻ���������磺��ͨ���˵����أ���ʱclearLevel=3����ҵ���ڶ���ʱҲ�ܼ��صڶ��س���
        {
            //SceneManager.LoadScene("Level2")
            // ��ת�������ĳ����л�
            FadeInOut.instance.SceneFadeInOut("Level2");

            BGMController myBGM = GameObject.Find("BGMController").GetComponent<BGMController>();
            myBGM.myAudioSource.PlayOneShot(myBGM.myButtonClip[0]);
        }
        else
        {
            // �ڶ���δ���ţ�����˵ڶ��ذ�ť�����Ų�ͬ����Ч
            BGMController myBGM = GameObject.Find("BGMController").GetComponent<BGMController>();
            myBGM.myAudioSource.PlayOneShot(myBGM.myButtonClip[1]);
        }
    }

    // ���ص�����
    public void GoToLevel3()
    {
        if (clearLevel >= 2) // ͨ��ǰ2�أ����Խ��е����أ���ȻҲ���Լ���ǰ��ͨ���Ĺؿ�
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

    // ����MainMenu����Ϸ������
    public void GoToMainMenu()
    {
        //SceneManager.LoadScene("MainMenu");
        FadeInOut.instance.SceneFadeInOut("MainMenu");

        BGMController myBGM = GameObject.Find("BGMController").GetComponent<BGMController>();
        myBGM.myAudioSource.PlayOneShot(myBGM.myButtonClip[0]);

    }
}
