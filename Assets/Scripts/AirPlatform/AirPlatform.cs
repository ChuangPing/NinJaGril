using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirPlatform : MonoBehaviour
{
    public Vector3 turnPoint; // Ŀ���
    public float moveSpeed;

    Vector3 targetPosition; // �ƶ��ֵ�Ŀ��㣨�����ƶ��仯��
    Vector3 originPosition; // ��ʼλ��

    private void Awake()
    {
        originPosition = transform.position;
        Debug.Log("originPosition" + originPosition);
        Debug.Log("turnPoint" + originPosition);
    }
    // Update is called once per frame
    void Update()
    {
        if (transform.position == originPosition) // �ص���ʼλ��
        {
            targetPosition = turnPoint;
        }
        else if (transform.position == turnPoint) // �ƶ���Ŀ��λ��
        {
            targetPosition = originPosition; // һ��Ŀ��λ��Ϊ��ʼλ��
        }

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }
}
