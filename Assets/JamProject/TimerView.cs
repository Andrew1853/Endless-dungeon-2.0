using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimerView : MonoBehaviour
{
    [SerializeField] TMP_Text _textComponent;
    public void UpdateTimerText(float value)
    {
        _textComponent.text = FormatTime(value);
    }
    static string FormatTime(float time)
    {
        int totalSeconds = (int)time;
        int minutes = totalSeconds / 60;
        int seconds = totalSeconds % 60;

        return string.Format("{0}:{1:00}", minutes, seconds);
    }
}
