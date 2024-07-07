using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // 引入Unity UI命名空间

public class ShootBullet : MonoBehaviour
{
    [SerializeField]
    private int thresholds;
    [SerializeField]
    private GameObject bulletPrefab;
    public float bulletSpeed = 10f; // 子弹速度
    public float currentTime = 0.1f; // 弹幕间隔时间
    // private Animator animator;

    private float invokeTime; // 弹幕计时

    [SerializeField]
    private List<GameObject> Bullets = new List<GameObject>();

    [SerializeField]
    private int bulletMount;
    public bool isEnraged;
    public float enragedDuration = 10f; // 狂暴状态持续时间为10秒钟
    public float currentEnragedTime = 0f; // 当前狂暴状态已经持续的时间
    private SceneType sceneType;

    public Text bulletCountText; // UI Text 对象

    //小途弄的以下，动画用
    public GameObject currentBullet;
    public Vector3 currentBulletPosition;
    public GameObject effect;
    public GameObject kuangbao;
    public GameObject huanEffect;
    public GameObject number;
    private Animator animator;
    private Animator huan;
    private Animator zidan;
    private GameObject sceneManager;
    private Animator bao;
    public AudioSource audioSource;//子弹音效
    private void Start()
    {
        currentBulletPosition = new Vector3(7f, 3.7f, 0.2f);
        //音效
        audioSource = GetComponent<AudioSource>();

        // 获取场景管理实例
        sceneManager = GameObject.Find("SceneManagerObject");
        invokeTime = currentTime;
        UpdateBulletCountUI(); // 初始更新一次UI
        currentBullet = Instantiate(bulletPrefab, currentBulletPosition, Quaternion.identity);
        //currentBullet.transform.localScale = new Vector3(2f, 2f, 1f);
        //currentBullet.transform.rotation = Quaternion.Euler(0, 0, 0);
        currentBullet.SetActive(false);

        // animator = GetComponent<Animator>();
        animator = effect.GetComponent<Animator>();
        huan = huanEffect.GetComponent<Animator>();
        zidan = number.GetComponent<Animator>();
        bao = kuangbao.GetComponent<Animator>();
        currentEnragedTime = enragedDuration;
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
                UpdateBulletCountUI(); // 射击后更新UI
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            invokeTime = currentTime;
        }


        //狂暴持續時間
        if (isEnraged)
        {
            bao.SetBool("kuangbao", true);
            currentEnragedTime -= Time.deltaTime;
            if (currentEnragedTime <= 0)
            {
                isEnraged = false;
                currentEnragedTime = enragedDuration;
            }
        }
        //if(Input.GetKeyDown(KeyCode.F)){
        //    RecycleBullet();
        //}

        // 获取场景类型
        sceneType = sceneManager.GetComponent<Manager>().currentSceneType;
    }

    private void UpdateBulletCountUI()
    {
        if (bulletCountText != null)
        {
            bulletCountText.text =bulletMount.ToString();
        }
    }


    public ColorType GetBulletColor(){
        return currentBullet.GetComponent<BulletController>().bulletCollor;
    }

    private void Shoot()
    {
        audioSource.Play();//音效
        animator.SetTrigger("kaihuo");
        zidan.SetTrigger("Switch");

        if (!isEnraged) bulletMount--;

        GameObject bullet = currentBullet;
        currentBullet = Instantiate(bulletPrefab, currentBulletPosition, Quaternion.identity);

        if (sceneType == SceneType.QingTang)
        {
            Bullets.Add(bullet);
        }

        bullet.transform.position = transform.position;
        bullet.GetComponent<BulletController>().trailRenderer.enabled = true;
        bullet.GetComponent<BulletController>().SetAcFlag();

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;

        Vector2 shootDirection = (mousePosition - transform.position).normalized;

        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();

        bulletRb.velocity = shootDirection * bulletSpeed;

        UpdateBulletCountUI(); // 更新UI显示
        // animator.SetTrigger("Switch");
    }

    public void AddBulletMount()
    {
        bulletMount++;
        UpdateBulletCountUI(); // 增加子弹后更新UI显示
       
    }

    public int GetBulletMount()
    {
        return bulletMount;
    }

    public void RecycleBullet()
    {
        huan.SetTrigger("huishou");
        int preBulletAmount = bulletMount;
        StartCoroutine(RecycleBulletsCoroutine(preBulletAmount));
    }

    private IEnumerator RecycleBulletsCoroutine(int preBulletAmount)
    {
        // 逐个回收子弹
        foreach (var bullet in Bullets)
        {
            bullet.GetComponent<BulletController>().Recycle();
            yield return new WaitForEndOfFrame(); // 等待当前帧结束
        }

        // 等待所有子弹回收完成
        yield return new WaitForSeconds(1f);

        // 所有子弹回收完成后执行后续逻辑
        int recycleBulletAmount = bulletMount - preBulletAmount;
        if (recycleBulletAmount >= thresholds)
        {
            Debug.Log("狂暴");
            isEnraged = true;
        }
        Debug.Log(preBulletAmount + " " + bulletMount + " " + recycleBulletAmount);
        Bullets.Clear();
    }

}
