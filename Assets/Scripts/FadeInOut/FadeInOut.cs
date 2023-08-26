using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeInOut : MonoBehaviour
{
    // ת������������һ��ͼƬfadeIn��fadeOut)
    public static FadeInOut instance;  // ��ǰ���ʵ��

    public GameObject FadeInOutImage; // ת������ͼƬ
    public Animator myAnim;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject); // �л�����ʱ����ɾ����ǰ����
        }
        else if (instance != null)
        {
            Destroy(this.gameObject);// �����Լ�
        }
    }

    // �л��������� ,levelName:��Ҫת���ĳ�������
    public void SceneFadeInOut(string levelName)
    {
        StartCoroutine(setSceneFadeInOut(levelName));
    }

    IEnumerator setSceneFadeInOut(string levelName)
    {
        // ���ǰͼƬgameObject��һ����ͻ��Զ�ִ��Fadein�������ܹ�1.5s�룩
        FadeInOutImage.SetActive(true);
        yield return new WaitForSecondsRealtime(1.5f);  // �ȴ�1.5s��������ִ����
        SceneManager.LoadScene(levelName); // ������Ҫת���ĳ���
        myAnim.Play("FadeOut"); // �����˳��������ɺڵ�͸����1sʱ�䣩

        yield return new WaitForSecondsRealtime(1.0f); // �ȴ���������
        FadeInOutImage.SetActive(false);
        Time.timeScale = 1.0f; // ��ֹ�����ͣ��ť������Ϊ0��������л���������˱�������Ϊ1.0f
    }
}
