using System.Collections;
using System.Collections.Generic;
using Doozy.Engine.UI;
using TMPro;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI combatLogText;
    [SerializeField] UIView countDownView;
    [SerializeField] TextMeshProUGUI questText;
    [SerializeField] UIView questView;

    public static CanvasManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void addCombatLog(string text)
    {
        if (combatLogText != null)
        {
            combatLogText.text = text + "\n" + combatLogText.text;
        }
    }
    
    public void SetCountdownText(string text)
    {
        countDownView.GetComponentInChildren<TextMeshProUGUI>().text = text;
    }
    public void hideCountdownView()
    {
        countDownView.Hide();
    }

    public void showGoToEndZoneText()
    {
        questText.text = "You must go to the endzone";
        questView.Show();
    }
  
}