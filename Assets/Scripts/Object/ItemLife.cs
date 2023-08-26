using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemLife : MonoBehaviour
{
    Player myPlayer;
    Canvas myCanvas;

    private void Awake()
    {
        // 获取Player脚本实例
        myPlayer = GameObject.Find("Player").GetComponent<Player>();
        myCanvas = GameObject.Find("Canvas").GetComponent<Canvas>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Player")
        {
            // Player碰触到LifeItem，为玩家增加生命值
            int life = PlayerPrefs.GetInt("PlayerLife") + 1;
            PlayerPrefs.SetInt("PlayerLife", life);
            // 更新Player的生命值
            myPlayer.playerLife = life;
            // 更新UI显示
            myCanvas.lifeUpdate();
            // 道具消失
            Destroy(this.gameObject);

        }
    }
}
