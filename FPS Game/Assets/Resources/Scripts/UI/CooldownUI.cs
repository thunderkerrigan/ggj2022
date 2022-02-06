using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CooldownUI : MonoBehaviour
{
    
    private GameManager _localGameManager;
    
    // Start is called before the first frame update
    void Start()
    {
        _localGameManager = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
        _localGameManager.OnTimerUpdate += UpdateCooldown;
    }
    
    private void OnDestroy()
    {
        if ((_localGameManager))
        {
            _localGameManager.OnTimerUpdate -= UpdateCooldown;
        }
    }
    
    private void UpdateCooldown(float time)
    {
        GetComponent<TextMeshProUGUI>().text = "Remaining Time:" + time.ToString("F1");
    }
}
