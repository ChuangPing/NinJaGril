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
        // 如果第一次进入游戏
        if (!PlayerPrefs.HasKey("IsFirstTimePlay"))
        {
            PlayerPrefs.SetInt("IsFirstTimePlay", 1);
            PlayerPrefs.SetInt("PlayerLife", 5); // 玩家生命值
            PlayerPrefs.SetInt("PlayerKunai", 2); // 初始飞镖个数
            PlayerPrefs.SetInt("PlayerStone", 0); // 宝石个数
            PlayerPrefs.SetInt("clearLevel", 0); // 关卡
        }
    }
}
