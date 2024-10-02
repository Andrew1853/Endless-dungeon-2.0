using DemiurgEngine.AI;
using DemiurgEngine.StatSystem;
using InventorySystem;
using System.Collections;
using UnityEngine;
using UnityEngine.Animations;
using static UnityEngine.Rendering.DebugUI;

public class CharacterFacade : MonoBehaviour
{
    [SerializeField] float _rotationThreshold;
    public CharacterRow row;
    public Brain brain;
    public CombatController combat;
    public HealthController health;
    public ObjectMover mover;
    public EquipController equip;
    public InventoryController inventory;
    LookAtConstraint lookAt;

    [SerializeField] Transform model;
            
    public bool isPlayer = false;
    private bool _initialized = false;
    bool lookingAtObject = false;

    public bool Initialized { get => _initialized; }
    void Awake()
    {
        brain = GetComponent<Brain>();
        combat = GetComponent<CombatController>();
        health = GetComponent<HealthController>();
        mover = GetComponent<ObjectMover>();
        equip= GetComponent<EquipController>();
        inventory = GetComponent<InventoryController>();
    }
    private void Start()
    {
        GameManager.instance.OnCharacterInstantiated(this);
    }
    Vector3 lastPos;
    void Update()
    {
        if (lookingAtObject == false)
        {
            Vector3 lookDir = transform.position - lastPos;
            if (lookDir.magnitude > _rotationThreshold)
            {
                model.rotation = Quaternion.FromToRotation(transform.up, lookDir.normalized);
                model.eulerAngles = new Vector3(0f, 0f, model.eulerAngles.z);
            }
        }
        lastPos = transform.position;
    }
    public void InitializeCharacter(CharacterRow rowInCharacterTable)
    {
        row = rowInCharacterTable;

        GetComponent<StatsControllerCharacter>().Init();
        GetComponent<HealthController>().Init();

        _initialized = true;
    }
    public void BindToSpawner(Spawner s)
    {
        row.spawner.Value = s;
    }

    public void FixLookAtCharacter(CharacterFacade t)
    {
        t.row.isAlive.onChange += (r, v) => StopCoroutine(nameof(LookAtCharacter));

        if (lookingAtObject)
        {
            StopLookAt();
        }
        StartCoroutine(nameof(LookAtCharacter), t.transform);
        lookingAtObject = true;
    }
    IEnumerator LookAtCharacter(Transform t)
    {
        while (true)
        {
            model.rotation = Quaternion.FromToRotation(transform.up, -(transform.position - t.position).normalized);
            model.eulerAngles = new Vector3(0f, 0f, model.eulerAngles.z);
            yield return null;
        }
    }
    public void StopLookAt()
    {
        StopCoroutine(nameof(LookAtCharacter));
        lookingAtObject = false;
    }
    public void ApplyDamage(float damage, object source)
    {
        health.ApplyDamage(damage, source);
    }
}
