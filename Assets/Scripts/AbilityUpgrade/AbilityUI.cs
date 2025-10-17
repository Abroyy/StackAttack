using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class AbilityUI : MonoBehaviour
{
    public GameObject cardPrefab;
    public Transform cardParent;

    public void Show(List<AbilityUpgradeData> upgrades)
    {
        Time.timeScale = 0f; // oyun durur
        foreach (Transform child in cardParent)
            Destroy(child.gameObject);

        foreach (var upgrade in upgrades)
        {
            GameObject card = Instantiate(cardPrefab, cardParent);
            Image icon = card.transform.Find("UpgradeIcon").GetComponent<Image>();
            TextMeshProUGUI desc = card.transform.Find("Description").GetComponent<TextMeshProUGUI>();
            Button btn = card.GetComponent<Button>();

            if (upgrade.upgradeIcon) icon.sprite = upgrade.upgradeIcon;
            desc.text = upgrade.description;

            btn.onClick.AddListener(() =>
            {
                if (upgrade.addDamage > 0 || upgrade.fireRateMultiplier != 1f || upgrade.addProjectile > 0 || upgrade.piercing > 0)
                    GameEvents.UpgradeAbility(upgrade.type, upgrade.addDamage, upgrade.fireRateMultiplier, upgrade.addProjectile, upgrade.piercing);
                else
                    GameEvents.UnlockWeapon(upgrade.type);

                Time.timeScale = 1f;
                gameObject.SetActive(false);
            });
        }

        gameObject.SetActive(true);
    }
}
