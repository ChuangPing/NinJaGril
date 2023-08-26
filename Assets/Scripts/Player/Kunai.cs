using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kunai : MonoBehaviour
{
    public float kunaiSpeed; // unity�༭�����÷��ڷ����ٶ�
    GameObject player;
    Rigidbody2D myRig;
    private void Awake()
    {
        player = GameObject.Find("Player");
        myRig = GetComponent<Rigidbody2D>();

        if (player.transform.localScale.x == 1.0f)
        {
            // player:�����ҷ�,���ڷ�����
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            // ������������ҷ��е�����rigidbody2d����Ҫ����Ϊ0
            myRig.AddForce(Vector2.right * kunaiSpeed, ForceMode2D.Impulse);

        }
        else if (player.transform.localScale.x == -1.0f)
        {
            transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
            myRig.AddForce(Vector2.left * kunaiSpeed, ForceMode2D.Impulse);
        }
        // ����һ��ʱ���Զ�����
        Destroy(this.gameObject, 5.0f); // 5ms����
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.tag == "StopPoint" || collision.tag == "Item")
        {
            // ������ item�ȵ��߲��������Լ�
        }
        else
        {
            // ���ڴ��������壬ʹ�Լ���ʧ
            Destroy(this.gameObject);
        }
    }
}
