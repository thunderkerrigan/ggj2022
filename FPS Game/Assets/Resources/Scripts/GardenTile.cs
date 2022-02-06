using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GardenTile : MonoBehaviour
{
    private StateGarden topGarden;
    private StateGarden middleGarden;
    private StateGarden bottomGarden;

    private int currentPV;
    private Garden parentGarden;
    public SpriteRenderer topSprite;
    public SpriteRenderer middleSprite;
    public SpriteRenderer bottomSprite;

    public void SetGarden(Garden parent, int hitPoints, StateGarden top, StateGarden middle, StateGarden bottom)
    {
        parentGarden = parent;
        topGarden = top;
        middleGarden = middle;
        bottomGarden = bottom;
        ApplyGarden(hitPoints);
        parentGarden.OnPotagerdamage += ApplyGarden;
    }

    private void ApplyGarden(int hitPoints)
    {
        currentPV = hitPoints;
            topSprite.sprite = topGarden.GetSprite(currentPV);
        middleSprite.sprite = middleGarden.GetSprite(currentPV);
        bottomSprite.sprite = bottomGarden.GetSprite(currentPV);
        if (hitPoints <= 0)
        {
            parentGarden.OnPotagerdamage -= ApplyGarden;
        }
    }
}