using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public FixedJoystick joystick;            // 移动摇杆
    public FixedJoystick shootJoystick;       // 射击摇杆
    public GameObject bulletPrefab;            // 子弹预制体
    public float moveSpeed = 5f;
    public float bulletSpeed = 10f;           // 子弹速度
    public float fireRate = 0.2f;             // 射击间隔

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private float nextFireTime;               // 下次可发射时间

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        // 移动逻辑
        float horizontal = joystick.Horizontal;
        float vertical = joystick.Vertical;

        Vector2 movement = new Vector2(horizontal, vertical);
        rb.velocity = movement * moveSpeed;

        if (horizontal != 0)
        {
            spriteRenderer.flipX = horizontal < 0;
        }

        // 射击逻辑
        float shootHorizontal = shootJoystick.Horizontal;
        float shootVertical = shootJoystick.Vertical;

        // 当射击摇杆被触摸且达到发射间隔时
        if ((shootHorizontal != 0 || shootVertical != 0) && Time.time >= nextFireTime)
        {
            // 计算射击方向
            Vector2 shootDirection = new Vector2(shootHorizontal, shootVertical).normalized;
            
            // 创建子弹
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
            
            // 设置子弹速度和方向
            bulletRb.velocity = shootDirection * bulletSpeed;

            // 更新下次发射时间
            nextFireTime = Time.time + fireRate;
        }
    }
}
