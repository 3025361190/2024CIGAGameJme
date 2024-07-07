using UnityEngine;
using UnityEngine.UI;

public class BulletCountUI : MonoBehaviour
{
    public Text bulletCountText;  // ���� UI Text ���
    public GameObject targetGameObject;  // ���ð��� ShootBullet �����Ŀ�� GameObject
    private ShootBullet shootBullet;  // ���� ShootBullet �ű�

    void Start()
    {
        // ��ȡĿ�� GameObject �ϵ� ShootBullet ���
        if (targetGameObject != null)
        {
            shootBullet = targetGameObject.GetComponent<ShootBullet>();
        }
        else
        {
            Debug.LogError("Target GameObject not assigned!");
        }

        // ��ȡ UI Text ���
        bulletCountText = GetComponent<Text>();
    }

    void Update()
    {
        // ÿ֡������ʾ���ӵ�����
        if (shootBullet != null && bulletCountText != null)
        {
            // �� ShootBullet �ű��� GetBulletMount ������ȡ�ӵ�����
            int bulletCount = shootBullet.GetBulletMount();

            // ���� UI Text ��ʾ���ı�����
            bulletCountText.text = bulletCount.ToString();
        }
    }
}
