using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class StageProgressManager : MonoBehaviour
{
    public static StageProgressManager appGlobalInstance;

    [FormerlySerializedAs("_stageButtons")] [SerializeField] private GameObject[] levelOptionButtons;
    [SerializeField] private LevelData[] _levels;
    [FormerlySerializedAs("_selectButton")] [SerializeField] private Button chooseButton;
    [FormerlySerializedAs("_completedStarSprite")] [SerializeField] private Sprite earnedStarIcon;
    [SerializeField] private Sprite _emptyStarSprite;

    private int _totalStagesCount = 15;
    private int _chosenLevelIndex;

    void Start()
    {
        if (appGlobalInstance == null)
        {
            appGlobalInstance = this;
            DontDestroyOnLoad(gameObject);
        }

        InitializeGameStages();
        UpdateStageButtons();
        chooseButton.interactable = false;
    }

    private void InitializeGameStages()
    {
        for (int loopStepCounter = 0; loopStepCounter < _totalStagesCount; loopStepCounter++)
        {
            
            if (PlayerPrefs.GetInt(GlobalConfigManager.ADVANCEMENT_LEVEL + loopStepCounter, -1) == -1)
            {
                PlayerPrefs.SetInt(GlobalConfigManager.ADVANCEMENT_LEVEL + loopStepCounter, 1);
                PlayerPrefs.SetInt("StageCleared" + loopStepCounter, 0);
                PlayerPrefs.SetInt("LevelScore" + loopStepCounter, 0);
            }
        }
        PlayerPrefs.Save();
    }

    private void UpdateStageButtons()
    {
        for (int iterationCounter = 0; iterationCounter < _totalStagesCount; iterationCounter++)
        {
            if (levelOptionButtons[iterationCounter] != null)
            {
                if (PlayerPrefs.GetInt(GlobalConfigManager.ADVANCEMENT_LEVEL + iterationCounter, 0) == 1)
                {
                    levelOptionButtons[iterationCounter].GetComponent<Button>().interactable = true;
                    int score = PlayerPrefs.GetInt("LevelScore" + iterationCounter, 0);
                    UpdateStars(iterationCounter, score);
                }
            }
        }
    }

    private void UpdateStars(int levelIndex, int score)
    {
        int starsToAward = 0;
        if (score >= 300)
        {
            starsToAward = 3;
        }
        else if (score >= 200)
        {
            starsToAward = 2;
        }
        else if (score >= 100)
        {
            starsToAward = 1;
        }

        for (int starIndex = 0; starIndex < 3; starIndex++)
        {
            if (_levels[levelIndex].stars[starIndex] != null)
            {
                if (starIndex < starsToAward)
                {
                    _levels[levelIndex].stars[starIndex].sprite = earnedStarIcon;
                }
                else
                {
                    _levels[levelIndex].stars[starIndex].sprite = _emptyStarSprite;
                }
            }
        }
    }

    public void PickStage(int levelIndex)
    {
        _chosenLevelIndex = levelIndex;
        chooseButton.interactable = true;
    }

    public void InitiateStage()
    {
        if (_chosenLevelIndex != -1)
        {
            PlayerPrefs.SetInt(GlobalConfigManager.CURRENT_ACTIVE_STAGE, _chosenLevelIndex);
            PlayerPrefs.Save();
            SceneManager.LoadScene(GlobalConfigManager.QUEST_ARENA);
        }
    }

    public void AchieveLevelCompletion(int levelIndex, int score)
    {

        PlayerPrefs.SetInt("StageCleared" + levelIndex, 1);
        PlayerPrefs.SetInt("LevelScore" + levelIndex, score);
        PlayerPrefs.Save();
        
        UpdateStars(levelIndex, score);  // Обновляем звезды после сохранения

        if (levelIndex < _totalStagesCount - 1)
        {
            PlayerPrefs.SetInt(GlobalConfigManager.ADVANCEMENT_LEVEL + (levelIndex + 1), 1);
        }
    }
}
