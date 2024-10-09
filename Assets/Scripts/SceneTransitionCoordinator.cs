using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionCoordinator : MonoBehaviour
{
    public void LoadGameplayScene()
    {
        SceneManager.LoadScene(GlobalConfigManager.QUEST_ARENA);
    }
    public void LaunchStartupScreen()
    {
        SceneManager.LoadScene(GlobalConfigManager.BOOTUP_STAGE);
    }
    
}