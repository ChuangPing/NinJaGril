using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Canvas : MonoBehaviour
{
    public Text lifeText, kunaiText, stoneText; // player�����ڡ���ʯ����UI

    public void Awake()
    {
        // Level1�����ȥ����: 
        lifeUpdate();
        KunaiUpdate();
        StoneUpdate();
    }

    // ����Player life����ֵUI����
    public void lifeUpdate()
    {
        lifeText.text = "X" + PlayerPrefs.GetInt("PlayerLife").ToString();
    }

    // ���·�������UI����
    public void KunaiUpdate()
    {
        kunaiText.text = "X" + PlayerPrefs.GetInt("PlayerKunai").ToString();
    }

    // ���±�ʯ����UI����
    public void StoneUpdate()
    {
        stoneText.text = "X" + PlayerPrefs.GetInt("PlayerStone").ToString();
    }
}
