using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    InputAction playerMove, playerJump, playerAttact, playerThrow;

    public float mySpeed; // 角色移动速度
    public float jumpForce; // 跳跃的高度
    public GameObject attactCollider; // 公开：默认隐藏，不能在脚本中直接获取，要通过unity客户端的方式将它进行赋值
    public GameObject KunaiPrefab; // 飞镖物体，unity编辑器进行赋值
    public AudioClip[] myAudioClip; // 音效资源

    // 设为public只是为了能在ButtonCollider中使用，不希望编辑器中能够修改，使用[]
    [HideInInspector]
    public bool canJump; // player是否落在地面
    [HideInInspector]
    public Animator myAnim;

    Rigidbody2D myRigi;
    SpriteRenderer mySr;
    AudioSource myAudioSource;

    bool isJumpPressed; // 跳跃键是否按下
    bool isAttact; // 是否处于攻击状态
    float kunaiDistance; // 控制飞镖水平位置的偏移，并根据player的方向进行左右偏移
    bool isHurt; // 是否处于受伤状态
    bool canBeHurt; // 能否处于受伤状态
    [HideInInspector] public int playerLife; // player生命值
    [HideInInspector] public int playerKunai; // player飞镖

    Canvas myCanvas;  // 控制界面：生命值、飞镖、宝石数量显示
    private void Awake()
    {
        // 获取组件
        myAnim = GetComponent<Animator>();
        myRigi = GetComponent<Rigidbody2D>();
        mySr = GetComponent<SpriteRenderer>();
        myAudioSource = GetComponent<AudioSource>();
        myCanvas = GameObject.Find("Canvas").GetComponent<Canvas>();

        isJumpPressed = false;
        canJump = true; // 刚开始player就在地面
        isAttact = false;
        isHurt = false;
        canBeHurt = true;
        //playerLife = 3;
        playerLife = PlayerPrefs.GetInt("PlayerLife"); // 获取玩家的默认初始生命值
        playerKunai = PlayerPrefs.GetInt("PlayerKunai");

        playerMove = GetComponent<PlayerInput>().currentActionMap["Move"];
        playerJump = GetComponent<PlayerInput>().currentActionMap["Jump"];
        playerAttact = GetComponent<PlayerInput>().currentActionMap["Attact"];
        playerThrow = GetComponent<PlayerInput>().currentActionMap["Kunai"];
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // 空格键控制角色跳跃
        //if (Input.GetKeyDown(KeyCode.Space) && canJump == true && isHurt == false) // 只有player在地面且不是受伤状态才能响应跳跃
        if (playerJump.triggered && canJump == true && isHurt == false) // 只有player在地面且不是受伤状态才能响应跳跃
        {
            isJumpPressed = true;
            canJump = false;
        }

        // t键控制角色攻击
        //if (Input.GetKeyDown(KeyCode.T) && isHurt == false)
        if (playerAttact.triggered && isHurt == false)
        {
            myAnim.SetTrigger("Attact"); // 播放攻击动画
            isAttact = true;
            canJump = false; // 在攻击时不能跳跃
        }

        // G键控制角色使用飞镖：当正在攻击（挥剑）和 正在丢飞镖动画时，不能进入防止多次点击导致数量减少
        //if (Input.GetKeyDown(KeyCode.G) && isHurt == false && !myAnim.GetCurrentAnimatorStateInfo(0).IsName("AttactThrow") && !myAnim.GetCurrentAnimatorStateInfo(0).IsName("Attact"))
        if (playerThrow.triggered && isHurt == false && !myAnim.GetCurrentAnimatorStateInfo(0).IsName("AttactThrow") && !myAnim.GetCurrentAnimatorStateInfo(0).IsName("Attact"))
        {
            if (playerKunai > 0)
            {
                playerKunai--; // 飞镖数量--
                PlayerPrefs.SetInt("PlayerKunai", playerKunai);
                myCanvas.KunaiUpdate(); // 更新UI飞镖的实时数量
                myAnim.SetTrigger("AttactThrow"); // 播放丢飞镖动画
                isAttact = true; // 正在播放攻击动画
                canJump = false;
            }
        }
    }

    private void FixedUpdate()
    {
        //float a = Input.GetAxisRaw("Horizontal"); // 水平方向
        float a = playerMove.ReadValue<Vector2>().x; // 水平方向
        if (isAttact == true || isHurt == true)
        {
            // 角色处于攻击状态不能移动 或处于受伤状态不能移动
            a = 0;
        }

        // 根据物体移动方向控制物体转向
        if (a > 0)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else if (a < 0)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        // 控制Run（跑）动画是否播放
        myAnim.SetFloat("Run", Mathf.Abs(a));

        // 跳跃
        if (isJumpPressed)
        {
            myRigi.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isJumpPressed = false; // 跳跃结束
            myAnim.SetBool("Jump", true); // 播放跳跃动画
        }
        // 移动
        if (!isHurt) // 没有受伤的状态
        {
            myRigi.velocity = new Vector2(a * mySpeed, myRigi.velocity.y);
        }


    }

    // TODO:由于可能存在受伤时，不能完整的播放攻击动画而这个函数又是在攻击动画的最后一帧执行，因此有可能执行不到
    // 因此需要在受伤的最后一帧调用这个函数
    public void SetIsAttactFalse() // 设置停止攻击状态，当攻击动画最后一帧结束时调用
    {
        isAttact = false;
        canJump = true; // 攻击动画播完，可以跳跃
        myAnim.ResetTrigger("Attact"); // 停止播放攻击动画
        myAnim.ResetTrigger("AttactThrow"); // 停止播放丢飞镖动画
    }

    public void SetAttactColliderOn() // 在攻击动画调用，显示wuq
    {
        attactCollider.SetActive(true); // 武器碰撞体：判断是否攻击到物体
    }

    public void SetAttactColliderOff() // 攻击动画关键帧调用（武器隐藏时调用）
    {

        attactCollider.SetActive(false); // 攻击结束，隐藏武器碰撞体
    }

    public void ForIsHurtSetting() // 受伤动画结束调用
    {
        isAttact = false;
        myAnim.ResetTrigger("Attact");
        myAnim.ResetTrigger("AttactThrow");
        attactCollider.SetActive(false); // 隐藏武器碰撞体
    }

    public void KunaiInstantiate() // 产生飞镖实例， Attact 函数关键帧结束调用
    {
        if (transform.localScale.x == 1.0f)
        {
            // plater超向右边
            kunaiDistance = 1.0f;
        }
        else if (transform.localScale.x == -1.0f)
        {
            kunaiDistance = -1.0f;
        }

        Vector3 temp = new Vector3(transform.position.x + kunaiDistance, transform.position.y, transform.position.z);
        Instantiate(KunaiPrefab, temp, Quaternion.identity); // 根据plater位置生产飞镖实例
    }

    public void PlaySwordEffect() // 剑音效，在剑第一帧动画时调用
    {
        myAudioSource.PlayOneShot(myAudioClip[3]);
    }

    public void PlayKunaiEffect() // 丢飞镖音效，在丢飞镖第一帧动画开始调用
    {
        myAudioSource.PlayOneShot(myAudioClip[2]);
    }

    // player碰触到enemy,时播放受伤的动画
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy" && isHurt == false && canBeHurt == true) // 防止多次触碰多次执行,并且还要确保处于能够处于受伤状态
        {

            playerLife--;
            // 将改变后的生命值存入到PlayerPre
            PlayerPrefs.SetInt("PlayerLife", playerLife);
            // 将更新后的数据响应到界面UI上
            myCanvas.lifeUpdate();
            if (playerLife >= 1)
            {
                // 播放受伤音效
                myAudioSource.PlayOneShot(myAudioClip[0]);
                isHurt = true; // 处于受伤状态
                canBeHurt = false; // 已经处于受伤状态，不能再处于受伤状态
                mySr.color = new Color(mySr.color.r, mySr.color.g, mySr.color.b, 0.5f); // 受伤时角色处于半透明状态
                myAnim.SetBool("Hurt", true); // 播放受伤动画
                                              // 根据player的方向：确定弹开的方向
                if (transform.localScale.x == 1.0f)
                {
                    // player朝向右，摊开时向左（后方）
                    myRigi.velocity = new Vector2(-2.5f, 10.0f); // 设置受伤向后和上方弹开效果
                }
                else if (transform.localScale.x == -1.0f)
                {
                    myRigi.velocity = new Vector2(2.5f, 10.0f);
                }

                StartCoroutine("SetHurtFalse");
            }
            else if (playerLife < 1)
            {
                myAudioSource.PlayOneShot(myAudioClip[4]);
                isHurt = true; // 处于受伤状态，停止player的攻击跳跃，移动
                isAttact = true;
                myRigi.velocity = new Vector2(0f, 0f); //死亡时取消碰到enemy向后弹的力量
                // 隐藏自己的碰撞体 TODO（避免enemy对死去的player一直攻击）
                myAnim.SetBool("Die", true); // 播放死亡动画
                // 设置player默认的生命值
                PlayerPrefs.SetInt("PlayerLife", 5);
                // 玩家死亡后，切换到关卡选择界面
                FadeInOut.instance.SceneFadeInOut("LevelSelect");
            }
        }

        if (collision.tag == "Item") // 触碰到道具
        {
            myAudioSource.PlayOneShot(myAudioClip[1]);
        }
    }

    // player 碰撞体stay状态,执行OnTriggerEnter2D一样的逻辑
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Enemy" && isHurt == false && canBeHurt == true) // 防止多次触碰多次执行,并且还要确保处于能够处于受伤状态
        {
            playerLife--;
            // 将改变后的生命值存入到PlayerPre
            PlayerPrefs.SetInt("PlayerLife", playerLife);
            // 将更新后的数据响应到界面UI上
            myCanvas.lifeUpdate();
            if (playerLife >= 1)
            {
                myAudioSource.PlayOneShot(myAudioClip[0]);
                isHurt = true; // 处于受伤状态
                canBeHurt = false; // 已经处于受伤状态，不能再处于受伤状态
                mySr.color = new Color(mySr.color.r, mySr.color.g, mySr.color.b, 0.5f); // 受伤时角色处于半透明状态
                myAnim.SetBool("Hurt", true); // 播放受伤动画

                // 根据player的方向：确定弹开的方向
                if (transform.localScale.x == 1.0f)
                {
                    // player朝向右，摊开时向左（后方）
                    myRigi.velocity = new Vector2(-2.5f, 10.0f); // 设置受伤向后和上方弹开效果
                }
                else if (transform.localScale.x == -1.0f)
                {
                    myRigi.velocity = new Vector2(2.5f, 10.0f);
                }
            }
            else if (playerLife < 1)
            {
                myAudioSource.PlayOneShot(myAudioClip[4]);
                isHurt = true; // 处于受伤状态，停止player的攻击跳跃，移动
                isAttact = true;
                myRigi.velocity = new Vector2(0f, 0f); //死亡时取消碰到enemy向后弹的力量
                // 隐藏自己的碰撞体 TODO（避免enemy对死去的player一直攻击）
                myAnim.SetBool("Die", true); // 播放死亡动画
                // 设置player默认的生命值
                PlayerPrefs.SetInt("PlayerLife", 5);
                // 玩家死亡后，切换到关卡选择界面
                FadeInOut.instance.SceneFadeInOut("LevelSelect");
            }


            StartCoroutine("SetHurtFalse");
        }
    }

    // player触碰到底部死亡
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.name == "BoundBottom")
        {
            myAudioSource.PlayOneShot(myAudioClip[4]);
            playerLife = 0;
            // 将修改的生命值保存
            PlayerPrefs.SetInt("PlayerLife", playerLife);
            // 同步UI显示
            myCanvas.lifeUpdate();
            isHurt = true;
            isAttact = true;
            myRigi.velocity = new Vector2(0f, 0f);
            myAnim.SetBool("Die", true);
            // 设置player默认的生命值
            PlayerPrefs.SetInt("PlayerLife", 5);
            // 玩家死亡后，切换到关卡选择界面
            FadeInOut.instance.SceneFadeInOut("LevelSelect");
        }
    }

    // 延迟1.0s调用，使受伤动画停止
    IEnumerator SetHurtFalse() // 当player触碰到enemy时，或播放受伤动画，并且同时向后上方弹开一段距离后，隔1.0s后回到ldle状态
    {
        yield return new WaitForSeconds(1.0f);
        isHurt = false; // 受伤状态结束
        myAnim.SetBool("Hurt", false); // 回到ldle东画

        yield return new WaitForSeconds(1.0f); // 结束受伤动画，延迟1s，让canBeHurt设置为true，再这1s内player无法再处于受伤状态  -- 无敌时间
        canBeHurt = true;
        mySr.color = new Color(mySr.color.r, mySr.color.g, mySr.color.b, 1.0f); // 恢复颜色（无敌时间结束）

    }
}
