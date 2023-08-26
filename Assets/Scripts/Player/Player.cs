using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    InputAction playerMove, playerJump, playerAttact, playerThrow;

    public float mySpeed; // ��ɫ�ƶ��ٶ�
    public float jumpForce; // ��Ծ�ĸ߶�
    public GameObject attactCollider; // ������Ĭ�����أ������ڽű���ֱ�ӻ�ȡ��Ҫͨ��unity�ͻ��˵ķ�ʽ�������и�ֵ
    public GameObject KunaiPrefab; // �������壬unity�༭�����и�ֵ
    public AudioClip[] myAudioClip; // ��Ч��Դ

    // ��Ϊpublicֻ��Ϊ������ButtonCollider��ʹ�ã���ϣ���༭�����ܹ��޸ģ�ʹ��[]
    [HideInInspector]
    public bool canJump; // player�Ƿ����ڵ���
    [HideInInspector]
    public Animator myAnim;

    Rigidbody2D myRigi;
    SpriteRenderer mySr;
    AudioSource myAudioSource;

    bool isJumpPressed; // ��Ծ���Ƿ���
    bool isAttact; // �Ƿ��ڹ���״̬
    float kunaiDistance; // ���Ʒ���ˮƽλ�õ�ƫ�ƣ�������player�ķ����������ƫ��
    bool isHurt; // �Ƿ�������״̬
    bool canBeHurt; // �ܷ�������״̬
    [HideInInspector] public int playerLife; // player����ֵ
    [HideInInspector] public int playerKunai; // player����

    Canvas myCanvas;  // ���ƽ��棺����ֵ�����ڡ���ʯ������ʾ
    private void Awake()
    {
        // ��ȡ���
        myAnim = GetComponent<Animator>();
        myRigi = GetComponent<Rigidbody2D>();
        mySr = GetComponent<SpriteRenderer>();
        myAudioSource = GetComponent<AudioSource>();
        myCanvas = GameObject.Find("Canvas").GetComponent<Canvas>();

        isJumpPressed = false;
        canJump = true; // �տ�ʼplayer���ڵ���
        isAttact = false;
        isHurt = false;
        canBeHurt = true;
        //playerLife = 3;
        playerLife = PlayerPrefs.GetInt("PlayerLife"); // ��ȡ��ҵ�Ĭ�ϳ�ʼ����ֵ
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
        // �ո�����ƽ�ɫ��Ծ
        //if (Input.GetKeyDown(KeyCode.Space) && canJump == true && isHurt == false) // ֻ��player�ڵ����Ҳ�������״̬������Ӧ��Ծ
        if (playerJump.triggered && canJump == true && isHurt == false) // ֻ��player�ڵ����Ҳ�������״̬������Ӧ��Ծ
        {
            isJumpPressed = true;
            canJump = false;
        }

        // t�����ƽ�ɫ����
        //if (Input.GetKeyDown(KeyCode.T) && isHurt == false)
        if (playerAttact.triggered && isHurt == false)
        {
            myAnim.SetTrigger("Attact"); // ���Ź�������
            isAttact = true;
            canJump = false; // �ڹ���ʱ������Ծ
        }

        // G�����ƽ�ɫʹ�÷��ڣ������ڹ������ӽ����� ���ڶ����ڶ���ʱ�����ܽ����ֹ��ε��������������
        //if (Input.GetKeyDown(KeyCode.G) && isHurt == false && !myAnim.GetCurrentAnimatorStateInfo(0).IsName("AttactThrow") && !myAnim.GetCurrentAnimatorStateInfo(0).IsName("Attact"))
        if (playerThrow.triggered && isHurt == false && !myAnim.GetCurrentAnimatorStateInfo(0).IsName("AttactThrow") && !myAnim.GetCurrentAnimatorStateInfo(0).IsName("Attact"))
        {
            if (playerKunai > 0)
            {
                playerKunai--; // ��������--
                PlayerPrefs.SetInt("PlayerKunai", playerKunai);
                myCanvas.KunaiUpdate(); // ����UI���ڵ�ʵʱ����
                myAnim.SetTrigger("AttactThrow"); // ���Ŷ����ڶ���
                isAttact = true; // ���ڲ��Ź�������
                canJump = false;
            }
        }
    }

    private void FixedUpdate()
    {
        //float a = Input.GetAxisRaw("Horizontal"); // ˮƽ����
        float a = playerMove.ReadValue<Vector2>().x; // ˮƽ����
        if (isAttact == true || isHurt == true)
        {
            // ��ɫ���ڹ���״̬�����ƶ� ��������״̬�����ƶ�
            a = 0;
        }

        // ���������ƶ������������ת��
        if (a > 0)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else if (a < 0)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        // ����Run���ܣ������Ƿ񲥷�
        myAnim.SetFloat("Run", Mathf.Abs(a));

        // ��Ծ
        if (isJumpPressed)
        {
            myRigi.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isJumpPressed = false; // ��Ծ����
            myAnim.SetBool("Jump", true); // ������Ծ����
        }
        // �ƶ�
        if (!isHurt) // û�����˵�״̬
        {
            myRigi.velocity = new Vector2(a * mySpeed, myRigi.velocity.y);
        }


    }

    // TODO:���ڿ��ܴ�������ʱ�����������Ĳ��Ź���������������������ڹ������������һִ֡�У�����п���ִ�в���
    // �����Ҫ�����˵����һ֡�����������
    public void SetIsAttactFalse() // ����ֹͣ����״̬���������������һ֡����ʱ����
    {
        isAttact = false;
        canJump = true; // �����������꣬������Ծ
        myAnim.ResetTrigger("Attact"); // ֹͣ���Ź�������
        myAnim.ResetTrigger("AttactThrow"); // ֹͣ���Ŷ����ڶ���
    }

    public void SetAttactColliderOn() // �ڹ����������ã���ʾwuq
    {
        attactCollider.SetActive(true); // ������ײ�壺�ж��Ƿ񹥻�������
    }

    public void SetAttactColliderOff() // ���������ؼ�֡���ã���������ʱ���ã�
    {

        attactCollider.SetActive(false); // ��������������������ײ��
    }

    public void ForIsHurtSetting() // ���˶�����������
    {
        isAttact = false;
        myAnim.ResetTrigger("Attact");
        myAnim.ResetTrigger("AttactThrow");
        attactCollider.SetActive(false); // ����������ײ��
    }

    public void KunaiInstantiate() // ��������ʵ���� Attact �����ؼ�֡��������
    {
        if (transform.localScale.x == 1.0f)
        {
            // plater�����ұ�
            kunaiDistance = 1.0f;
        }
        else if (transform.localScale.x == -1.0f)
        {
            kunaiDistance = -1.0f;
        }

        Vector3 temp = new Vector3(transform.position.x + kunaiDistance, transform.position.y, transform.position.z);
        Instantiate(KunaiPrefab, temp, Quaternion.identity); // ����platerλ����������ʵ��
    }

    public void PlaySwordEffect() // ����Ч���ڽ���һ֡����ʱ����
    {
        myAudioSource.PlayOneShot(myAudioClip[3]);
    }

    public void PlayKunaiEffect() // ��������Ч���ڶ����ڵ�һ֡������ʼ����
    {
        myAudioSource.PlayOneShot(myAudioClip[2]);
    }

    // player������enemy,ʱ�������˵Ķ���
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy" && isHurt == false && canBeHurt == true) // ��ֹ��δ������ִ��,���һ�Ҫȷ�������ܹ���������״̬
        {

            playerLife--;
            // ���ı�������ֵ���뵽PlayerPre
            PlayerPrefs.SetInt("PlayerLife", playerLife);
            // �����º��������Ӧ������UI��
            myCanvas.lifeUpdate();
            if (playerLife >= 1)
            {
                // ����������Ч
                myAudioSource.PlayOneShot(myAudioClip[0]);
                isHurt = true; // ��������״̬
                canBeHurt = false; // �Ѿ���������״̬�������ٴ�������״̬
                mySr.color = new Color(mySr.color.r, mySr.color.g, mySr.color.b, 0.5f); // ����ʱ��ɫ���ڰ�͸��״̬
                myAnim.SetBool("Hurt", true); // �������˶���
                                              // ����player�ķ���ȷ�������ķ���
                if (transform.localScale.x == 1.0f)
                {
                    // player�����ң�̯��ʱ���󣨺󷽣�
                    myRigi.velocity = new Vector2(-2.5f, 10.0f); // �������������Ϸ�����Ч��
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
                isHurt = true; // ��������״̬��ֹͣplayer�Ĺ�����Ծ���ƶ�
                isAttact = true;
                myRigi.velocity = new Vector2(0f, 0f); //����ʱȡ������enemy��󵯵�����
                // �����Լ�����ײ�� TODO������enemy����ȥ��playerһֱ������
                myAnim.SetBool("Die", true); // ������������
                // ����playerĬ�ϵ�����ֵ
                PlayerPrefs.SetInt("PlayerLife", 5);
                // ����������л����ؿ�ѡ�����
                FadeInOut.instance.SceneFadeInOut("LevelSelect");
            }
        }

        if (collision.tag == "Item") // ����������
        {
            myAudioSource.PlayOneShot(myAudioClip[1]);
        }
    }

    // player ��ײ��stay״̬,ִ��OnTriggerEnter2Dһ�����߼�
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Enemy" && isHurt == false && canBeHurt == true) // ��ֹ��δ������ִ��,���һ�Ҫȷ�������ܹ���������״̬
        {
            playerLife--;
            // ���ı�������ֵ���뵽PlayerPre
            PlayerPrefs.SetInt("PlayerLife", playerLife);
            // �����º��������Ӧ������UI��
            myCanvas.lifeUpdate();
            if (playerLife >= 1)
            {
                myAudioSource.PlayOneShot(myAudioClip[0]);
                isHurt = true; // ��������״̬
                canBeHurt = false; // �Ѿ���������״̬�������ٴ�������״̬
                mySr.color = new Color(mySr.color.r, mySr.color.g, mySr.color.b, 0.5f); // ����ʱ��ɫ���ڰ�͸��״̬
                myAnim.SetBool("Hurt", true); // �������˶���

                // ����player�ķ���ȷ�������ķ���
                if (transform.localScale.x == 1.0f)
                {
                    // player�����ң�̯��ʱ���󣨺󷽣�
                    myRigi.velocity = new Vector2(-2.5f, 10.0f); // �������������Ϸ�����Ч��
                }
                else if (transform.localScale.x == -1.0f)
                {
                    myRigi.velocity = new Vector2(2.5f, 10.0f);
                }
            }
            else if (playerLife < 1)
            {
                myAudioSource.PlayOneShot(myAudioClip[4]);
                isHurt = true; // ��������״̬��ֹͣplayer�Ĺ�����Ծ���ƶ�
                isAttact = true;
                myRigi.velocity = new Vector2(0f, 0f); //����ʱȡ������enemy��󵯵�����
                // �����Լ�����ײ�� TODO������enemy����ȥ��playerһֱ������
                myAnim.SetBool("Die", true); // ������������
                // ����playerĬ�ϵ�����ֵ
                PlayerPrefs.SetInt("PlayerLife", 5);
                // ����������л����ؿ�ѡ�����
                FadeInOut.instance.SceneFadeInOut("LevelSelect");
            }


            StartCoroutine("SetHurtFalse");
        }
    }

    // player�������ײ�����
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.name == "BoundBottom")
        {
            myAudioSource.PlayOneShot(myAudioClip[4]);
            playerLife = 0;
            // ���޸ĵ�����ֵ����
            PlayerPrefs.SetInt("PlayerLife", playerLife);
            // ͬ��UI��ʾ
            myCanvas.lifeUpdate();
            isHurt = true;
            isAttact = true;
            myRigi.velocity = new Vector2(0f, 0f);
            myAnim.SetBool("Die", true);
            // ����playerĬ�ϵ�����ֵ
            PlayerPrefs.SetInt("PlayerLife", 5);
            // ����������л����ؿ�ѡ�����
            FadeInOut.instance.SceneFadeInOut("LevelSelect");
        }
    }

    // �ӳ�1.0s���ã�ʹ���˶���ֹͣ
    IEnumerator SetHurtFalse() // ��player������enemyʱ���򲥷����˶���������ͬʱ����Ϸ�����һ�ξ���󣬸�1.0s��ص�ldle״̬
    {
        yield return new WaitForSeconds(1.0f);
        isHurt = false; // ����״̬����
        myAnim.SetBool("Hurt", false); // �ص�ldle����

        yield return new WaitForSeconds(1.0f); // �������˶������ӳ�1s����canBeHurt����Ϊtrue������1s��player�޷��ٴ�������״̬  -- �޵�ʱ��
        canBeHurt = true;
        mySr.color = new Color(mySr.color.r, mySr.color.g, mySr.color.b, 1.0f); // �ָ���ɫ���޵�ʱ�������

    }
}
