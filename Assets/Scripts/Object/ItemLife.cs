using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemLife : MonoBehaviour
{
    Player myPlayer;
    Canvas myCanvas;

    private void Awake()
    {
        // ��ȡPlayer�ű�ʵ��
        myPlayer = GameObject.Find("Player").GetComponent<Player>();
        myCanvas = GameObject.Find("Canvas").GetComponent<Canvas>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Player")
        {
            // Player������LifeItem��Ϊ�����������ֵ
            int life = PlayerPrefs.GetInt("PlayerLife") + 1;
            PlayerPrefs.SetInt("PlayerLife", life);
            // ����Player������ֵ
            myPlayer.playerLife = life;
            // ����UI��ʾ
            myCanvas.lifeUpdate();
            // ������ʧ
            Destroy(this.gameObject);

        }
    }
}
