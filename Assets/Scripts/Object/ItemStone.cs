using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemStone : MonoBehaviour
{
    // 宝石道具脚本
    Canvas myCanvas; // UI脚本实例

    private void Awake()
    {
        myCanvas = GameObject.Find("Canvas").GetComponent<Canvas>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Player")
        {
            int stone = PlayerPrefs.GetInt("PlayerStone") + 1;
            PlayerPrefs.SetInt("PlayerStone", stone);
            myCanvas.StoneUpdate();
            Destroy(this.gameObject);
        }
    }
}
