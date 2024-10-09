using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SingleSceneTransition : MonoBehaviour
{ 
    private void Start()
    {
        StartCoroutine(LoadPrimaryScene(GlobalConfigManager.MAIN_SYSTEM_PLATFORM));
    }

    private IEnumerator LoadPrimaryScene(string nameScene)
    {
        AsyncOperation backgroundTask = SceneManager.LoadSceneAsync(nameScene);

        backgroundTask.allowSceneActivation = false;

        while (!backgroundTask.isDone)
        {
            if (backgroundTask.progress >= 0.9f)
            {
                backgroundTask.allowSceneActivation = true;
            }

            yield return null;
        }
    }

        
}