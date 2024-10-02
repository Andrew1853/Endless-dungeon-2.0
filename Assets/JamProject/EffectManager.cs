using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public GameObject explosionEffect;

    public void TriggerExplosion(Vector3 position)
    {
        Instantiate(explosionEffect, position, Quaternion.identity);
    }
}
