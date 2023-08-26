using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Canvas : MonoBehaviour
{
    public Text lifeText, kunaiText, stoneText; // player、飞镖、宝石数量UI

    public void Awake()
    {
        // Level1进入进去调用: 
        lifeUpdate();
        KunaiUpdate();
        StoneUpdate();
    }

    // 更新Player life生命值UI内容
    public void lifeUpdate()
    {
        lifeText.text = "X" + PlayerPrefs.GetInt("PlayerLife").ToString();
    }

    // 更新飞镖数量UI内容
    public void KunaiUpdate()
    {
        kunaiText.text = "X" + PlayerPrefs.GetInt("PlayerKunai").ToString();
    }

    // 更新宝石数量UI内容
    public void StoneUpdate()
    {
        stoneText.text = "X" + PlayerPrefs.GetInt("PlayerStone").ToString();
    }
}
