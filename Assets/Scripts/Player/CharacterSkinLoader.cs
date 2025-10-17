using UnityEngine;
using System.Collections.Generic;

public class CharacterSkinLoader : MonoBehaviour
{
    public SpriteRenderer characterRenderer;
    public TrailRenderer characterTrail;

    [Header("Renk Listeleri")]
    public List<Color> skinColors;
    public List<Color> trailColors;

    private void Start()
    {
        CustomizeManager.ApplySavedCustomization(characterRenderer, characterTrail, skinColors, trailColors);
    }
}
