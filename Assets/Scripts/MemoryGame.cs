using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEngine.Serialization;

public class MemoryGame : MonoBehaviour
{
    [SerializeField] private Sprite[] allImages;
    [SerializeField] private Sprite hiddenImage;
    [SerializeField] private Image[] gridImages;
    [SerializeField] private Image targetImage;
    [FormerlySerializedAs("timerText")] [SerializeField] private TextMeshProUGUI _countdownText;
    [FormerlySerializedAs("scoreText")] [SerializeField] private TextMeshProUGUI[] _pointsText;
    [SerializeField] private Button startButton;
    [SerializeField] private Button finishButton;
    [SerializeField] private GameObject _gameSceneGrid;
    [SerializeField] private GameObject _levelDone;

    private Sprite currentTarget;
    private int score;
    private float timeRemaining = 20f;
    private bool isGameActive;
    private bool[] isRevealed;
    private Dictionary<int, Sprite> originalImages = new Dictionary<int, Sprite>();
    private List<Sprite> targetSprites = new List<Sprite>();


    void Start()
    {
        startButton.onClick.AddListener(StartGame);
        finishButton.onClick.AddListener(FinishGame);
        ResetGame();
    }

    void ResetGame()
    {
        foreach (var textScore in _pointsText)
        {
           textScore.text = score.ToString();
        }
        
        isRevealed = new bool[gridImages.Length];
        originalImages.Clear();
        targetSprites.Clear();

        int randomChoiceIndex = Random.Range(0, allImages.Length);
        currentTarget = allImages[randomChoiceIndex];
        targetImage.sprite = currentTarget;

        targetSprites.Add(currentTarget);
        targetSprites.Add(currentTarget);
        targetSprites.Add(currentTarget);

        HashSet<int> usedRandomIndices = new HashSet<int>();

        while (usedRandomIndices.Count < 3)
        {
            int gridIndex = Random.Range(0, gridImages.Length);
            if (usedRandomIndices.Add(gridIndex))
            {
                gridImages[gridIndex].sprite = currentTarget;
                originalImages[gridIndex] = currentTarget;
            }
        }

        for (int loopCounter = 0; loopCounter < gridImages.Length; loopCounter++)
        {
            if (!usedRandomIndices.Contains(loopCounter))
            {
                Sprite randomSprite = GetRandomNonTargetSprite();
                gridImages[loopCounter].sprite = randomSprite;
                originalImages[loopCounter] = randomSprite;
            }

            int index = loopCounter;
            gridImages[loopCounter].GetComponent<Button>().onClick.RemoveAllListeners();
            gridImages[loopCounter].GetComponent<Button>().onClick.AddListener(() => OnGridImageClicked(index));
        }
    }

    public int RetrieveScore()
    {
        return score;
    }

    Sprite GetRandomNonTargetSprite()
    {
        Sprite randomSprite;
        do
        {
            randomSprite = allImages[Random.Range(0, allImages.Length)];
        } while (randomSprite == currentTarget);

        return randomSprite;
    }

    void StartGame()
    {
        for (int arrayIndex = 0; arrayIndex < gridImages.Length; arrayIndex++)
        {
            gridImages[arrayIndex].sprite = hiddenImage;
            isRevealed[arrayIndex] = false;
        }

        isGameActive = true;
        timeRemaining = 20f;
    }

    void Update()
    {
        if (isGameActive)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                _countdownText.text = "00:" + Mathf.Floor(timeRemaining).ToString("00");
            }
            else
            {
                TerminateGameplay();
            }
        }
    }

    void OnGridImageClicked(int index)
    {
        if (!isGameActive || isRevealed[index]) return;

        gridImages[index].sprite = originalImages[index];

        if (originalImages[index] == currentTarget)
        {
            score += 100;
        }
        else
        {
            score -= 50;
        }

        isRevealed[index] = true;
        foreach (var textScore in _pointsText)
        {
            textScore.text = score.ToString();
        }
    }

    void FinishGame()
    {
        TerminateGameplay();
    }

    void TerminateGameplay()
    {
        isGameActive = false;
        finishButton.gameObject.SetActive(false);
        _gameSceneGrid.SetActive(false);
        _levelDone.SetActive(true);
    }
}
