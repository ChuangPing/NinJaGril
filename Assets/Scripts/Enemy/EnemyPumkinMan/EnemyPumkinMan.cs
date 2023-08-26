using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Rendering;

public class EnemyPumkinMan : MonoBehaviour
{
    bool isAlive; // 是否存活
    bool isIdle; // 是否能处于Idle状态
    bool jumpAttact; // 是否处于跳跃攻击状态
    bool isJumpUp; // 是否能处于跳跃的上升阶段（跳跃动画分为两部分）
    //bool isJumpDown; // 是否能处于跳跃下降阶段
    bool slideAttact; // 处于滑动攻击状态
    bool isHurt; // 是否处于受伤状态
    bool canBeHurt; // 是否能处于受伤状态

    public int life;
    public float attactDistance; // 收到攻击的距离
    public float jumpHeight; // 跳跃的高度
    public float jumpUpSpeed; // 跳跃速度
    public float jumpDownSpeed; // 调阅下降速度
    public float slideSpeed; // 滑行速度
    public float failDownSpeed; // 死亡或受伤时如果在空中，掉落的速度

    GameObject player;
    Animator myAnim;
    BoxCollider2D myCollider;
    SpriteRenderer mySr;
    AudioSource myAudioSource;

    Vector3 sliderTargetPosition; // slider攻击滑动位置

    public void Awake()
    {
        isAlive = true;
        isIdle = true; // 默认开始的动画就是Idle
        jumpAttact = false;
        isJumpUp = true;
        slideAttact = false;
        isHurt = false;
        canBeHurt = true; // 默认能够处于受伤状态

        player = GameObject.Find("Player");
        myAnim = GetComponent<Animator>();
        myCollider = GetComponent<BoxCollider2D>();
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
        if (isAlive)
        {
            if (isIdle)
            {
                lookAtPlayer();
                // 当player与enemyPumKinMan距离小于2.0f时，滑翔攻击
                if (Vector3.Distance(player.transform.position, transform.position) <= attactDistance)
                {
                    // slideAttact 开始在空中滑翔攻击
                    isIdle = false;
                    StartCoroutine("IdleToSlideAttact");
                    //slideAttact = true; // 满足滑动攻击
                }
                else //大于 2.0f时，enemyPumKinMan跳跃到player头顶开始跳跃攻击
                {
                    // jumpAttact
                    isIdle = false;// 攻击状态不能处于Idle
                    //jumpAttact = true; // 处于跳跃攻击状态
                    // 平滑处理：不是当能处于攻击状态时马上就开始攻击，而是延迟1秒
                    StartCoroutine("IdleToJumpAttact");

                }
            }
            else if (jumpAttact) // 处于跳跃攻击状态
            {
                lookAtPlayer();
                if (isJumpUp)
                {
                    // 跳跃的目标位置，player头顶上方3.5f处
                    Vector3 myTarget = new Vector3(player.transform.position.x, jumpHeight, transform.position.z);
                    transform.position = Vector3.MoveTowards(transform.position, myTarget, jumpUpSpeed * Time.deltaTime);
                    myAnim.SetBool("JumpUp", true);
                }
                else
                {
                    // 跳跃up状态完成，执行down状态
                    myAnim.SetBool("JumpUp", false);
                    myAnim.SetBool("JumpDown", true);
                    // 上一阶段跳到指定高度3.5f后，下降阶段需要移动到地板-2.97f
                    Vector3 myTarget = new Vector3(transform.position.x, -2.97f, transform.position.z);
                    transform.position = Vector3.MoveTowards(transform.position, myTarget, jumpDownSpeed * Time.deltaTime);
                }

                if (transform.position.y == jumpHeight)
                {
                    isJumpUp = false; // 已经跳跃到指定位置，一阶段跳跃结束
                }
                else if (transform.position.y == -2.97f)
                {
                    jumpAttact = false; // 已经落到地面，跳跃攻击阶段结束
                    StartCoroutine("JumpDownToIdle"); // 延迟0.5秒执行，而不是僵硬的落地就执行Idle动画
                }
            }
            else if (slideAttact)
            {
                myAnim.SetBool("Slide", true);
                transform.position = Vector3.MoveTowards(transform.position, sliderTargetPosition, slideSpeed * Time.deltaTime);
                // 判断滑动是否结束
                if (transform.position == sliderTargetPosition)
                {
                    myAnim.SetBool("Slide", false);
                    slideAttact = false; // 滑动攻击结束
                    isIdle = true;
                    // 滑动结束将碰撞体设为原来的
                    myCollider.offset = new Vector2(-0.191112f, -0.1454114f);
                    myCollider.size = new Vector2(1.099079f, 2.036471f);
                }
            }
            else if (isHurt)
            {
                // 处于受伤状态,移动地面
                Vector3 myTargetPosition = new Vector3(transform.position.x, -2.975f, transform.position.z);
                transform.position = Vector3.MoveTowards(transform.position, myTargetPosition, failDownSpeed * Time.deltaTime);
            }
        }
        else
        {
            // 确保死亡后位置在地板上
            Vector3 myTargetPosition = new Vector3(transform.position.x, -2.975f, transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, myTargetPosition, failDownSpeed * Time.deltaTime);
        }

    }

    void lookAtPlayer() // 使enemyPumKinMan始终看向player
    {
        if (player.transform.position.x > transform.position.x)
        {
            //  player在enemyPumkinMan的右边，看向右边
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }
        else
        {
            transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        }
    }

    IEnumerator IdleToSlideAttact() // Idle状态到slideAttact状态延迟转换
    {
        yield return new WaitForSeconds(1);
        lookAtPlayer(); // 滑动攻击时保证enemy看向player
        sliderTargetPosition = new Vector3(player.transform.position.x, transform.position.y, transform.position.z); // 只是用player的x
        // 重新设置碰撞体的大小
        myCollider.offset = new Vector2(-0.191112f, -0.4237702f);
        myCollider.size = new Vector2(1.099079f, 1.479754f);
        slideAttact = true;
    }

    IEnumerator JumpDownToIdle()
    {
        yield return new WaitForSeconds(0.5f);
        isIdle = true;
        isJumpUp = true; // 当前跳跃结束，下一个又可以执行跳跃动画
        // 停止所有的跳跃动画
        myAnim.SetBool("JumpUp", false);
        myAnim.SetBool("JumpDown", false);
    }

    IEnumerator IdleToJumpAttact()
    {
        yield return new WaitForSeconds(0.5f);
        jumpAttact = true;
    }

    IEnumerator SetAnimHurtToFalse()
    {
        yield return new WaitForSeconds(0.5f);
        // 初始化受伤状态：不能播放其它动画
        myAnim.SetBool("Hurt", false); // 结束受伤状态
        myAnim.SetBool("JumpUp", false);
        myAnim.SetBool("JumpDown", false);
        myAnim.SetBool("Slide", false);
        mySr.material.color = new Color(1.0f, 1.0f, 1.0f, 0.5f); // 提示这是无敌时间半透明
        isHurt = false;
        isIdle = true; // 切换为Idle状态

        yield return new WaitForSeconds(2.0f); // 受伤后，无敌时间2s，2s后才能再次受伤
        mySr.material.color = new Color(1.0f, 1.0f, 1.0f, 0.5f); // 无敌时间结束，前面三个1.0f表示设置原来的颜色
        canBeHurt = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.tag == "PlayerAttact")
        {
            if (canBeHurt)
            {
                // 播放受伤音效
                myAudioSource.PlayOneShot(myAudioSource.clip);
                life--;
                if (life >= 1)
                {
                    // 重置状态，防止在受伤时出现播放其它动画等异常情况
                    isIdle = false;
                    jumpAttact = false;
                    slideAttact = false;
                    StopCoroutine("JumpDownToIdle");
                    StopCoroutine("IdleToJumpAttact");
                    StopCoroutine("IdleToJumpAttact");

                    isHurt = true; // 当前处于受伤状态
                    myAnim.SetBool("Hurt", true);
                    StartCoroutine("SetAnimHurtToFalse"); // 延迟调用结束受伤状态
                }
                else
                {
                    isAlive = false; // 停止执行攻击移动等一系列逻辑
                    myCollider.enabled = false; // enemyPumkinMan已经死亡，隐藏它的Collider
                    StopAllCoroutines(); // 停止所有的coroutine
                    myAnim.SetBool("Die", true);

                    // 设定过3秒，自动转换到关卡选择场景
                    Time.timeScale = 0.5f; // 死亡后时间变慢一点
                    StartCoroutine("AfterDie");
                }
                canBeHurt = false; // 已经在受伤状态, 不能在进入受伤状态

            }
        }
    }

    IEnumerator AfterDie()
    {
        yield return new WaitForSecondsRealtime(3.0f);
        FadeInOut.instance.SceneFadeInOut("LevelSelect");
    }

}
