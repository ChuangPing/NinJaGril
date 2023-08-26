using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirPlatform : MonoBehaviour
{
    public Vector3 turnPoint; // 目标点
    public float moveSpeed;

    Vector3 targetPosition; // 移动种的目标点（随着移动变化）
    Vector3 originPosition; // 初始位置

    private void Awake()
    {
        originPosition = transform.position;
        Debug.Log("originPosition" + originPosition);
        Debug.Log("turnPoint" + originPosition);
    }
    // Update is called once per frame
    void Update()
    {
        if (transform.position == originPosition) // 回到初始位置
        {
            targetPosition = turnPoint;
        }
        else if (transform.position == turnPoint) // 移动到目标位置
        {
            targetPosition = originPosition; // 一个目标位置为初始位置
        }

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }
}
