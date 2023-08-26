using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMaleZomble : MonoBehaviour
{
    public Vector3 targetPosition; // Enemy�ƶ���Ŀ��λ�ã�ͨ��unity�༭���趨��
    public float mySpeed; // Enemy �ƶ����ٶ�
    public GameObject attactCollider; // enemy ��ײ��
    public int enemyLife; // enemy����ֵ��ͨ��unity�༭���趨��

    protected Animator myAnim; // Enemy�������
    protected Vector3 originPosition; // ��ʼλ��
    protected Vector3 turnPoint; // ��һ����Ҫ�ƶ���λ��
    protected GameObject myPlayer;
    protected BoxCollider2D myCollider; // enemy��boxCollider;
    protected SpriteRenderer mySr; // enemy�������ı������������
    [SerializeField] // ʹ����public���ڱ༭����չʾ
    protected AudioClip[] myAudioClip; // ����enemyMan�Ĺ�����������Ч�ļ�
    protected AudioSource myAudioSource;

    protected bool isFirstIdle; // �Ƿ��ǵ�һ��Idle�������ƶ���ִ��Idle������
    protected bool isAfterBattleCheck = false; // ����������
    protected bool isAlive = true; // enemy�Ƿ����

    protected virtual void Awake()
    {
        myAnim = GetComponent<Animator>();
        originPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        isFirstIdle = true;
        myPlayer = GameObject.Find("Player");
        isAfterBattleCheck = false;
        enemyLife = 3;
        myCollider = GetComponent<BoxCollider2D>();
        isAlive = true;
        mySr = GetComponent<SpriteRenderer>();
        myAudioSource = GetComponent<AudioSource>();
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        MoveAndAttact();
    }

    protected virtual void MoveAndAttact() // �������ƶ�����
    {
        if (isAlive)
        {
            if (Vector3.Distance(myPlayer.transform.position, transform.position) < 1.3f)
            {
                // ����ʱ����player��λ�ã�����ת��
                if (myPlayer.transform.localPosition.x <= transform.position.x)
                {
                    // myPlayer��enemyû����ͬһ��,player��enemy���棬enemy��Ҫת����myPlayer
                    transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
                }
                else
                {
                    transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                }

                if (myAnim.GetCurrentAnimatorStateInfo(0).IsName("Attact") || myAnim.GetCurrentAnimatorStateInfo(0).IsName("AttactWait"))
                {
                    return; // ��ֹ�ظ�����
                }
                myAudioSource.PlayOneShot(myAudioClip[1]); // ���Ź�����Ч
                myAnim.SetTrigger("Attact"); // ����Attact����
                isAfterBattleCheck = true; // �����˹���
                return; // ����Attact������ִֹ�������ѭ�������ƶ��߼�
            }
            else
            {
                // ����������ת��
                if (isAfterBattleCheck) // �����˹�����������Ҫ�ƶ���λ�ø���enemy����
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
                    isAfterBattleCheck = false; // ��һ�ι������������������һ�ι�������
                }

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

    protected IEnumerator TurnRight(bool turnRight) // �ӳ�2��ִ��Enemyת��
    {
        yield return new WaitForSeconds(2.0f);
        if (turnRight)
        {
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }
        else
        {
            transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);

        }
    }

    // ��ʾ��ײ�壺���������ؼ�֡�е���
    public void SetAttactColliderOn()
    {
        attactCollider.SetActive(true);
    }

    // ������ײ�壺��������֡���ú�������ʱ��һ֡���ú�����ʱ��һ֡����
    public void SetAttactColliderOff()
    {
        attactCollider.SetActive(false);
    }

    // ��enemy������player��������ײ�壨�����߷��ڣ��������˶���
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PlayerAttact")
        {
            // ����������Ч
            myAudioSource.PlayOneShot(myAudioClip[0]);
            enemyLife--;
            if (enemyLife >= 1)
            {
                myAnim.SetTrigger("Hurt");
            }
            else if (enemyLife < 1)
            {
                isAlive = false;
                // enemy �����������Լ���collision����ֹplayer������������enemy�������˶���
                myCollider.enabled = false;
                myAnim.SetTrigger("Die");
                StartCoroutine("AfterDie");
            }
        }
    }

    // enemy��������
    IEnumerator AfterDie()
    {
        yield return new WaitForSeconds(1.0f);
        mySr.material.color = new Color(1.0f, 1.0f, 1.0f, 0.5f); // ʹ��1.0f��ʾԭ����rgbû�з����仯��ֻ��͸���ȷ����仯

        yield return new WaitForSeconds(1.0f);
        mySr.material.color = new Color(1.0f, 1.0f, 1.0f, 0.2f);

        yield return new WaitForSeconds(1.0f);
        Destroy(this.gameObject);
    }

}
