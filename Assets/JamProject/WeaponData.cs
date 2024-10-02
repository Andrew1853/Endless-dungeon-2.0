using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "Weapon")]
public class WeaponData : ScriptableObject
{
    public string weaponName;
    public float fireRate = 0.5f;        // Скорострельность
    public int bulletsPerShot = 1;       // Количество пуль при выстреле (для дробовика)
    public float recoilStrength = 2f;    // Сила отдачи
    public int maxAmmo = 6;              // Количество патронов до перезарядки
    public float reloadTime = 2f;        // Время перезарядки
    public float damage = 10f;           // Урон
    public float spread = 0.1f;          // Разброс пуль (для дробовика)
    public AudioClip fireSound;          // Звук выстрела
}
