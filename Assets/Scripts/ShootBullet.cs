using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // ����Unity UI�����ռ�

public class ShootBullet : MonoBehaviour
{
    public ObjectPool bulletPool; // �����ʵ��
    public float bulletSpeed = 10f; // �ӵ��ٶ�
    public float currentTime = 0.1f; // ��Ļ���ʱ��

    private float invokeTime; // ��Ļ��ʱ

    private List<GameObject> Bullets = new List<GameObject>();

    private int bulletMount;
    public bool isEnraged;
    private SceneType sceneType;

    public Text bulletCountText; // UI Text ����

    private void Start()
    {
        invokeTime = currentTime;
        UpdateBulletCountUI(); // ��ʼ����һ��UI
    }

    private void Update()
    {
        if (Input.GetMouseButton(0) && (bulletMount > 0 || isEnraged))
        {
            invokeTime += Time.deltaTime;
            if (invokeTime - currentTime > 0)
            {
                invokeTime = 0;
                Shoot();
                UpdateBulletCountUI(); // ��������UI
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            invokeTime = currentTime;
        }
    }

    private void UpdateBulletCountUI()
    {
        if (bulletCountText != null)
        {
            bulletCountText.text = "Bullet Count: " + bulletMount.ToString();
        }
    }

    private void Shoot()
    {
        if (!isEnraged) bulletMount--;

        GameObject bullet = bulletPool.GetObjectFromPool(); // �Ӷ���ػ�ȡ�ӵ�

        if (sceneType == SceneType.QingTang)
        {
            Bullets.Add(bullet);
        }

        bullet.transform.position = transform.position;

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;

        Vector2 shootDirection = (mousePosition - transform.position).normalized;

        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();

        bulletRb.velocity = shootDirection * bulletSpeed;

        UpdateBulletCountUI(); // ����UI��ʾ
    }

    public void AddBulletMount()
    {
        bulletMount++;
        UpdateBulletCountUI(); // �����ӵ������UI��ʾ
    }

    public int GetBulletMount()
    {
        return bulletMount;
    }

    public void RecycleBullet()
    {
        foreach (var bullet in Bullets)
        {
            // ��������ӵ����߼�
        }
    }
}
