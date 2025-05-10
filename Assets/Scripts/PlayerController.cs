using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public FixedJoystick joystick;            // 移动摇杆
    public FixedJoystick shootJoystick;       // 射击摇杆
    public GameObject bulletPrefab;            // 子弹预制体
    public float moveSpeed = 5f;
    public float bulletSpeed = 10f;           // 子弹速度
    public float fireRate = 0.2f;             // 射击间隔

    [SerializeField]
    private int maxBullets = 10000;           // 最大子弹数量
    private int remainingBullets;             // 剩余子弹数量

    private List<GameObject> activeBullets = new List<GameObject>(); // 添加子弹列表

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private float nextFireTime;               // 下次可发射时间

    public Text bulletCountText;

    public GameObject currentBullet;
    public Vector3 currentBulletPosition;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        remainingBullets = 10;    // 初始化子弹数量为10
        UpdateBulletCount();     
        currentBulletPosition = new Vector3(7f, 3.7f, 0.2f);    
        currentBullet = Instantiate(bulletPrefab, currentBulletPosition, Quaternion.identity);
        //currentBullet.SetActive(false);
    }

    void FixedUpdate()
    {
        HandleMovement();
        HandleShooting();
    }

    // 处理移动逻辑
    private void HandleMovement()
    {
        float horizontal = joystick.Horizontal;
        float vertical = joystick.Vertical;

        Vector2 movement = new Vector2(horizontal, vertical);
        rb.velocity = movement * moveSpeed;

        if (horizontal != 0)
        {
            spriteRenderer.flipX = horizontal < 0;
        }
    }

    // 处理射击逻辑
    private void HandleShooting()
    {
        float shootHorizontal = shootJoystick.Horizontal;
        float shootVertical = shootJoystick.Vertical;

        if (shootHorizontal != 0 || shootVertical != 0)
        {
            RotatePlayer(shootHorizontal, shootVertical);
            TryShoot(shootHorizontal, shootVertical);
        }

        // 清理已销毁的子弹
        CleanupBullets();
    }

    // 旋转角色
    private void RotatePlayer(float horizontal, float vertical)
    {
        float angle = Mathf.Atan2(vertical, horizontal) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
        spriteRenderer.flipY = Mathf.Abs(angle) > 90;
    }

    // 尝试发射子弹
    private void TryShoot(float horizontal, float vertical)
    {
        if (remainingBullets > 0 && Time.time >= nextFireTime)
        {
            Vector2 shootDirection = new Vector2(horizontal, vertical).normalized;
            SpawnBullet(shootDirection);
            remainingBullets--;
            UpdateBulletCount();
        }
    }

    // 生成子弹
    private void SpawnBullet(Vector2 direction)
    {
        float angle = transform.rotation.eulerAngles.z;
        float bulletAngle = angle + 90f;
        
        GameObject bullet = currentBullet;
        bullet.transform.position = transform.position;
        currentBullet = Instantiate(bulletPrefab, currentBulletPosition, Quaternion.Euler(0, 0, bulletAngle));
        bullet.GetComponent<BulletController>().trailRenderer.enabled = true;
        bullet.GetComponent<BulletController>().SetAcFlag();
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
        bulletRb.velocity = direction * bulletSpeed;
        
        activeBullets.Add(bullet);
        nextFireTime = Time.time + fireRate;
    }

    // 更新子弹数量
    private void UpdateBulletCount()
    {
        if (bulletCountText != null)
        {
            bulletCountText.text = remainingBullets.ToString();
        }
    }

    // 清理已销毁的子弹
    private void CleanupBullets()
    {
        activeBullets.RemoveAll(bullet => bullet == null);
    }


    // 获取剩余子弹数量
    public int GetRemainingBullets()
    {
        return remainingBullets;
    }

    // 添加子弹
    public void AddBullets(int amount)
    {
        remainingBullets = Mathf.Min(remainingBullets + amount, maxBullets);
    }
}
