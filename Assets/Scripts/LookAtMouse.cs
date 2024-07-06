using UnityEngine;

public class RotatePerpendicularToMouse : MonoBehaviour
{
    void Update()
    {
        // 获取鼠标位置
        Vector3 mousePos = Input.mousePosition;
        Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(mousePos);

        // 计算朝向向量
        Vector3 direction = (worldMousePos - transform.position).normalized;

        // 计算垂直朝向向量
        Vector3 perpendicularDirection = new Vector3(-direction.y, direction.x, 0);

        // 设置物体的朝向
        transform.up = perpendicularDirection;
    }
}
