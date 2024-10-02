using DemiurgEngine.StatSystem;
using InventorySystem;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using EventBus = DemiurgEngine.EventBus;

namespace Assets.JamProject
{
    public class MoneyView : MonoBehaviour
    {
        [SerializeField] TMP_Text _text;

        Money _money;
        void Start()
        {
            GlobalStats.SubscribeToCustomStat<Money>(OnMoneyChanged, OnMoneyStatInited);
        }

        void OnMoneyChanged(float v1, float money)
        {
            _text.text = ((int)_money.BaseValue).ToString();
        }
        void OnMoneyStatInited(Stat moneyStat)
        {
            _money = moneyStat as Money;
            OnMoneyChanged(0, _money.BaseValue);
        }
    }
}