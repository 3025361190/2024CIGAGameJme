/*
文件名：PlayerController.cs
编辑人：没道理啊
文件描述：玩家控制脚本,挂载在玩家预制体上,用于控制玩家移动和射击
组件依赖：Rigidbody2D, SpriteRenderer, Collider2D, TrailRenderer,?????
rigidbody2d组件需要设置为Kinematic
*/
#define ENABLE_KEYBOARD_CONTROL  // 注释这行可以禁用所有键盘控制
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Playables;
using Newtonsoft.Json.Linq;
using System;

public class PlayerController : MonoBehaviour
{
    public FixedJoystick joystick;              // 移动摇杆
    // private Vector2 moveDirection;
    public FixedJoystick shootJoystick;         // 射击摇杆
    // private Vector2 shootDirection;
    public GameObject bulletPrefab;             // 子弹预制体
    public float moveSpeed;                     // 移动速度
    private float playerScaleX;               // 玩家缩放比例X
    private float playerScaleY;               // 玩家缩放比例Y
    private float playerScaleZ;               // 玩家缩放比例Z
    private float playerColliderRadius;       // 玩家碰撞器半径
    public float bulletSpeed;                   // 子弹速度
    public float fireRate;                      // 射击间隔
    public float normalFireRate;                // 正常射击间隔
    public int bulletCountInScreen = 0;         //屏幕中存在的子弹数量
    

    // 狂暴模式相关
    private int baseBulletCount;            //清汤时发射的子弹数量
    private float rageFiringRate;           //狂暴射速
    private float rageThreshold;            //狂暴触发阈值
    private int rageMinValue;            //狂暴模式最小回收的子弹数量
    private bool isRageActive = false;      // 狂暴模式标志
    private float rageDuration;             //狂暴持续时间
    private float rageTimer = 0.0f;         // 狂暴模式计时器
    private bool tryActivateRage = false; // 尝试激活狂暴模式？
    private float rageActivateTime;      // 狂暴激活时间
    private float rageActivateTimer = 0.0f; // 狂暴激活计时器
    public Slider progressBar;              // 进度条
    private float maxValue;
    private float currentValue;
    private float minValue;

    [SerializeField]
    private int maxBullets;           // 最大子弹数量

    //JObject globalConfig = JsonLoader.LoadJsonAsJObject("StaticData/global_config");
    //private int remainingBullets;             // 剩余子弹数量
    
    // 触摸的起始位置
    private Dictionary<int, bool> touchStartedOnJoystick = new Dictionary<int, bool>();

    
    private List<GameObject> activeBullets = new List<GameObject>(); // 添加子弹列表

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private float nextFireTime;               // 下次可发射时间

    public Text bulletCountText;

    public GameObject currentBullet;
    public Vector3 currentBulletPosition;

    private SceneType sceneType;
    private GameObject skillButton;
    private GameObject pauseButton;

    public PlayableDirector director;//tl相关，策划加的
    private bool ispause=true;

    //策划加的
    public GameObject KuangbaoEffect;
    public Animator KaihuoEffect;
    public Animator ScoreAnim;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        skillButton = GameObject.Find("SkillButton");
        pauseButton = GameObject.Find("Pause");
        // bulletCountText = GameObject.Find("BulletCountText").GetComponent<Text>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        // UpdateBulletCount();     
        currentBulletPosition = new Vector3(7f, 3.7f, 1.0f);    
        currentBullet = Instantiate(bulletPrefab, currentBulletPosition, Quaternion.identity);
        currentBullet.GetComponent<Collider2D>().enabled = false;
        //currentBullet.SetActive(false);
        //var bulletConfig = JsonLoader.LoadJsonAsJObject("StaticData/bullet_config");
        var bulletConfig = GameManager.Instance.data.bulletData;
        maxBullets = bulletConfig["maxBullets"].ToObject<int>();
        normalFireRate = bulletConfig["firingRate"].ToObject<float>();
        fireRate = normalFireRate;
        bulletSpeed = bulletConfig["bulletSpeed"].ToObject<float>();
        //var rageConfig = JsonLoader.LoadJsonAsJObject("StaticData/rage_config");
        var rageConfig = GameManager.Instance.data.rageData;
        rageFiringRate = rageConfig["rageFiringRate"].ToObject<float>();
        rageThreshold = rageConfig["rageThreshold"].ToObject<float>();
        rageMinValue = rageConfig["rageMinValue"].ToObject<int>();
        rageDuration = rageConfig["rageDuration"].ToObject<float>();
        rageActivateTime = rageConfig["rageActivateTime"].ToObject<float>();
        //var globalConfig = JsonLoader.LoadJsonAsJObject("StaticData/global_config");
        var globalConfig = GameManager.Instance.data.globalData;
        moveSpeed = globalConfig["maxMoveSpeed"].ToObject<float>();
        playerScaleX = globalConfig["playerScaleX"].ToObject<float>();
        playerScaleY = globalConfig["playerScaleY"].ToObject<float>();
        playerScaleZ = globalConfig["playerScaleZ"].ToObject<float>();
        playerColliderRadius = globalConfig["playerColliderRadius"].ToObject<float>();

        transform.localScale = new Vector3(playerScaleX, playerScaleY, playerScaleZ);
        GetComponent<CircleCollider2D>().radius = playerColliderRadius;
    }


    void FixedUpdate()
    {

        // moveDirection = new Vector2(joystick.Horizontal, joystick.Vertical);
        // shootDirection = new Vector2(shootJoystick.Horizontal, shootJoystick.Vertical);

        // if (shootDirection != Vector2.zero)
        // {
        //     //RotatePlayer(shootDirection.x, shootDirection.y);
        // }else if(moveDirection != Vector2.zero)
        // {
        //     //RotatePlayer(moveDirection.x, moveDirection.y);
        // }
        
        if(isRageActive)
        {
            rageTimer += Time.deltaTime;
            currentValue -= Time.deltaTime;
            currentValue = Mathf.Clamp(currentValue, 0, maxValue);
            progressBar.value = currentValue / maxValue;
            if(currentValue <= 0.0f)
            {
                ExitRage();
            }
        }
        if(tryActivateRage)
        {
            rageActivateTimer += Time.deltaTime;
            progressBar.value = (currentValue - minValue) / maxValue;
            if (progressBar.value >= 1.0f && currentValue - minValue >= rageMinValue && !isRageActive)
            {
                TriggerRage();
            }
            if(rageActivateTimer >= rageActivateTime)
            {
                if(!isRageActive)
                {
                    progressBar.gameObject.SetActive(false);
                }
                tryActivateRage = false;
                rageActivateTimer = 0.0f;
            }
        }

        HandleMovement();
        HandleShooting();
        // 获取场景类型
        sceneType = skillButton.GetComponent<SkillButton>().currentSceneType;
        // 新手教程相关（1）
        if (ispause)
        {
            NewMountSmall();
        }
    }

    private bool IsPointerOverSomething(RectTransform rectTransform, Vector2 position)
    {
        if (rectTransform == null) return false;
        
        return RectTransformUtility.RectangleContainsScreenPoint(
            rectTransform,
            position,
            null
        );
    }

    // 处理移动逻辑
    private void HandleMovement()
    {
        // 获取摇杆输入
        float horizontal = joystick.Horizontal;
        float vertical = joystick.Vertical;

#if ENABLE_KEYBOARD_CONTROL
        // 添加WASD键盘输入（测试用）
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            horizontal = -1;
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            horizontal = 1;
        }

        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            vertical = -1;
        }
        else if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            vertical = 1;
        }

        // 如果同时有键盘和摇杆输入，优先使用摇杆
        if (joystick.Horizontal != 0 || joystick.Vertical != 0)
        {
            horizontal = joystick.Horizontal;
            vertical = joystick.Vertical;
        }
#endif
        float fixedHorizontal = horizontal;
        float fixedVertical = vertical;

        // 设置x边界
        if (transform.position.x <= -8.2f)
        {
            fixedHorizontal = Mathf.Max(horizontal, 0);
        }
        else if (transform.position.x >= 8.3f)
        {
            fixedHorizontal = Mathf.Min(horizontal, 0);
        }
        // 设置y边界
        if (transform.position.y <= -4.3f)
        {
            fixedVertical = Mathf.Max(vertical, 0);
        }
        else if (transform.position.y >= 4.3f)
        {
            fixedVertical = Mathf.Min(vertical, 0);
        }

        Vector2 movement = new(fixedHorizontal, fixedVertical);
        rb.velocity = movement * moveSpeed;
        if (horizontal != 0 || vertical != 0)
        {
            RotatePlayer(horizontal, vertical);
        }

        // if (horizontal != 0)
        // {
        //     spriteRenderer.flipX = horizontal < 0;
        // }
    }

    // 处理射击逻辑
    private void HandleShooting()
    {
        float shootHorizontal = 0;
        float shootVertical = 0;
        if(!GameManager.Instance.GetControllMode())
        {
            // 摇杆控制模式
            if (shootJoystick.Horizontal != 0 || shootJoystick.Vertical != 0)
            {
                shootHorizontal = shootJoystick.Horizontal;
                shootVertical = shootJoystick.Vertical;
            }
        }
        else
        {
            // 触摸控制模式
            if (Input.touchCount > 0)
            {
                for (int i = 0; i < Input.touchCount; i++)
                {
                    Touch touch = Input.GetTouch(i);
                    
                    if (touch.phase == TouchPhase.Began)
                    {
                        // 检查是否点击在UI元素上
                        bool isOnUI = false;
                        if (joystick != null && joystick.gameObject.activeInHierarchy)
                        {
                            isOnUI |= IsPointerOverSomething(joystick.GetComponent<RectTransform>(), touch.position);
                        }
                        if (skillButton != null && skillButton.activeInHierarchy)
                        {
                            isOnUI |= IsPointerOverSomething(skillButton.GetComponent<RectTransform>(), touch.position);
                        }
                        if (pauseButton != null && pauseButton.activeInHierarchy)
                        {
                            isOnUI |= IsPointerOverSomething(pauseButton.GetComponent<RectTransform>(), touch.position);
                        }
                        touchStartedOnJoystick[touch.fingerId] = isOnUI;
                    }
                    
                    // 如果这个触摸没有开始于UI元素上，就处理射击
                    if (!touchStartedOnJoystick.ContainsKey(touch.fingerId) || !touchStartedOnJoystick[touch.fingerId])
                    {
                        Vector2 touchPos = Camera.main.ScreenToWorldPoint(touch.position);
                        shootHorizontal = touchPos.x - transform.position.x;
                        shootVertical = touchPos.y - transform.position.y;
                        
                        if (touch.phase == TouchPhase.Began)
                        {
                            TryShoot(shootHorizontal, shootVertical);
                        }
                    }
                    
                    if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                    {
                        touchStartedOnJoystick.Remove(touch.fingerId);
                    }
                }
            }
        }

        // 持续射击（来自射击摇杆或触摸）
        if (shootHorizontal != 0 || shootVertical != 0)
        {
            RotatePlayer(shootHorizontal, shootVertical);
            TryShoot(shootHorizontal, shootVertical);
        }
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
            AudioManager.Instance.PlaySFX(0);
            KaihuoEffect.SetTrigger("attack");
            Vector2 shootDirection = new Vector2(horizontal, vertical).normalized;
            SpawnBullet(shootDirection);
            if(!isRageActive)
            {
                ScoreAnim.SetTrigger("Switch");
                GameManager.Instance.currentBulletCount--;
            }
            // 将方向向量顺时针旋转10度
            if(BuffManager.Instance.GetBuffStack(21) > 0)
            {
                if(GameManager.Instance.currentBulletCount > 1 && sceneType == SceneType.QingTang)
                {
                    float angleInRadians = -10f * Mathf.Deg2Rad; // 负号表示顺时针旋转
                    Vector2 rotatedDirection = new Vector2(
                        shootDirection.x * Mathf.Cos(angleInRadians) - shootDirection.y * Mathf.Sin(angleInRadians),
                        shootDirection.x * Mathf.Sin(angleInRadians) + shootDirection.y * Mathf.Cos(angleInRadians)
                    );
                    SpawnBullet(rotatedDirection);
                    angleInRadians = 10f * Mathf.Deg2Rad;
                    rotatedDirection = new Vector2(
                        shootDirection.x * Mathf.Cos(angleInRadians) - shootDirection.y * Mathf.Sin(angleInRadians),
                        shootDirection.x * Mathf.Sin(angleInRadians) + shootDirection.y * Mathf.Cos(angleInRadians)
                    );
                    SpawnBullet(rotatedDirection);
                    if(!isRageActive)
                    {
                        ScoreAnim.SetTrigger("Switch");
                        GameManager.Instance.currentBulletCount -= 2;
                    }
                }
                if(GameManager.Instance.currentBulletCount > 0 && sceneType == SceneType.QingTang)
                {
                    Vector2 oppositeDirection = -shootDirection; // 相反方向
                    SpawnBullet(oppositeDirection);
                    if(!isRageActive)
                    {
                        ScoreAnim.SetTrigger("Switch");
                        GameManager.Instance.currentBulletCount--;
                    }
                }
            }
            
            


            // UpdateBulletCount();
        }
    }
    

    // 生成子弹
    private void SpawnBullet(Vector2 direction)
    {
        float angle = transform.rotation.eulerAngles.z;
        float bulletAngle = angle + 90f;
        
        GameObject bullet = currentBullet;
        bullet.GetComponent<Collider2D>().enabled = true;
        bullet.transform.position = transform.position;
        currentBullet = Instantiate(bulletPrefab, currentBulletPosition, Quaternion.Euler(0, 0, bulletAngle));
        currentBullet.GetComponent<Collider2D>().enabled = false;
        bullet.GetComponent<BulletController>().trailRenderer.enabled = true;
        bullet.GetComponent<BulletController>().SetAcFlag();
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
        bulletRb.velocity = direction * bulletSpeed;
        
        if (sceneType == SceneType.QingTang)
        {
            baseBulletCount++;
            bulletCountInScreen++;
            activeBullets.Add(bullet);
        }else
        {
            baseBulletCount = 0;
        }
        nextFireTime = Time.time + fireRate;
    }

    // 暂时用不到，子弹文本显示由BulletCountUI脚本控制，其中在update中主动调用GetRemainingBullets()获取了
    // 更新子弹数量显示
    // private void UpdateBulletCount()
    // {
    //     if (bulletCountText != null)
    //     {
    //         bulletCountText.text = GameManager.Instance.currentBulletCount.ToString();
    //     }
    // }

    public void RecycleBullet()
    {
        foreach (var bullet in activeBullets)
        {
            // 处理回收子弹的逻辑
            if(bullet != null)
            {
                bullet.GetComponent<BulletController>().Recycle();
            }
            else
            {
                Debug.Log("Bullet is null, removing from activeBullets list.");
            }
        }
        tryActivateRage = true;
        minValue = GameManager.Instance.currentBulletCount;
        currentValue = GameManager.Instance.currentBulletCount;
        maxValue = Math.Max(baseBulletCount * rageThreshold, 10);
        progressBar.value = 0.0f;
        progressBar.gameObject.SetActive(true);
        activeBullets.Clear();
        bulletCountInScreen = 0;
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
        if(tryActivateRage)
        {
            currentValue += amount;
        }
        GameManager.Instance.currentBulletCount = Mathf.Min(GameManager.Instance.currentBulletCount + amount, maxBullets);
    }

    // 触发狂暴
    private void TriggerRage()
    {
        //音频
        AudioManager.Instance.PlayBGM(1);
        KuangbaoEffect.SetActive(true);
        if (isRageActive)
        {
            return;
        }

        maxValue = rageDuration;
        minValue = 0.0f;
        currentValue = maxValue;
        fireRate = rageFiringRate;
        isRageActive = true;
        rageTimer = 0.0f;
    }

    // 退出狂暴
    private void ExitRage()
    {
        AudioManager.Instance.PlayBGM(0);
        KuangbaoEffect.SetActive(false);
        progressBar.gameObject.SetActive(false);
        fireRate = normalFireRate;
        isRageActive = false;
        rageTimer = 0.0f;
    }

    // 控制子弹分裂次数（不写了.................）
    
     // 新手教程相关（2）
     void NewMountSmall()
     {
         if (GameManager.Instance.currentBulletCount <= 50 )
         {
    
             director.playableGraph.GetRootPlayable(0).SetSpeed(1);
             ispause = false;
         }
     }
}