using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthView : MonoBehaviour, IInitializableUI
{
    [SerializeField] HealthController _health;
    Image _image;
    public void Initialize()
    {
        _image = GetComponent<Image>();
        _health.onHealthChanged += UpdateView;
        UpdateView(_health.CurrentHealth);
    }
    void UpdateView(float value)
    {
        _image.fillAmount = value / _health.MaxHealth;
    }
}
