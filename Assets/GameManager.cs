using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public GameObject failPanel;
    public AudioClip failAudio;
    public ZombieSpawner zombieSpawner;
    private AudioSource audioSource;
    private bool isGameOver = false;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void GameOver()
    {
        if (isGameOver) return;
        isGameOver = true;
        // 显示失败界面
        failPanel.SetActive(true);
        // 暂停游戏
        Time.timeScale = 0;
        // 停止僵尸生成器的背景音乐bgm4
        if (zombieSpawner != null && zombieSpawner.bgm4 != null)
        {
            zombieSpawner.bgm4.Stop();
        }
        // 播放失败音乐
        audioSource.PlayOneShot(failAudio);
    }
    // 重新开始按钮调用
    public void RestartLevel()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    // 返回主菜单按钮调用
    public void BackToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Main");
    }
}