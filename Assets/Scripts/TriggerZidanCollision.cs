using UnityEngine;

public class TriggerZidanCollision : MonoBehaviour
{
    public GameObject prefabToSpawn; // Ҫ���ɵ�Ԥ����

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("111");
        // �����ײ�������Ƿ���� "zidan" ��ǩ
        if (other.CompareTag("zidan"))
        {
            // �ڴ�������λ�ú���ת�Ƕ�����Ԥ����Ŀ�¡��
            Instantiate(prefabToSpawn, transform.position, transform.rotation);
        }
    }
}
