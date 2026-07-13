using UnityEngine;

public class FailTrigger : MonoBehaviour
{
    
    public GameManager gameManager;

    // 有碰撞体进入触发区时自动调用
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 只响应标签为Zombie的僵尸
        if (other.CompareTag("Zombie"))
        {
            gameManager.GameOver();
        }
    }
}