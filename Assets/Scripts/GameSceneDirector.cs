using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class GameSceneDirector : MonoBehaviour
{
    [FormerlySerializedAs("_levelDisplayText")] [SerializeField] private TextMeshProUGUI[] _levelInfoText;
    [SerializeField] private MemoryGame _memoryGame;
    private int _currentActiveStage;
        
    private void Start()
    {
        _currentActiveStage = PlayerPrefs.GetInt(GlobalConfigManager.CURRENT_ACTIVE_STAGE, 0);
            
        for (int stepCounter = 0; stepCounter < _levelInfoText.Length; stepCounter++)
        {
            _levelInfoText[stepCounter].text = $"Level {_currentActiveStage+1} DONE";
        }
    }

    public void FinalizeGameCompletion()
    {
        PlayerPrefs.SetInt(GlobalConfigManager.CURRENT_ACTIVE_STAGE, _currentActiveStage+1);
        PlayerPrefs.Save();
        StageProgressManager levelFlowManager = FindObjectOfType<StageProgressManager>();
        levelFlowManager.AchieveLevelCompletion(_currentActiveStage, _memoryGame.RetrieveScore());
        PlayerPrefs.SetInt("LevelScore" + _currentActiveStage,  _memoryGame.RetrieveScore());
        PlayerPrefs.Save();
    }
        
    public void ActivateTimeFlow()
    {
        Time.timeScale = 1f;
    }
    
    public void PauseTimeFlow()
    {
        Time.timeScale = 0f;
    }
}