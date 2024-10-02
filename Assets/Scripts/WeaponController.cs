using UnityEngine;

public class WeaponController : MonoBehaviour
{
    CharacterFacade _owner;
    bool _isActive;

    public bool IsActive { get => _isActive; set => _isActive = value; }

    public void Init(CharacterFacade owner)
    {
        _owner = owner;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform == _owner.transform)
        {
            return;
        }
        if (_isActive)
        {
            CharacterFacade c;
            if (collision.TryGetComponent<CharacterFacade>(out c))
            {
                c.ApplyDamage(_owner.equip.WeaponItem.damage, _owner);
            }
        }
    }
}
