using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsFirstTimePlayCheck : MonoBehaviour
{
    private void Awake()
    {
        FirstTimePlayState();
    }

    public void FirstTimePlayState()
    {
        // �����һ�ν�����Ϸ
        if (!PlayerPrefs.HasKey("IsFirstTimePlay"))
        {
            PlayerPrefs.SetInt("IsFirstTimePlay", 1);
            PlayerPrefs.SetInt("PlayerLife", 5); // �������ֵ
            PlayerPrefs.SetInt("PlayerKunai", 2); // ��ʼ���ڸ���
            PlayerPrefs.SetInt("PlayerStone", 0); // ��ʯ����
            PlayerPrefs.SetInt("clearLevel", 0); // �ؿ�
        }
    }
}
