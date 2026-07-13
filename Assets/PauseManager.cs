using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    // 把暂停面板拖到这个变量上
    public GameObject pausePanel;

    void Update()
    {
        // 按ESC键也可以快速呼出/关闭暂停菜单
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    // 切换暂停状态：点暂停按钮调用这个
    public void TogglePause()
    {
        // 如果面板没显示 → 打开暂停
        if (!pausePanel.activeSelf)
        {
            pausePanel.SetActive(true);
            Time.timeScale = 0; // 游戏时间暂停，所有运动、物理都会停
        }
        // 如果面板已经显示 → 关闭暂停
        else
        {
            ResumeGame();
        }
    }

    // 返回游戏
    public void ResumeGame()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1; // 恢复游戏时间流速
    }

    // 重新开始当前关卡
    public void RestartLevel()
    {
        Time.timeScale = 1; // 必须先重置时间，否则重载后还是暂停状态
        // 重新加载当前场景
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // 返回主菜单
    public void BackToMainMenu()
    {
        Time.timeScale = 1; // 重置时间
        SceneManager.LoadScene("Main"); 
    }
}