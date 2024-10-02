using DemiurgEngine.StatSystem;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class StatView : UIBehaviour
{
    [SerializeField] Image _barImage;
    //TODO implement this
    [SerializeField] Image _fadeBarImage;

    [SerializeField] TMP_Text _textCurrent;
    [SerializeField] TMP_Text _textMax;

    [SerializeField] Stat _statName;
    [SerializeField] StatsControllerCharacter _statsController;
    [SerializeField] int _partsQuantity;
    Stat _stat;
    float _currentBarWidth;

    public StatsControllerCharacter StatsController { get => _statsController; set { _statsController = value; _stat = _statsController.GetStat(_statName); } }

    // Start is called before the first frame update
    public enum StatType
    {
        simple = 0,
        complex
    }
    public void Init()
    {
        _stat = _statsController.GetStat(_statName);

        UpdateBar(_stat.CurrentValue, _stat.BaseValue);
        _stat.onChange += UpdateBar;
    }
    void UpdateBar(float current, float max)
    {
        //_currentBarWidth = Mathf.Round(_health.CurrentValue / _health.Value * _partsQuantity);
        //_barImage.fillAmount = _currentBarWidth / _partsQuantity;
        _barImage.fillAmount = current / max;
        if (_textCurrent)
        {
            _textCurrent.text = current.ToString();
        }
        if (_textMax)
        {
            _textMax.text = max.ToString();
        }
    }
}
