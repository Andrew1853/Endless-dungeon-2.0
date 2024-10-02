using UnityEngine;
using UnityEngine.Events;

public class CombatController : MonoBehaviour
{
    public UnityEvent OnAttackStarts;
    public UnityEvent OnAttackEnds;

    [SerializeField] Animator _animator;
    [SerializeField] WeaponController _weapon;
    bool _isAttacking = false;

    public bool IsAttacking { get => _isAttacking; }
    private void Start()
    {
        if (_weapon != null)
        {
            _weapon.Init(GetComponent<CharacterFacade>());
        }
    }
    public void StartAttack()
    {
        _weapon.IsActive = true;
        _animator.SetTrigger("Attack");
        OnAttackStarts?.Invoke();
    }
    public void OnAttackAnimEnd()
    {
        _weapon.IsActive = false;

        OnAttackEnds?.Invoke();
    }
}