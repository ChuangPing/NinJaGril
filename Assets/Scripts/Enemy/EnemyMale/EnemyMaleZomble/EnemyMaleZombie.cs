using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMaleZomble : MonoBehaviour
{
    public Vector3 targetPosition; // Enemy移动的目标位置（通过unity编辑器设定）
    public float mySpeed; // Enemy 移动的速度
    public GameObject attactCollider; // enemy 碰撞体
    public int enemyLife; // enemy生命值（通过unity编辑器设定）

    protected Animator myAnim; // Enemy动画组件
    protected Vector3 originPosition; // 初始位置
    protected Vector3 turnPoint; // 下一个需要移动的位置
    protected GameObject myPlayer;
    protected BoxCollider2D myCollider; // enemy的boxCollider;
    protected SpriteRenderer mySr; // enemy死亡，改变材质慢慢死亡
    [SerializeField] // 使不是public的在编辑器中展示
    protected AudioClip[] myAudioClip; // 设置enemyMan的攻击和受伤音效文件
    protected AudioSource myAudioSource;

    protected bool isFirstIdle; // 是否是第一次Idle（区别移动后执行Idle动画）
    protected bool isAfterBattleCheck = false; // 发生攻击后
    protected bool isAlive = true; // enemy是否活着

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

    protected virtual void MoveAndAttact() // 攻击和移动函数
    {
        if (isAlive)
        {
            if (Vector3.Distance(myPlayer.transform.position, transform.position) < 1.3f)
            {
                // 攻击时根据player的位置，进行转向
                if (myPlayer.transform.localPosition.x <= transform.position.x)
                {
                    // myPlayer与enemy没有在同一侧,player在enemy后面，enemy需要转身朝向myPlayer
                    transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
                }
                else
                {
                    transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                }

                if (myAnim.GetCurrentAnimatorStateInfo(0).IsName("Attact") || myAnim.GetCurrentAnimatorStateInfo(0).IsName("AttactWait"))
                {
                    return; // 防止重复攻击
                }
                myAudioSource.PlayOneShot(myAudioClip[1]); // 播放攻击音效
                myAnim.SetTrigger("Attact"); // 播放Attact动画
                isAfterBattleCheck = true; // 发生了攻击
                return; // 播放Attact动画阻止执行下面的循环左右移动逻辑
            }
            else
            {
                // 纠正攻击后转向
                if (isAfterBattleCheck) // 发生了攻击，根据需要移动的位置更正enemy方向
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
                    isAfterBattleCheck = false; // 上一次攻击发生方向纠正，上一次攻击结束
                }

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

    protected IEnumerator TurnRight(bool turnRight) // 延迟2秒执行Enemy转身
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

    // 显示碰撞体：攻击动画关键帧中调用
    public void SetAttactColliderOn()
    {
        attactCollider.SetActive(true);
    }

    // 隐藏碰撞体：攻击结束帧调用和在受伤时第一帧调用和受伤时第一帧调用
    public void SetAttactColliderOff()
    {
        attactCollider.SetActive(false);
    }

    // 当enemy触碰到player武器的碰撞体（刀或者飞镖）播放受伤动画
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PlayerAttact")
        {
            // 播放受伤音效
            myAudioSource.PlayOneShot(myAudioClip[0]);
            enemyLife--;
            if (enemyLife >= 1)
            {
                myAnim.SetTrigger("Hurt");
            }
            else if (enemyLife < 1)
            {
                isAlive = false;
                // enemy 死亡：隐藏自己的collision，防止player碰触到死亡了enemy触发受伤动画
                myCollider.enabled = false;
                myAnim.SetTrigger("Die");
                StartCoroutine("AfterDie");
            }
        }
    }

    // enemy死亡过程
    IEnumerator AfterDie()
    {
        yield return new WaitForSeconds(1.0f);
        mySr.material.color = new Color(1.0f, 1.0f, 1.0f, 0.5f); // 使用1.0f表示原来的rgb没有发生变化，只有透明度发生变化

        yield return new WaitForSeconds(1.0f);
        mySr.material.color = new Color(1.0f, 1.0f, 1.0f, 0.2f);

        yield return new WaitForSeconds(1.0f);
        Destroy(this.gameObject);
    }

}
