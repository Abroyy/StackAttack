using UnityEngine;

public enum ItemType
{
    Damage,
    Projectile
}

[CreateAssetMenu(menuName = "Game/Item Data")]
public class ItemData : ScriptableObject
{
    [Header("Item Info")]
    public string itemName;
    public ItemType itemType;
    public Sprite icon;

    [Header("Bonuses")]
    public int bonusDamage;
    public int bonusProjectile;

    [Header("Level Up Costs")]
    public int costGold = 0;
    public int costBlueprint = 0;

    [Header("Description")]
    [TextArea] public string description;
}
