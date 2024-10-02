using System;
using System.Collections.Generic;
using UnityEngine;

namespace DemiurgEngine.StatSystem
{
    [CreateAssetMenu]
    [Serializable]
    public class Stat : ScriptableObject, IJsonSerializable
    {
        /// <summary>
        /// 1й аргумент: текущее значение 
        /// 2й аргумент: максимальное значение с применением модификаторов
        /// </summary>
        public event Action<float, float> onChange;

        public Stat AssetRef { get; private set; }
        public State CurrentState { get => _state; set => _state = value; }

        StatsController _controller;


        [SerializeField] float _notCalculatedBaseValue;
        [SerializeField] float _cap = -1;

        [Serializable]
        public class State
        {
            public float currentValue = 0;
            public float calculatedBaseValue;
        }
        State _state;

        public float CurrentValue { get => _state.currentValue; private set { _state.currentValue = Mathf.Clamp(value, 0, BaseValue); } }
        public float BaseValue { get => _state.calculatedBaseValue; }
        public float Cap { get => _cap; set => _cap = value; }
        public StatsController Controller => _controller;

        List<Modifier> _modifiers = new();

        [SerializeField]
        [Range(0, 1)] float _currentValueOnStart = 1;
        public virtual void Init(StatsController controller, Stat origin)
        {
            AssetRef = origin;

            _state = new State();
            if (_cap < 0)
            {
                _cap = float.MaxValue;
            }
            SetBaseValue(_notCalculatedBaseValue, false);
            SetCurrentValueOnStart();
            _controller = controller;
        }
        public void Override(float value)
        {
            SetBaseValue(value, false);
            SetCurrentValueOnStart();
            onChange?.Invoke(_state.currentValue, _state.calculatedBaseValue);
        }
        void SetCurrentValueOnStart()
        {
            _state.currentValue = _state.calculatedBaseValue * _currentValueOnStart;
        }
        public void SetBaseValue(float value, bool invokeCallbacks)
        {
            _notCalculatedBaseValue = value;
            Calculate();

            if (invokeCallbacks)
            {
                onChange?.Invoke(_state.currentValue, _state.calculatedBaseValue);
                OnChangeInternal();

            }
        }
        public void SetCurrentValue(float value, bool invokeCallbacks)
        {
            CurrentValue = value;
            if (invokeCallbacks)
            {
                onChange?.Invoke(_state.currentValue, _state.calculatedBaseValue);
                OnChangeInternal();
            }
        }
        public void AddBaseValue(float add, bool invokeCallbacks)
        {
            SetBaseValue(_notCalculatedBaseValue + add, invokeCallbacks);
        }
        public void AddCurrentValue(float add, bool invokeCallbacks)
        {
            SetCurrentValue(CurrentValue + add, invokeCallbacks);

        }
        public void DecreaseBaseValue(float decrease, bool invokeCallbacks)
        {
            SetBaseValue(_notCalculatedBaseValue - decrease, invokeCallbacks);
        }
        public void DecreaseCurrentValue(float decrease, bool invokeCallbacks)
        {
            SetCurrentValue(CurrentValue - decrease, invokeCallbacks);
        }
        void Calculate()
        {
            float finalValue = _notCalculatedBaseValue;
            float sumPercentAdd = 0f;
            _modifiers.Sort((x, y) => x.type.CompareTo(y.type));

            bool addedPercent = false;
            for (int i = 0; i < _modifiers.Count; i++)
            {
                Modifier mod = _modifiers[i];
                if (mod.type == ModifierType.Add)
                {
                    finalValue += mod.value;
                }
                else if (mod.type == ModifierType.PercentAdd)
                {
                    addedPercent = true;
                    sumPercentAdd += mod.value;
                }
                //else if (mod.Type == ModifierType.PercentMult)
                //{
                //    finalValue *= 1f + mod.Value;
                //}
            }
            if (addedPercent)
            {
                finalValue += finalValue * (sumPercentAdd / 100);
            }
            if (finalValue >= _cap)
            {
                finalValue = _cap;
                if (Cap == 0)
                {
                    Debug.LogWarning($"{name} stat cap is zero!");
                }
            }

            _state.calculatedBaseValue = finalValue;
        }
        public void AddModifier(Modifier modifier)
        {
            _modifiers.Add(modifier);
            OnChangeInternal();
            onChange?.Invoke(CurrentValue, BaseValue);
        }
        public Modifier[] GetModifiers() => _modifiers.ToArray();
        public virtual void GetObjectData(Dictionary<string, object> data)
        {
            data.Add("Name", name);
            data.Add("State", _state);
        }

        public virtual void SetObjectData(Dictionary<string, object> data)
        {
            State state = data["State"] as State;
            if (state != null)
            {
                _state = state;
                Calculate();
            }
        }

        protected virtual void OnChangeInternal()
        {
            if (AssetRef == this)
            {
                Debug.LogWarning("Попытка изменить стат в ассетах!");
            }
        }
    }
}