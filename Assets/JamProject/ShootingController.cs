using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingController : MonoBehaviour
{
    [Header("== ОСНОВНЫЕ НАСТРОЙКИ ==")]                        // Камера игрока
    public List<WeaponData> weaponTypes;                 // Список типов оружия
    public int selectedWeapon = 0;                       // Индекс текущего оружия
    private AudioSource audioSource;

    [Header("== НАСТРОЙКИ ОРУЖИЯ ==")]
    private WeaponData currentWeapon;                    // Текущее выбранное оружие
    private float nextFireTime = 0f;                     // Время до следующего выстрела
    private int currentAmmo;                             // Текущее количество патронов
    private bool isReloading = false;                    // Флаг перезарядки

    [Header("== НАСТРОЙКИ СЛЕДА ОТ ПУЛИ ==")]
    // Параметры следа от пули
    public float maxTrailDistance = 50f;                 // Максимальная длина трассировки
    public float trailStepDistance = 0.5f;
    public GameObject bulletTrailPrefab;
    public GameObject explosionPrefab;            // Расстояние между частицами на траектории

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        SelectWeapon();
    }
    void Update()
    {
        if (isReloading)
            return;
        if (Input.GetButtonDown("Fire1") && Time.time >= nextFireTime)
        {
            if (currentAmmo > 0)
            {
                Shoot();
            }
            else
            {
                StartCoroutine(Reload());
            }
        }
    }
    void Shoot()
    {
        nextFireTime = Time.time + currentWeapon.fireRate;
        if (currentWeapon.fireSound)
        {
            audioSource.PlayOneShot(currentWeapon.fireSound);
        }
        GetComponent<RecoilEffect>().TriggerRecoil();

        for (int i = 0; i < currentWeapon.bulletsPerShot; i++)
        {
            FireBullet();
        }

        currentAmmo--;
    }

    void FireBullet()
    {
        Vector3 rayDirection = Camera.main.transform.forward;

        if (currentWeapon.bulletsPerShot > 1) // Для дробовика
        {
            rayDirection.x += Random.Range(-currentWeapon.spread, currentWeapon.spread);
            rayDirection.y += Random.Range(-currentWeapon.spread, currentWeapon.spread);
        }

        Ray ray = new Ray(Camera.main.transform.position, rayDirection);
        RaycastHit hit;

        float currentDistance = 0f;
        while (currentDistance < maxTrailDistance)
        {
            if (Physics.Raycast(ray, out hit, trailStepDistance))
            {
                // Если попали в объект, показываем след и разрушаем
                CreateBulletTrail(ray.origin, hit.point);
                ApplyDamage(hit);
                CreateExplosion(hit);

                break;
            }
            else
            {
                // Если нет попадания, создаем след на промежуточном расстоянии
                Vector3 nextPosition = ray.origin + ray.direction * trailStepDistance;
                CreateBulletTrail(ray.origin, nextPosition);

                // Смещаем Ray дальше
                ray.origin = nextPosition;
                currentDistance += trailStepDistance;
            }
        }
    }
    void CreateExplosion(RaycastHit hit)
    {
        NPCFollow target = hit.transform.GetComponent<NPCFollow>();
        if (target == null)
        {
            GameObject explosion = Instantiate(explosionPrefab, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(explosion, 2f);
        }
    }
    void CreateBulletTrail(Vector3 startPosition, Vector3 endPosition)
    {
        GameObject trail = Instantiate(bulletTrailPrefab, startPosition, Quaternion.identity);

        // Направляем частицы в сторону цели
        Vector3 direction = (endPosition - startPosition).normalized;
        trail.transform.forward = direction;

        Destroy(trail, 4f);
    }
    void ApplyDamage(RaycastHit hit)
    {
        //Health target = hit.collider.GetComponent<Health>();
        NPCFollow target = hit.collider.GetComponent<NPCFollow>();
        if (target != null)
        {
            target.ApplyKnockback();
            GetComponent<CrosshairController>().ShowHitIndicator();
        }
    }

    IEnumerator Reload()
    {
        isReloading = true;
        yield return new WaitForSeconds(currentWeapon.reloadTime);
        currentAmmo = currentWeapon.maxAmmo;
        isReloading = false;
    }

    void SelectWeapon()
    {
        currentWeapon = weaponTypes[selectedWeapon];
        currentAmmo = currentWeapon.maxAmmo;
    }
}
