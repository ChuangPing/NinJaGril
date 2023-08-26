using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonCollider : MonoBehaviour
{
    // Start is called before the first frame update
    Player playerScript; // ������Ľű�ʵ��
    private void Awake()
    {
        playerScript = GetComponentInParent<Player>();
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // ��buttonCollision��ײ���ذ壬���ò��Ŷ�������
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Ground")
        {
            playerScript.canJump = true; // ������Ծ
            playerScript.myAnim.SetBool("Jump", false); // ֹͣ������Ծ����
        }

        // ��player��ײ���ƶ�ƽ̨���ͽ�player��Ϊ�ƶ�ƽ̨�������壬�����ƶ�ƽ̨һ���ƶ�
        if (collision.tag == "AirPlatform")
        {
            playerScript.canJump = true; // ������Ծ
            playerScript.myAnim.SetBool("Jump", false); // ֹͣ������Ծ����

            playerScript.transform.parent = collision.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "AirPlatform")
        {
            // ��player�뿪AirPlatform��ֹͣ���棨ȡ�����ӹ�ϵ��
            playerScript.transform.parent = null;
        }
    }
}
