using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MoveTrailsRandomly : MonoBehaviour
{
    public float moveSpeed = 10f; // �ƶ��ٶ�
    public float minWaitTime = 0.005f; // ��С�ȴ�ʱ��
    public float maxWaitTime = 0.01f; // ���ȴ�ʱ��

    void Start()
    {
        // ��ȡ������������β��Ч��TrailRenderer���
        TrailRenderer[] allTrails = UnityEngine.Object.FindObjectsOfType<TrailRenderer>();

        // ʹ��Э��ͬʱ����ͬ�ĵȴ�ʱ���ƶ�ÿ����β��Ч
        StartCoroutine(MoveTrailsRandomlyCoroutine(allTrails));
    }

    IEnumerator MoveTrailsRandomlyCoroutine(TrailRenderer[] allTrails)
    {
        foreach (TrailRenderer trail in allTrails)
        {
            // ����ȴ�һ��ʱ���ٿ�ʼ�ƶ�
            float waitTime = Random.Range(minWaitTime, maxWaitTime);
            yield return new WaitForSeconds(waitTime);

            // ����β��Ч������Ļ���ĵ�
            Vector3 screenCenter = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2f, Screen.height / 2f, Camera.main.nearClipPlane));
            Vector3 direction = (screenCenter - trail.transform.position).normalized;
            trail.transform.up = direction; // ����β��Ч���Ϸ�����Ϊ�������ĵ�ķ���

            // ͬʱ�����ƶ�Э�̣����ȴ��ƶ����
            StartCoroutine(MoveTrailToCenter(trail, screenCenter));
        }
    }

    IEnumerator MoveTrailToCenter(TrailRenderer trail, Vector3 targetPosition)
    {
        while (true)
        {
            // ������β��Ч��ǰλ�ú�Ŀ��λ�õķ�������
            Vector3 direction = (targetPosition - trail.transform.position).normalized;

            // ������β��Ч�ƶ���λ����
            Vector3 moveAmount = direction * moveSpeed * Time.deltaTime;

            // �ƶ���β��Ч��λ��
            trail.transform.position += moveAmount;

            // ��ĳ���������˳�ѭ�������絽��Ŀ��λ��
            if (Vector3.Distance(trail.transform.position, targetPosition) < 0.01f)
            {
                break;
            }

            // ��ͣһ֡
            yield return null;
        }
    }
}
