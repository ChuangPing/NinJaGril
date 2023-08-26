using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kunai : MonoBehaviour
{
    public float kunaiSpeed; // unity编辑器设置飞镖飞行速度
    GameObject player;
    Rigidbody2D myRig;
    private void Awake()
    {
        player = GameObject.Find("Player");
        myRig = GetComponent<Rigidbody2D>();

        if (player.transform.localScale.x == 1.0f)
        {
            // player:朝向右方,飞镖方向朝右
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            // 给飞镖添加向右飞行的力，rigidbody2d重力要设置为0
            myRig.AddForce(Vector2.right * kunaiSpeed, ForceMode2D.Impulse);

        }
        else if (player.transform.localScale.x == -1.0f)
        {
            transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
            myRig.AddForce(Vector2.left * kunaiSpeed, ForceMode2D.Impulse);
        }
        // 飞行一段时间自动销毁
        Destroy(this.gameObject, 5.0f); // 5ms销毁
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
            // 触碰到 item等道具不会销毁自己
        }
        else
        {
            // 飞镖触碰到物体，使自己消失
            Destroy(this.gameObject);
        }
    }
}
