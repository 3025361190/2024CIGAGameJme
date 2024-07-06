using UnityEngine;

public class TriggerZidanCollision : MonoBehaviour
{
    public GameObject prefabToSpawn; // 要生成的预制体

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("111");
        // 检查碰撞的物体是否具有 "zidan" 标签
        if (other.CompareTag("zidan"))
        {
            // 在触发器的位置和旋转角度生成预制体的克隆体
            Instantiate(prefabToSpawn, transform.position, transform.rotation);
        }
    }
}
