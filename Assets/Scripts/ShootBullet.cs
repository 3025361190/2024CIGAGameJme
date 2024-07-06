using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // ����Unity UI�����ռ�

public class ShootBullet : MonoBehaviour
{
    [SerializeField]
    private GameObject bulletPrefab;
    public float bulletSpeed = 10f; // �ӵ��ٶ�
    public float currentTime = 0.1f; // ��Ļ���ʱ��
    // private Animator animator;

    private float invokeTime; // ��Ļ��ʱ

    [SerializeField]
    private List<GameObject> Bullets = new List<GameObject>();

    [SerializeField]
    private int bulletMount;
    public bool isEnraged;
    private SceneType sceneType;

    public Text bulletCountText; // UI Text ����

    private void Start()
    {
        invokeTime = currentTime;
        UpdateBulletCountUI(); // ��ʼ����һ��UI
        // animator = GetComponent<Animator>();
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

        if(Input.GetKeyDown(KeyCode.F)){
            RecycleBullet();
        }
    }

    private void UpdateBulletCountUI()
    {
        if (bulletCountText != null)
        {
            bulletCountText.text =bulletMount.ToString();
        }
    }

    private void Shoot()
    {
        if (!isEnraged) bulletMount--;

        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

        // if (sceneType == SceneType.QingTang)
        // {
            Bullets.Add(bullet);
        // }

        bullet.transform.position = transform.position;

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;

        Vector2 shootDirection = (mousePosition - transform.position).normalized;

        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();

        bulletRb.velocity = shootDirection * bulletSpeed;

        UpdateBulletCountUI(); // ����UI��ʾ
        // animator.SetTrigger("Switch");
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
        Debug.Log("����");
        foreach (var bullet in Bullets)
        {
            // ��������ӵ����߼�
            bullet.GetComponent<BulletController>().Recycle();
        }
        Bullets.Clear();
    }
}
