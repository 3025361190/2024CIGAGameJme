using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MoveTrailsRandomly : MonoBehaviour
{
    public float moveSpeed = 10f; // 移动速度
    public float minWaitTime = 0.005f; // 最小等待时间
    public float maxWaitTime = 0.01f; // 最大等待时间

    void Start()
    {
        // 获取场景中所有拖尾特效的TrailRenderer组件
        TrailRenderer[] allTrails = UnityEngine.Object.FindObjectsOfType<TrailRenderer>();

        // 使用协程同时按不同的等待时间移动每个拖尾特效
        StartCoroutine(MoveTrailsRandomlyCoroutine(allTrails));
    }

    IEnumerator MoveTrailsRandomlyCoroutine(TrailRenderer[] allTrails)
    {
        foreach (TrailRenderer trail in allTrails)
        {
            // 随机等待一段时间再开始移动
            float waitTime = Random.Range(minWaitTime, maxWaitTime);
            yield return new WaitForSeconds(waitTime);

            // 让拖尾特效朝向屏幕中心点
            Vector3 screenCenter = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2f, Screen.height / 2f, Camera.main.nearClipPlane));
            Vector3 direction = (screenCenter - trail.transform.position).normalized;
            trail.transform.up = direction; // 将拖尾特效的上方向设为朝向中心点的方向

            // 同时启动移动协程，并等待移动完成
            StartCoroutine(MoveTrailToCenter(trail, screenCenter));
        }
    }

    IEnumerator MoveTrailToCenter(TrailRenderer trail, Vector3 targetPosition)
    {
        while (true)
        {
            // 计算拖尾特效当前位置和目标位置的方向向量
            Vector3 direction = (targetPosition - trail.transform.position).normalized;

            // 计算拖尾特效移动的位移量
            Vector3 moveAmount = direction * moveSpeed * Time.deltaTime;

            // 移动拖尾特效的位置
            trail.transform.position += moveAmount;

            // 在某个条件下退出循环，例如到达目标位置
            if (Vector3.Distance(trail.transform.position, targetPosition) < 0.01f)
            {
                break;
            }

            // 暂停一帧
            yield return null;
        }
    }
}
