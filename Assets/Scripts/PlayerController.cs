using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Playables;
using Newtonsoft.Json.Linq;

public class PlayerController : MonoBehaviour
{
    public FixedJoystick joystick;            // 移动摇杆
    public FixedJoystick shootJoystick;       // 射击摇杆
    public GameObject bulletPrefab;            // 子弹预制体
    public float moveSpeed = 5f;
    public float bulletSpeed;           // 子弹速度
    public float fireRate;             // 射击间隔
    public float normalFireRate;

    // 狂暴模式相关
    private int baseBulletCount;            //清汤时发射的子弹数量
    private float rageFiringRate;           //狂暴射速
    private float rageThreshold;            //狂暴触发阈值
    private bool isRageActive = false;      // 狂暴模式标志
    private float rageDuration;             //狂暴持续时间
    private float rageTimer = 0.0f;         // 狂暴模式计时器

    [SerializeField]
    private int maxBullets;           // 最大子弹数量

    //JObject globalConfig = JsonLoader.LoadJsonAsJObject("StaticData/global_config");
    //private int remainingBullets;             // 剩余子弹数量

    
    private List<GameObject> activeBullets = new List<GameObject>(); // 添加子弹列表

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private float nextFireTime;               // 下次可发射时间

    public Text bulletCountText;

    public GameObject currentBullet;
    public Vector3 currentBulletPosition;

    private SceneType sceneType;
    private GameObject skillButton;

    public PlayableDirector director;//tl相关，策划加的
    private bool ispause=true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        skillButton = GameObject.Find("SkillButton");
        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateBulletCount();     
        currentBulletPosition = new Vector3(7f, 3.7f, 0.2f);    
        currentBullet = Instantiate(bulletPrefab, currentBulletPosition, Quaternion.identity);
        //currentBullet.SetActive(false);
        var bulletConfig = JsonLoader.LoadJsonAsJObject("StaticData/bullet_config");
        maxBullets = bulletConfig["maxBullets"].ToObject<int>();
        normalFireRate = bulletConfig["firingRate"].ToObject<float>();
        fireRate = normalFireRate;
        bulletSpeed = bulletConfig["bulletSpeed"].ToObject<float>();
        var rageConfig = JsonLoader.LoadJsonAsJObject("StaticData/rage_config");
        rageFiringRate = rageConfig["rageFiringRate"].ToObject<float>();
        rageThreshold = rageConfig["rageThreshold"].ToObject<float>();
        rageDuration = rageConfig["rageDuration"].ToObject<float>();
    }

    void FixedUpdate()
    {
        if(isRageActive)
        {
            rageTimer += Time.deltaTime;
            if(rageTimer >= rageDuration)
            {
                ExitRage();
            }
        }

        HandleMovement();
        HandleShooting();
        // 获取场景类型
        sceneType = skillButton.GetComponent<SkillButton>().currentSceneType;
        // TODO：有bug,先注释2
        // if (ispause)
        // {
        //     NewMountSmall();
        // }
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
        //CleanupBullets();
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
        if (GameManager.Instance.currentBulletCount > 0 && Time.time >= nextFireTime)
        {
            Vector2 shootDirection = new Vector2(horizontal, vertical).normalized;
            SpawnBullet(shootDirection);
            if(!isRageActive)
            {
                GameManager.Instance.currentBulletCount--;
            }
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
        
        if (sceneType == SceneType.QingTang)
        {
            baseBulletCount++;
            activeBullets.Add(bullet);
        }else
        {
            baseBulletCount = 0;
        }
        nextFireTime = Time.time + fireRate;
    }

    // 更新子弹数量
    private void UpdateBulletCount()
    {
        if (bulletCountText != null)
        {
            bulletCountText.text = GameManager.Instance.currentBulletCount.ToString();
        }
    }

    public void RecycleBullet()
    {
        int beforeRecycleBulletCount = GameManager.Instance.currentBulletCount;
        foreach (var bullet in activeBullets)
        {
            // 处理回收子弹的逻辑
            bullet.GetComponent<BulletController>().Recycle();
        }
        // 增加的子弹数量
        int addedBullets = GameManager.Instance.currentBulletCount - beforeRecycleBulletCount;
        Debug.Log("增加的子弹数量： " + addedBullets);
        Debug.Log("发射的子弹数量： " + baseBulletCount);
        if(addedBullets >= rageThreshold * baseBulletCount)
        {
            TriggerRage();
        }
        activeBullets.Clear();
    }

    // 清理已销毁的子弹
    private void CleanupBullets()
    {
        activeBullets.RemoveAll(bullet => bullet == null);
    }


    // 获取剩余子弹数量
    public int GetRemainingBullets()
    {
        return GameManager.Instance.currentBulletCount;
    }

    // 添加子弹
    public void AddBullets(int amount)
    {
        GameManager.Instance.currentBulletCount = Mathf.Min(GameManager.Instance.currentBulletCount + amount, maxBullets);
    }

    // 触发狂暴
    private void TriggerRage()
    {
        if (isRageActive)
        {
            Debug.Log("狂暴模式已激活，无法再次触发");
            return;
        }

        fireRate = rageFiringRate;
        isRageActive = true;
        rageTimer = 0.0f;
    }

    // 退出狂暴
    private void ExitRage()
    {
        fireRate = normalFireRate;
        isRageActive = false;
        rageTimer = 0.0f;
    }

    // TODO：控制子弹分裂次数

    // TODO：有报错，暂时注释掉1
    // //tl相关，策划加的
    // void NewMountSmall()
    // {
    //     if (remainingBullets <= 50 )
    //     {

    //         director.playableGraph.GetRootPlayable(0).SetSpeed(1);
    //         Debug.Log("时间轴恢复播放");
    //         ispause = false;
    //     }
    // }
}
