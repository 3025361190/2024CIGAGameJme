using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // ����Unity UI�����ռ�

public class ShootBullet : MonoBehaviour
{
    [SerializeField]
    private int thresholds;
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
    public float enragedDuration = 10f; // ��״̬����ʱ��Ϊ10����
    public float currentEnragedTime = 0f; // ��ǰ��״̬�Ѿ�������ʱ��
    private SceneType sceneType;

    public Text bulletCountText; // UI Text ����

    //С;Ū�����£�������
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
    public AudioSource audioSource;//�ӵ���Ч
    private void Start()
    {
        currentBulletPosition = new Vector3(7f, 3.7f, 0.2f);
        //��Ч
        audioSource = GetComponent<AudioSource>();

        // ��ȡ��������ʵ��
        sceneManager = GameObject.Find("SceneManagerObject");
        invokeTime = currentTime;
        UpdateBulletCountUI(); // ��ʼ����һ��UI
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
                UpdateBulletCountUI(); // ��������UI
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            invokeTime = currentTime;
        }


        //�񱩳��m�r�g
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

        // ��ȡ��������
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
        audioSource.Play();//��Ч
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
        huan.SetTrigger("huishou");
        int preBulletAmount = bulletMount;
        StartCoroutine(RecycleBulletsCoroutine(preBulletAmount));
    }

    private IEnumerator RecycleBulletsCoroutine(int preBulletAmount)
    {
        // ��������ӵ�
        foreach (var bullet in Bullets)
        {
            bullet.GetComponent<BulletController>().Recycle();
            yield return new WaitForEndOfFrame(); // �ȴ���ǰ֡����
        }

        // �ȴ������ӵ��������
        yield return new WaitForSeconds(1f);

        // �����ӵ�������ɺ�ִ�к����߼�
        int recycleBulletAmount = bulletMount - preBulletAmount;
        if (recycleBulletAmount >= thresholds)
        {
            Debug.Log("��");
            isEnraged = true;
        }
        Debug.Log(preBulletAmount + " " + bulletMount + " " + recycleBulletAmount);
        Bullets.Clear();
    }

}
