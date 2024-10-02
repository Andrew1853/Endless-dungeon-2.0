using UnityEngine;
using DemiurgEngine.StatSystem;
using System;
using System.Collections;

[RequireComponent(typeof(StatsControllerCharacter))]
public class HealthController : MonoBehaviour
{
    [SerializeField] float _defaultKnockOutTime = 1f;

    public event Action<float> onHealthChanged;
    public event Action<float, object> onDamageApplied;

    DeathHandler _deathHandler;

    StatsControllerCharacter _sc;

    [SerializeField]
    [AutoAssignStat]
    Stat _health;
    public float CurrentHealth { get => _health.CurrentValue; set { _health.SetCurrentValue(value, true); } }
    public float MaxHealth { get => _health.BaseValue; set { _health.SetBaseValue(value, true); } }

    [SerializeField] float _invulnerabilityTime;
    float _lastAttackTime;

    [SerializeField] float _currentHealthValue;
     
    public void Init()
    {
        _deathHandler = GetComponent<DeathHandler>();
        _sc = GetComponent<StatsControllerCharacter>();

        _health.onChange += OnHealthChange;
    }
    private void Update()
    {
        _currentHealthValue = _health.CurrentValue;
    }
    void OnHealthChange(float current, float max)
    {
        onHealthChanged?.Invoke(current);
        _currentHealthValue = current;
    }
    public void ApplyDamage(float damage, object source)
    {
        if (Time.time - _lastAttackTime < _invulnerabilityTime)
        {
            return;
        }
        _lastAttackTime = Time.time;

        if (damage >= CurrentHealth)
        {
            CurrentHealth = 0;
        }
        else
        {
            CurrentHealth -= damage;
        }

        onDamageApplied?.Invoke(_health.CurrentValue, source);

        if (CurrentHealth == 0)
        {
            OnZeroHP(source);
        }
    }

    public virtual void OnZeroHP(object source)
    {
        if (source.GetType() == typeof(CharacterFacade) && tag == "Human")
        {
            _health.SetCurrentValue(1, false);
        }
        else
        {
            _deathHandler.Die();
        }
    }
    public void KnockOut(float time)
    {
        StartKnockOut();
        StartCoroutine(KnockOutRoutine(time));
    }
    public void KnockOut()
    {
        StartKnockOut();
        StartCoroutine(KnockOutRoutine(_defaultKnockOutTime));
    }
    void StartKnockOut()
    {
        CharacterFacade c = GetComponent<CharacterFacade>();
        if (c.isPlayer)
        {

        }
        else
        {
            if (c.brain != null)
            {
                c.brain.enabled = false;
            }
        }
        c.mover.Stop();
    }
    void EndKnockOut()
    {

    }
    IEnumerator KnockOutRoutine(float t)
    {
        yield return new WaitForSeconds(t);
        EndKnockOut();
    }
}
