using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WineTrigger : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Player")
        {
            string levelName = SceneManager.GetActiveScene().name; // ��ȡ��ǰ��ǰ�����֣�level+number
            string temp = levelName.Substring(5); // ��ȡ��ǰ���������ֱ��
            int levelNumber = int.Parse(temp); // ת��Ϊ����
            // ����ͨ���Ĺؿ�����
            int clearedLevel = PlayerPrefs.GetInt("clearLevel");
            if (levelNumber > clearedLevel)
            {
                // ��ǰͨ���Ĺؿ�����ǰͨ���Ĺؿ���ʱ���Ż�����洢�µ�clearLevel,���򲻽��и�ֵ�����������ǰͨ���Ĺؿ���
                PlayerPrefs.SetInt("clearLevel", levelNumber); // �����ű�����ȡ
            }
            //SceneManager.LoadScene("LevelSelect"); // ��player������ͨ�ص����壬��ʼ���뵽�ؿ�ѡ�񳡾�
            Time.timeScale = 0; // ͨ��ʱ����ͣ��Ϸ��Ч��
            FadeInOut.instance.SceneFadeInOut("LevelSelect"); // ���������л�����
        }
    }
}
