using DemiurgEngine;
using InventorySystem;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.Rendering.DebugUI.Table;


public class DeathHandler : MonoBehaviour
{
    public UnityEvent OnDeath;

    [SerializeField] Sprite _corpseSprite;
    [SerializeField] SpriteRenderer _spriteRenderer;
    [SerializeField] AudioSource _audioSource;
    [SerializeField] AudioClip _deathSound;
    [SerializeField] float _destroyDelay;

    public bool Dead { get; private set; } = false;

    List<Action> _onDeath = new();
    public void Die()
    {
        if (Dead)
        {
            return;
        }
        Dead = true;

        CharacterRow creatureRow = GetComponent<CharacterFacade>().row;

        creatureRow.isAlive.Value = false;
        creatureRow.spawner.Value?.OnCreatureDeath(creatureRow.gameObject.Value);
        //ExecuteActions();
        _onDeath.ForEach(action => action.Invoke());
        //Invoke("DestroyGameObject", _destroyDelay);
        GetComponentInChildren<TMP_Text>().text = "Dead";

        OnDeath?.Invoke();

        EventBus.Publish(new CharacterDeathEvent() { character = GetComponent<CharacterFacade>() });
    }
    //protected virtual void ExecuteActions()
    //{
    //    PlaySound();
    //}
    public void PlaySound()
    {
        _audioSource?.PlayOneShot(_deathSound);
    }
    void DestroyGameObject() { Destroy(gameObject); }
    public void AddActionOnDeath(Action action)
    {
        _onDeath.Add(action);
    }
}
