using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TimerResponse : MonoBehaviour
{
    TMP_Text timerText;
    private void Awake()
    {
        timerText = GetComponentInChildren<TMP_Text>();
    }

    private void Update()
    {
        timerText.text = RoundManager.instance.GetTimer().ToString();
    }
}
