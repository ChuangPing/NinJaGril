using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PanelButtonScript : MonoBehaviour
{
    public GameObject selectPanel; // unity�༭����ֵ��panel��UI��
    public GameObject stopButton; // ��ͣ��ť
    public GameObject levelSelectButton; // ѡ��ؿ���ť������չʾUI�ڣ�
    public GameObject mainMenuButton; // �ص�������
    public GameObject replayButton; // ���水ť



    // �����ͣ��Ϸ��ť��ʾUI
    public void setSelectPanelOn()
    {
        selectPanel.SetActive(true); // չʾ����UI������ѡ�����浱ǰ�ؿ����ص�mainMenu���ص�ѡ��ؿ�����
        Time.timeScale = 0f; // ��ͣ��Ϸ
    }

    // ���ȡ����ͣ��Ϸ��ť����UI����ʼ��Ϸ
    public void setSelectPanelOff()
    {
        selectPanel.SetActive(false);
        Time.timeScale = 1.0f; // ��ͣ��Ϸ
    }

    // ��ʾ��ͣ��ť
    public void setStopButtonOn()
    {
        stopButton.SetActive(true);

        BGMController myBGM = GameObject.Find("BGMController").GetComponent<BGMController>();
        myBGM.myAudioSource.PlayOneShot(myBGM.myButtonClip[0]);
    }

    // ������ͣ��ť
    public void setStopBUttonOff()
    {
        stopButton.SetActive(false);
        BGMController myBGM = GameObject.Find("BGMController").GetComponent<BGMController>();
        myBGM.myAudioSource.PlayOneShot(myBGM.myButtonClip[0]);
    }

    // ������水ť�����浱ǰ�ؿ�
    public void ReplayButton()
    {
        // ��ȡ��ǰ����������
        string scenName = SceneManager.GetActiveScene().name;
        // ���¼��ص�ǰ����
        //SceneManager.LoadScene(scenName);
        //Time.timeScale = 1.0f; // ��������ʱ�䣬��Ϊ�����ͣ��ť��Time.timeScale����Ϊ0������������ť����ͣ��ť��������ʾ�����Ҫ���÷���������ͣ״̬

        FadeInOut.instance.SceneFadeInOut(scenName);

        BGMController myBGM = GameObject.Find("BGMController").GetComponent<BGMController>();
        myBGM.myAudioSource.PlayOneShot(myBGM.myButtonClip[0]);

    }

    // �ص�MainMenu����
    public void MainMenu()
    {
        //SceneManager.LoadScene("MainMenu");
        //Time.timeScale = 1.0f;

        // ����ת�������ĳ����л�
        FadeInOut.instance.SceneFadeInOut("MainMenu");

        BGMController myBGM = GameObject.Find("BGMController").GetComponent<BGMController>();
        myBGM.myAudioSource.PlayOneShot(myBGM.myButtonClip[0]);
    }

    // �ص��ؿ���������Ϸ��ͣ�󷵻ص���Ϸ��ť�¼���
    public void LevelSelectButton()
    {
        //SceneManager.LoadScene("LevelSelect");
        //Time.timeScale = 1.0f;
        FadeInOut.instance.SceneFadeInOut("LevelSelect");

        BGMController myBGM = GameObject.Find("BGMController").GetComponent<BGMController>();
        myBGM.myAudioSource.PlayOneShot(myBGM.myButtonClip[0]);
    }

    // ���play��ť������player�����ܵĶ���
    public void MainMenuPlayButton()
    {
        GameObject mainMenuPlayer = GameObject.Find("MainMenuPlayer"); // ��ȡpay����player����
        Animator myAnim = mainMenuPlayer.GetComponent<Animator>(); // ��ȡpay�Ķ���������
        myAnim.SetBool("Run", true); // ���Ŷ���

        // ��ȡplayer��ť
        GameObject playerButton = GameObject.Find("Canvas/SafeAreaPanel/PlayerButton");
        playerButton.SetActive(false); // ���player��ʼ��Ϸ����ť��ʧ

        // ���ŵ����ť����
        BGMController myBGM = GameObject.Find("BGMController").GetComponent<BGMController>();
        myBGM.myAudioSource.PlayOneShot(myBGM.myButtonClip[0]);

        // ����ؿ�ѡ�񳡾�
        //SceneManager.LoadScene("LevelSelect");
        FadeInOut.instance.SceneFadeInOut("LevelSelect");
    }

    //  ���ɾ����Ϸ���ݰ�ť����ʾ��ʾ�Ƿ�ɾ��UI
    public void DataDeleteButton()
    {
        RectTransform DataDeleImage = GameObject.Find("Canvas/SafeAreaPanel/DataDeleteImage").GetComponent<RectTransform>();
        DataDeleImage.anchoredPosition = new Vector2(0f, -100f); // ��ʼѯ���Ƿ�ɾ��UI�ڳ���֮��

        BGMController myBGM = GameObject.Find("BGMController").GetComponent<BGMController>();
        myBGM.myAudioSource.PlayOneShot(myBGM.myButtonClip[0]);
    }

    // ���ȷ��ɾ����ť��ɾ�����ݣ�����ѯ��ɾ��UI���Ƶ�����֮�⣩
    public void YesButton()
    {
        PlayerPrefs.DeleteAll();
        RectTransform DataDeleImage = GameObject.Find("Canvas/SafeAreaPanel/DataDeleteImage").GetComponent<RectTransform>();
        DataDeleImage.anchoredPosition = new Vector2(0f, 1500f); // ѯ��ɾ��UI�ƶ�����֮��

        // ɾ��PlayerPrefs�������ݺ�������Ϸ��ʼ����
        IsFirstTimePlayCheck checkScript = GameObject.Find("IsFirstTimePlayCheck").GetComponent<IsFirstTimePlayCheck>();
        checkScript.FirstTimePlayState();

        BGMController myBGM = GameObject.Find("BGMController").GetComponent<BGMController>();
        myBGM.myAudioSource.PlayOneShot(myBGM.myButtonClip[0]);
    }

    // ���ȡ��ɾ����ť������ѯ��ɾ��UI
    public void NoButton()
    {
        RectTransform DataDeleImage = GameObject.Find("Canvas/SafeAreaPanel/DataDeleteImage").GetComponent<RectTransform>();
        DataDeleImage.anchoredPosition = new Vector2(0f, 1500f); // ��ʼѯ���Ƿ�ɾ��UI�ڳ���֮��

        BGMController myBGM = GameObject.Find("BGMController").GetComponent<BGMController>();
        myBGM.myAudioSource.PlayOneShot(myBGM.myButtonClip[0]);
    }


}
