using System.Collections.Generic;
using UnityEngine;

public class ShootBullet : MonoBehaviour
{
    public ObjectPool bulletPool; // 对象池实例
    public float bulletSpeed = 10f; // 子弹速度
    public float currentTime = 0.1f; // 弹幕间隔时间

    private float invokeTime; // 弹幕计时

    private List<GameObject> Bullets = new List<GameObject>();


    private int bulletMount;
    public bool isEnraged;

    private SceneType sceneType;

    private void Start()
    {
        invokeTime = currentTime;
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
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            invokeTime = currentTime;
        }
    }

    private void ChangeFrequency(float f){
        currentTime = f;
    }
    private void ChangeSpeed(float v){
        bulletSpeed = v;
    }
    public void AddBulletMount(){
        bulletMount++;
    }
    public int GetBulletMount(){
        return bulletMount;
    }

    public void RecycleBullet(){
        foreach (var bullet in Bullets)
        {
            // bullet
        }

    }
    
    private void Shoot()
    {
        if(!isEnraged)  bulletMount--;

        GameObject bullet = bulletPool.GetObjectFromPool(); // 从对象池获取子弹

        if(sceneType == SceneType.QingTang){
            Bullets.Add(bullet);
        }

        bullet.transform.position = transform.position;

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;

        Vector2 shootDirection = (mousePosition - transform.position).normalized;

        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();

        bulletRb.velocity = shootDirection * bulletSpeed;
    }
}
