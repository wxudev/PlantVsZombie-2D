using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    // 开始游戏：跳转到游戏主场景
    public void StartGame()
    {
        
        SceneManager.LoadScene("SampleScene");
    }

    // 退出游戏
    public void QuitGame()
    {
        // 编辑器模式下停止运行，打包后退出程序
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}