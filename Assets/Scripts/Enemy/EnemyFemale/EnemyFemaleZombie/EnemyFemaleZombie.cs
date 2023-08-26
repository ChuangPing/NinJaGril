using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
//using UnityEditor.Tilemaps;
using UnityEngine;

public class EnemyFemaleZombie : EnemyMaleZomble
{
    public float RunSpeed; // enemyFemale����player���ٶ�
    // ����ɾ��Ĭ�ϵ�start��update����
    bool isBattleMode; // �Ƿ����׷��player

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
                    // ����ʱ����player��λ�ã�����ת��
                    if (myPlayer.transform.position.x <= transform.position.x)
                    {
                        // myPlayer��enemyû����ͬһ��,player��enemy���棬enemy��Ҫת����myPlayer
                        transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
                    }
                    else
                    {
                        transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    }

                    //myAnim.SetTrigger("Attact"); // ����Attact����
                    Vector3 newTarget = new Vector3(myPlayer.transform.position.x, transform.position.y, transform.position.z);
                    if (myAnim.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
                    {
                        // female��������ʱ��������player���ȥ
                        transform.position = Vector3.MoveTowards(transform.position, newTarget, 2.0f * Time.deltaTime);
                    }

                    isAfterBattleCheck = true; // �����˹���
                    return; // ����Attact������ִֹ�������ѭ�������ƶ��߼�
                }
                else
                {
                    // ����������ת��
                    if (isAfterBattleCheck) // �����˹�����������Ҫ�ƶ���λ�ø���enemy����
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
                                // �ӳٵ��ã�Ч����enemy����playerһ�����ת��
                                //transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
                                StartCoroutine(TurnRight(false));
                            }
                            else if (turnPoint == originPosition)
                            {
                                StartCoroutine(TurnRight(true));
                            }
                        }

                        isAfterBattleCheck = false; // ��һ�ι������������������һ�ι�������
                    }

                }
            }
            else
            {
                // ����׷����ֹͣ��ʱ������StopPoint�������ص�turnPointʱת������
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
                    // ����ֹͣ�㣬���ص�turnPoint
                    transform.position = Vector3.MoveTowards(transform.position, turnPoint, mySpeed * Time.deltaTime);
                }

                if (transform.position == turnPoint)
                {
                    // enemFemale�ص�turnPoint���ֿ����ٴ�׷��
                    isBattleMode = true;
                }
                return;
            }

            if (transform.position.x == originPosition.x) // Enemy �ƶ��ص�Զ�� �������˱���û��ʼ�ƶ�����ԭ��ͻص�ԭ�㣩
            {
                // �����һ�λ�û��ʼ�ƶ�ִ��Idle
                if (!isFirstIdle) // �ƶ���һ�ξ��뿪ʼִ��Idle
                {
                    myAnim.SetTrigger("Idle"); // ����Idle����
                }
                turnPoint = targetPosition; // ��һ���ƶ���λ�����趨��Ŀ��λ��
                                            // �ӳ�2s��Idle����ִ�н���ת��
                StartCoroutine(TurnRight(false));
            }
            else if (transform.position.x == targetPosition.x)
            {
                // �ƶ���Ŀ��λ��
                myAnim.SetTrigger("Idle");
                turnPoint = originPosition; // ��һ����Ҫ�ƶ���λ��Ϊԭ����ԭ��
                StartCoroutine(TurnRight(true));
                isFirstIdle = false; // �ƶ���ִ��Idle
            }
            // �ж�Enemy Walk�����Ƿ���ִ��
            if (myAnim.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
            {
                // �ӵ�ǰλ���ƶ�Ŀ��λ�ã���һ����Ҫ�ƶ���λ�ã�
                transform.position = Vector3.MoveTowards(transform.position, turnPoint, mySpeed * Time.deltaTime);
            }
        }
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (collision.tag == "StopPoint")
        {
            isBattleMode = false; // enemyFemale����ֹͣ�㣬������׷��player
        }

        if (collision.tag == "PlayerAttact")
        {
            isBattleMode = true; // ���ܵ�player����ʱ��enemyFemaleҲ����׷��
        }
    }
}
