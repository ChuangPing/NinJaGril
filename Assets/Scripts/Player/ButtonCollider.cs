using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonCollider : MonoBehaviour
{
    // Start is called before the first frame update
    Player playerScript; // 父物体的脚本实例
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

    // 当buttonCollision碰撞到地板，设置播放动画变量
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Ground")
        {
            playerScript.canJump = true; // 可以跳跃
            playerScript.myAnim.SetBool("Jump", false); // 停止播放跳跃动画
        }

        // 当player碰撞到移动平台，就将player设为移动平台的子物体，随着移动平台一起移动
        if (collision.tag == "AirPlatform")
        {
            playerScript.canJump = true; // 可以跳跃
            playerScript.myAnim.SetBool("Jump", false); // 停止播放跳跃动画

            playerScript.transform.parent = collision.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "AirPlatform")
        {
            // 当player离开AirPlatform，停止跟随（取消父子关系）
            playerScript.transform.parent = null;
        }
    }
}
