using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "HappyTree/New Garden")]
public class StateGarden : ScriptableObject
{
    [SerializeField] private GardenType m_type;
    [SerializeField] private GardenRow m_row;
    public GardenType type => m_type;
    public GardenRow row => m_row;
    public Sprite _100HP;
    public Sprite _66HP;
    public Sprite _33HP;
    public Sprite _0HP;

    public Sprite GetSprite(int hp)
    {
        if (hp > 66)
        {
            return _100HP;
        }
        else if (hp > 33)
        {
            return _66HP;
        }
        else if (hp > 0)
        {
            return _33HP;
        }
        else
        {
            return _0HP;
        }
    }
}