using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Rendering;

public class EnemyPumkinMan : MonoBehaviour
{
    bool isAlive; // �Ƿ���
    bool isIdle; // �Ƿ��ܴ���Idle״̬
    bool jumpAttact; // �Ƿ�����Ծ����״̬
    bool isJumpUp; // �Ƿ��ܴ�����Ծ�������׶Σ���Ծ������Ϊ�����֣�
    //bool isJumpDown; // �Ƿ��ܴ�����Ծ�½��׶�
    bool slideAttact; // ���ڻ�������״̬
    bool isHurt; // �Ƿ�������״̬
    bool canBeHurt; // �Ƿ��ܴ�������״̬

    public int life;
    public float attactDistance; // �յ������ľ���
    public float jumpHeight; // ��Ծ�ĸ߶�
    public float jumpUpSpeed; // ��Ծ�ٶ�
    public float jumpDownSpeed; // �����½��ٶ�
    public float slideSpeed; // �����ٶ�
    public float failDownSpeed; // ����������ʱ����ڿ��У�������ٶ�

    GameObject player;
    Animator myAnim;
    BoxCollider2D myCollider;
    SpriteRenderer mySr;
    AudioSource myAudioSource;

    Vector3 sliderTargetPosition; // slider��������λ��

    public void Awake()
    {
        isAlive = true;
        isIdle = true; // Ĭ�Ͽ�ʼ�Ķ�������Idle
        jumpAttact = false;
        isJumpUp = true;
        slideAttact = false;
        isHurt = false;
        canBeHurt = true; // Ĭ���ܹ���������״̬

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
                // ��player��enemyPumKinMan����С��2.0fʱ�����蹥��
                if (Vector3.Distance(player.transform.position, transform.position) <= attactDistance)
                {
                    // slideAttact ��ʼ�ڿ��л��蹥��
                    isIdle = false;
                    StartCoroutine("IdleToSlideAttact");
                    //slideAttact = true; // ���㻬������
                }
                else //���� 2.0fʱ��enemyPumKinMan��Ծ��playerͷ����ʼ��Ծ����
                {
                    // jumpAttact
                    isIdle = false;// ����״̬���ܴ���Idle
                    //jumpAttact = true; // ������Ծ����״̬
                    // ƽ���������ǵ��ܴ��ڹ���״̬ʱ���ϾͿ�ʼ�����������ӳ�1��
                    StartCoroutine("IdleToJumpAttact");

                }
            }
            else if (jumpAttact) // ������Ծ����״̬
            {
                lookAtPlayer();
                if (isJumpUp)
                {
                    // ��Ծ��Ŀ��λ�ã�playerͷ���Ϸ�3.5f��
                    Vector3 myTarget = new Vector3(player.transform.position.x, jumpHeight, transform.position.z);
                    transform.position = Vector3.MoveTowards(transform.position, myTarget, jumpUpSpeed * Time.deltaTime);
                    myAnim.SetBool("JumpUp", true);
                }
                else
                {
                    // ��Ծup״̬��ɣ�ִ��down״̬
                    myAnim.SetBool("JumpUp", false);
                    myAnim.SetBool("JumpDown", true);
                    // ��һ�׶�����ָ���߶�3.5f���½��׶���Ҫ�ƶ����ذ�-2.97f
                    Vector3 myTarget = new Vector3(transform.position.x, -2.97f, transform.position.z);
                    transform.position = Vector3.MoveTowards(transform.position, myTarget, jumpDownSpeed * Time.deltaTime);
                }

                if (transform.position.y == jumpHeight)
                {
                    isJumpUp = false; // �Ѿ���Ծ��ָ��λ�ã�һ�׶���Ծ����
                }
                else if (transform.position.y == -2.97f)
                {
                    jumpAttact = false; // �Ѿ��䵽���棬��Ծ�����׶ν���
                    StartCoroutine("JumpDownToIdle"); // �ӳ�0.5��ִ�У������ǽ�Ӳ����ؾ�ִ��Idle����
                }
            }
            else if (slideAttact)
            {
                myAnim.SetBool("Slide", true);
                transform.position = Vector3.MoveTowards(transform.position, sliderTargetPosition, slideSpeed * Time.deltaTime);
                // �жϻ����Ƿ����
                if (transform.position == sliderTargetPosition)
                {
                    myAnim.SetBool("Slide", false);
                    slideAttact = false; // ������������
                    isIdle = true;
                    // ������������ײ����Ϊԭ����
                    myCollider.offset = new Vector2(-0.191112f, -0.1454114f);
                    myCollider.size = new Vector2(1.099079f, 2.036471f);
                }
            }
            else if (isHurt)
            {
                // ��������״̬,�ƶ�����
                Vector3 myTargetPosition = new Vector3(transform.position.x, -2.975f, transform.position.z);
                transform.position = Vector3.MoveTowards(transform.position, myTargetPosition, failDownSpeed * Time.deltaTime);
            }
        }
        else
        {
            // ȷ��������λ���ڵذ���
            Vector3 myTargetPosition = new Vector3(transform.position.x, -2.975f, transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, myTargetPosition, failDownSpeed * Time.deltaTime);
        }

    }

    void lookAtPlayer() // ʹenemyPumKinManʼ�տ���player
    {
        if (player.transform.position.x > transform.position.x)
        {
            //  player��enemyPumkinMan���ұߣ������ұ�
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }
        else
        {
            transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        }
    }

    IEnumerator IdleToSlideAttact() // Idle״̬��slideAttact״̬�ӳ�ת��
    {
        yield return new WaitForSeconds(1);
        lookAtPlayer(); // ��������ʱ��֤enemy����player
        sliderTargetPosition = new Vector3(player.transform.position.x, transform.position.y, transform.position.z); // ֻ����player��x
        // ����������ײ��Ĵ�С
        myCollider.offset = new Vector2(-0.191112f, -0.4237702f);
        myCollider.size = new Vector2(1.099079f, 1.479754f);
        slideAttact = true;
    }

    IEnumerator JumpDownToIdle()
    {
        yield return new WaitForSeconds(0.5f);
        isIdle = true;
        isJumpUp = true; // ��ǰ��Ծ��������һ���ֿ���ִ����Ծ����
        // ֹͣ���е���Ծ����
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
        // ��ʼ������״̬�����ܲ�����������
        myAnim.SetBool("Hurt", false); // ��������״̬
        myAnim.SetBool("JumpUp", false);
        myAnim.SetBool("JumpDown", false);
        myAnim.SetBool("Slide", false);
        mySr.material.color = new Color(1.0f, 1.0f, 1.0f, 0.5f); // ��ʾ�����޵�ʱ���͸��
        isHurt = false;
        isIdle = true; // �л�ΪIdle״̬

        yield return new WaitForSeconds(2.0f); // ���˺��޵�ʱ��2s��2s������ٴ�����
        mySr.material.color = new Color(1.0f, 1.0f, 1.0f, 0.5f); // �޵�ʱ�������ǰ������1.0f��ʾ����ԭ������ɫ
        canBeHurt = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.tag == "PlayerAttact")
        {
            if (canBeHurt)
            {
                // ����������Ч
                myAudioSource.PlayOneShot(myAudioSource.clip);
                life--;
                if (life >= 1)
                {
                    // ����״̬����ֹ������ʱ���ֲ��������������쳣���
                    isIdle = false;
                    jumpAttact = false;
                    slideAttact = false;
                    StopCoroutine("JumpDownToIdle");
                    StopCoroutine("IdleToJumpAttact");
                    StopCoroutine("IdleToJumpAttact");

                    isHurt = true; // ��ǰ��������״̬
                    myAnim.SetBool("Hurt", true);
                    StartCoroutine("SetAnimHurtToFalse"); // �ӳٵ��ý�������״̬
                }
                else
                {
                    isAlive = false; // ִֹͣ�й����ƶ���һϵ���߼�
                    myCollider.enabled = false; // enemyPumkinMan�Ѿ���������������Collider
                    StopAllCoroutines(); // ֹͣ���е�coroutine
                    myAnim.SetBool("Die", true);

                    // �趨��3�룬�Զ�ת�����ؿ�ѡ�񳡾�
                    Time.timeScale = 0.5f; // ������ʱ�����һ��
                    StartCoroutine("AfterDie");
                }
                canBeHurt = false; // �Ѿ�������״̬, �����ڽ�������״̬

            }
        }
    }

    IEnumerator AfterDie()
    {
        yield return new WaitForSecondsRealtime(3.0f);
        FadeInOut.instance.SceneFadeInOut("LevelSelect");
    }

}
