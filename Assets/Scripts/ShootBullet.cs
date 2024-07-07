using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // 引入Unity UI命名空间

public class ShootBullet : MonoBehaviour
{
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
    private SceneType sceneType;

    public Text bulletCountText; // UI Text 对象


    public GameObject currentBullet;
    public GameObject effect;
    public GameObject huanEffect;
    private Animator animator;
    private Animator huan;

    public AudioSource audioSource;//子弹音效
    private void Start()
    {
        //音效
        audioSource = GetComponent<AudioSource>();

        invokeTime = currentTime;
        UpdateBulletCountUI(); // 初始更新一次UI
        currentBullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        currentBullet.SetActive(false);

        // animator = GetComponent<Animator>();
        animator = effect.GetComponent<Animator>();
        huan = huanEffect.GetComponent<Animator>();
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

        if(Input.GetKeyDown(KeyCode.F)){
            RecycleBullet();
        }

        // 增加getSceneType方法
        // ...
        // Debug.Log(GetBulletColor());
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
        if (!isEnraged) bulletMount--;

        GameObject bullet = currentBullet;
        currentBullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

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
        Debug.Log("回收");
        foreach (var bullet in Bullets)
        {
            // 处理回收子弹的逻辑
            bullet.GetComponent<BulletController>().Recycle();
        }
        Bullets.Clear();
    }
}
