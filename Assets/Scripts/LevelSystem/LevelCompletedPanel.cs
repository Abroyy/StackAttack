using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelCompletedManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject levelCompletedPanel;
    public Transform rewardParent;
    public GameObject rewardSlotPrefab;
    public Button continueButton;
    public TextMeshProUGUI resultText;
    public Image resultBg;

    [Header("Win/Lose References")]
    public Color winColor;
    public Color loseColor;

    private void OnEnable()
    {
        GameEvents.OnBossDead += OnLevelWin;
        GameEvents.OnGameOver += OnPlayerLost;
    }

    private void OnDisable()
    {
        GameEvents.OnBossDead -= OnLevelWin;
        GameEvents.OnGameOver -= OnPlayerLost;
    }

    private void Start()
    {
        levelCompletedPanel.SetActive(false);
        continueButton.onClick.AddListener(OnContinueClicked);
    }
    private void OnLevelWin()
    {
        StartCoroutine(ShowPanelAfterDelay(true));
    }

    private void OnPlayerLost()
    {
        StartCoroutine(ShowPanelAfterDelay(false));
    }
    private IEnumerator ShowPanelAfterDelay(bool isWin)
    {
        yield return new WaitForSeconds(1f);

        levelCompletedPanel.SetActive(true);
        resultText.text = isWin ? "WIN" : "LOST";
        resultText.color = isWin ? winColor : loseColor;
        resultBg.color = isWin ? winColor : loseColor;

        foreach (Transform child in rewardParent)
            Destroy(child.gameObject);

        int levelIndex = SceneManager.GetActiveScene().buildIndex;
        LevelRewardSystem rewardSystem = FindAnyObjectByType<LevelRewardSystem>();

        if (rewardSystem == null) yield break;

        var reward = System.Array.Find(rewardSystem.levelRewards, l => l.levelIndex == levelIndex);
        if (reward == null) yield break;

        int blueprintAmount = Random.Range(reward.minBlueprint, reward.maxBlueprint + 1);

        //  Ödül Slotlarýný Göster 
        if (isWin)
        {
            // ITEM SLOT
            GameObject itemSlot = Instantiate(rewardSlotPrefab, rewardParent);
            itemSlot.transform.Find("Icon").GetComponent<Image>().sprite = reward.rewardItem.icon;
            itemSlot.transform.Find("Amount").GetComponent<TextMeshProUGUI>().text = "x1";

            // BLUEPRINT SLOT
            GameObject blueprintSlot = Instantiate(rewardSlotPrefab, rewardParent);
            blueprintSlot.transform.Find("Icon").GetComponent<Image>().sprite = reward.rewardItem.icon;
            blueprintSlot.transform.Find("Amount").GetComponent<TextMeshProUGUI>().text = blueprintAmount.ToString();

            //Reward verme iþlemi
            if (!InventoryManager.Instance.ownedItems.Contains(reward.rewardItem))
                InventoryManager.Instance.AddItem(reward.rewardItem);
            InventoryManager.Instance.AddBlueprint(reward.rewardItem, blueprintAmount);

            GameEvents.InvokeLevelReward(reward.rewardItem, blueprintAmount);
            LevelSelectManager.UnlockNextLevel(SceneManager.GetActiveScene().name);
        }
        else
        {
            GameObject blueprintSlot = Instantiate(rewardSlotPrefab, rewardParent);
            blueprintSlot.transform.Find("Icon").GetComponent<Image>().sprite = reward.rewardItem.icon;
            blueprintSlot.transform.Find("Amount").GetComponent<TextMeshProUGUI>().text = blueprintAmount.ToString();

            // Blueprint ekle ama item verme
            InventoryManager.Instance.AddBlueprint(reward.rewardItem, blueprintAmount);
            GameEvents.InvokeLevelReward(reward.rewardItem, blueprintAmount);
        }
        Time.timeScale = 0f;

    }
    private void OnContinueClicked()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
