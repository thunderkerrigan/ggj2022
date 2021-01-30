using System.Collections;
using System.Collections.Generic;
using Doozy.Engine.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class CanvasManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI combatLogText;
    [SerializeField] UIView countDownView;
    [SerializeField] TextMeshProUGUI questText;
    [SerializeField] UIView questView;
    [SerializeField] UIView malusView;

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
    
    public void showStunnedView()
    {
        malusView.GetComponentInChildren<Image>().sprite = Resources.Load <Sprite> ("Reverse");
        malusView.Show();
        StartCoroutine(hideMalusView(3));
    }
    
    public void showControlReverseView()
    {
        malusView.GetComponentInChildren<Image>().sprite = Resources.Load <Sprite> ("Stun");
        malusView.Show();
        StartCoroutine(hideMalusView(5));
    }
    
    IEnumerator hideMalusView(int duration)
    {
        yield return new WaitForSeconds(duration);
        malusView.Hide();
    }

}