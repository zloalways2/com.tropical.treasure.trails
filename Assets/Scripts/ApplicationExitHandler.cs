using UnityEngine;

public class ApplicationExitHandler : MonoBehaviour
{ 
    public void ExitToDesktop()
    {
#if UNITY_EDITOR
        
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
}