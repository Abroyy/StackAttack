using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class LevelSelectManager : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI levelNameText;
    public Button previousButton;
    public Button nextButton;
    public Button playButton;

    [Header("Level Settings")]
    public string[] inspectorLevelSceneNames;
    public static string[] levelSceneNames;
    public static int maxUnlockedLevel = 0;

    private int currentLevelIndex = 0;

    private void Awake()
    {

        levelSceneNames = inspectorLevelSceneNames;
    }
    private void Start()
    {

        maxUnlockedLevel = PlayerPrefs.GetInt("MaxUnlockedLevel", 0);
        currentLevelIndex = PlayerPrefs.GetInt("LastSelectedLevel", 0);

        UpdateUI();

        previousButton.onClick.AddListener(PrevLevel);
        nextButton.onClick.AddListener(NextLevel);
        playButton.onClick.AddListener(PlaySelectedLevel);
    }
    private void UpdateUI()
    {
        levelNameText.text = $"LEVEL {currentLevelIndex + 1}";

        previousButton.gameObject.SetActive(currentLevelIndex > 0);
        nextButton.gameObject.SetActive(currentLevelIndex < maxUnlockedLevel && currentLevelIndex < levelSceneNames.Length - 1);
    }
    private void PrevLevel()
    {
        currentLevelIndex = Mathf.Max(0, currentLevelIndex - 1);
        PlayerPrefs.SetInt("LastSelectedLevel", currentLevelIndex);
        UpdateUI();
    }
    private void NextLevel()
    {
        currentLevelIndex = Mathf.Min(levelSceneNames.Length - 1, currentLevelIndex + 1);
        PlayerPrefs.SetInt("LastSelectedLevel", currentLevelIndex);
        UpdateUI();
    }
    private void PlaySelectedLevel()
    {
        string sceneName = levelSceneNames[currentLevelIndex];
        SceneManager.LoadScene(sceneName);
    }
    public static void UnlockNextLevel(string completedSceneName)
    {
        if (levelSceneNames == null || levelSceneNames.Length == 0) return;

        int completedIndex = System.Array.IndexOf(levelSceneNames, completedSceneName);
        if (completedIndex == -1) return;

        if (completedIndex >= maxUnlockedLevel && completedIndex < levelSceneNames.Length - 1)
        {
            maxUnlockedLevel = completedIndex + 1;
            PlayerPrefs.SetInt("MaxUnlockedLevel", maxUnlockedLevel);
            PlayerPrefs.Save();
        }
    }
}
