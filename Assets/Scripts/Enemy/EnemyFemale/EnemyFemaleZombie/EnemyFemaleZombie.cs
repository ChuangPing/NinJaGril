using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
//using UnityEditor.Tilemaps;
using UnityEngine;

public class EnemyFemaleZombie : EnemyMaleZomble
{
    public float RunSpeed; // enemyFemale冲向player的速度
    // 必须删除默认的start和update函数
    bool isBattleMode; // 是否可以追击player

    protected override void Awake()
    {
        base.Awake();
        turnPoint = targetPosition;
        isBattleMode = true;
    }
    protected override void MoveAndAttact()
    {
        //base.MoveAndAttact();
        if (isAlive)
        {
            if (isBattleMode)
            {
                if (Vector3.Distance(myPlayer.transform.position, transform.position) < 4.0f)
                {
                    // 攻击时根据player的位置，进行转向
                    if (myPlayer.transform.position.x <= transform.position.x)
                    {
                        // myPlayer与enemy没有在同一侧,player在enemy后面，enemy需要转身朝向myPlayer
                        transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
                    }
                    else
                    {
                        transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    }

                    //myAnim.SetTrigger("Attact"); // 播放Attact动画
                    Vector3 newTarget = new Vector3(myPlayer.transform.position.x, transform.position.y, transform.position.z);
                    if (myAnim.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
                    {
                        // female发生攻击时动作：向player冲过去
                        transform.position = Vector3.MoveTowards(transform.position, newTarget, 2.0f * Time.deltaTime);
                    }

                    isAfterBattleCheck = true; // 发生了攻击
                    return; // 播放Attact动画阻止执行下面的循环左右移动逻辑
                }
                else
                {
                    // 纠正攻击后转向
                    if (isAfterBattleCheck) // 发生了攻击，根据需要移动的位置更正enemy方向
                    {
                        if (transform.position.x > turnPoint.x || transform.position.x < turnPoint.x)
                        {
                            if (transform.position.x > turnPoint.x)
                            {
                                transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
                            }
                            else if (transform.position.x < turnPoint.x)
                            {
                                transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                            }
                        }
                        else
                        {
                            if (turnPoint == targetPosition)
                            {
                                // 延迟调用，效果：enemy看向player一会儿在转向
                                //transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
                                StartCoroutine(TurnRight(false));
                            }
                            else if (turnPoint == originPosition)
                            {
                                StartCoroutine(TurnRight(true));
                            }
                        }

                        isAfterBattleCheck = false; // 上一次攻击发生方向纠正，上一次攻击结束
                    }

                }
            }
            else
            {
                // 处理追击到停止点时（碰到StopPoint），返回到turnPoint时转向问题
                if (transform.position.x > turnPoint.x)
                {
                    transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
                }
                else
                {
                    transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                }
                if (myAnim.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
                {
                    // 到达停止点，返回到turnPoint
                    transform.position = Vector3.MoveTowards(transform.position, turnPoint, mySpeed * Time.deltaTime);
                }

                if (transform.position == turnPoint)
                {
                    // enemFemale回到turnPoint，又可以再次追击
                    isBattleMode = true;
                }
                return;
            }

            if (transform.position.x == originPosition.x) // Enemy 移动回到远点 （包含了本身还没开始移动就在原点和回到原点）
            {
                // 区别第一次还没开始移动执行Idle
                if (!isFirstIdle) // 移动了一段距离开始执行Idle
                {
                    myAnim.SetTrigger("Idle"); // 播放Idle动画
                }
                turnPoint = targetPosition; // 下一个移动的位置是设定的目标位置
                                            // 延迟2s等Idle动画执行结束转身
                StartCoroutine(TurnRight(false));
            }
            else if (transform.position.x == targetPosition.x)
            {
                // 移动到目标位置
                myAnim.SetTrigger("Idle");
                turnPoint = originPosition; // 下一次需要移动的位置为原来的原点
                StartCoroutine(TurnRight(true));
                isFirstIdle = false; // 移动后执行Idle
            }
            // 判断Enemy Walk动画是否再执行
            if (myAnim.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
            {
                // 从当前位置移动目标位置（下一个需要移动的位置）
                transform.position = Vector3.MoveTowards(transform.position, turnPoint, mySpeed * Time.deltaTime);
            }
        }
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (collision.tag == "StopPoint")
        {
            isBattleMode = false; // enemyFemale到达停止点，不能再追击player
        }

        if (collision.tag == "PlayerAttact")
        {
            isBattleMode = true; // 当受到player攻击时，enemyFemale也可以追击
        }
    }
}
