using UnityEngine;

public class RotatePerpendicularToMouse : MonoBehaviour
{
    void Update()
    {
        // ��ȡ���λ��
        Vector3 mousePos = Input.mousePosition;
        Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(mousePos);

        // ���㳯������
        Vector3 direction = (worldMousePos - transform.position).normalized;

        // ���㴹ֱ��������
        Vector3 perpendicularDirection = new Vector3(-direction.y, direction.x, 0);

        // ��������ĳ���
        transform.up = perpendicularDirection;
    }
}
