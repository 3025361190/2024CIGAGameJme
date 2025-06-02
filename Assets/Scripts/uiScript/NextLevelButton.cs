using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NextLevelButton : MonoBehaviour
{
    private Button button;

    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(() =>
        {
            // 安全调用单例方法
            if (GameManager.Instance != null)
            {
                GameManager.Instance.NextLevel();
            }
            else
            {
                Debug.LogError("GameManager实例不存在！");
                // 备用方案：使用场景索引加载下一关
                int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
                if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
                {
                    SceneManager.LoadScene(nextSceneIndex);
                }
            }
        });
    }

    void OnDestroy()
    {
        if (button != null)
        {
            button.onClick.RemoveAllListeners();
        }
    }
}