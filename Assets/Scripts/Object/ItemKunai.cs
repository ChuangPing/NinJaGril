using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemKunai : MonoBehaviour
{
    // 飞镖道具脚本
    Player myPlayer;
    Canvas myCanvas;

    private void Awake()
    {
        myPlayer = GameObject.Find("Player").GetComponent<Player>();
        myCanvas = GameObject.Find("Canvas").GetComponent<Canvas>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Player")
        {
            // 当玩家触碰到飞镖道据
            int kunai = PlayerPrefs.GetInt("PlayerKunai") + 1;
            PlayerPrefs.SetInt("PlayerKunai", kunai);
            // 设置玩家当前kunai数量并更新到UI
            myPlayer.playerKunai = kunai;
            myCanvas.KunaiUpdate();

            // 销毁道歉道具
            Destroy(this.gameObject);
        }
    }
}
