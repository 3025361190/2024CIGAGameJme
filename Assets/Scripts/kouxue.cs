using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static huanxue;

public class kouxue : MonoBehaviour
{
    private huanxue healthBar;
    public GameObject quanquan; // �������������Ա���Unity Inspector�н��а�

    void Start()
    {
        healthBar = GetComponent<huanxue>(); // ��ȡ���ӵ�ͬһ�����ϵ�HealthBar���
    }

    void Update()
    {
        // �� Update �����и��� HealthBar��ģ��ʱ������
      //  healthBar.Update(Time.deltaTime);
    }

    public void ButtonClickTakeDamage()
    {
        // ����ť�����ʱ���۳�һ��Ѫ��
        healthBar.TakeDamage(1);
    }
}

