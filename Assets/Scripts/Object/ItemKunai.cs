using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemKunai : MonoBehaviour
{
    // ���ڵ��߽ű�
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
            // ����Ҵ��������ڵ���
            int kunai = PlayerPrefs.GetInt("PlayerKunai") + 1;
            PlayerPrefs.SetInt("PlayerKunai", kunai);
            // ������ҵ�ǰkunai���������µ�UI
            myPlayer.playerKunai = kunai;
            myCanvas.KunaiUpdate();

            // ���ٵ�Ǹ����
            Destroy(this.gameObject);
        }
    }
}
