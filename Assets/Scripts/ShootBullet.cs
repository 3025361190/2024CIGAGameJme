using UnityEngine;

public class ShootBullet : MonoBehaviour
{
    public ObjectPool bulletPool; // �����ʵ��
    public float bulletSpeed = 10f; // �ӵ��ٶ�
    public float currentTime = 0.1f; // ��Ļ���ʱ��

    private float invokeTime; // ��Ļ��ʱ


    public int bulletMount;
    public bool isEnraged;

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
    
    private void Shoot()
    {
        if(!isEnraged)  bulletMount--;

        GameObject bullet = bulletPool.GetObjectFromPool(); // �Ӷ���ػ�ȡ�ӵ�
        bullet.transform.position = transform.position;

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;

        Vector2 shootDirection = (mousePosition - transform.position).normalized;

        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();

        bulletRb.velocity = shootDirection * bulletSpeed;
    }
}
