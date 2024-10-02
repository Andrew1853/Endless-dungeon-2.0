using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityHFSM;
using static UnityEngine.Rendering.DebugUI.Table;
using InventorySystem;
using System.Collections.Generic;

namespace DemiurgEngine.AI {
    public class Brain : MonoBehaviour
    {
        [SerializeField] DialogCloudController _dialogController;
        [SerializeField] TMP_Text coordText;

        CharacterFacade _character;
        public enum SType
        {
            idle,
            moving,
            movingAround,
            combatEntry,
            combatExit,
            waitBeforeAttack,
            chase,
            stepBack,
            strafe,
            attack,
            combatMain,
            hunt,
            seekRabbits,
            attackRabbit,
            lootCorpses,
            executeTasks
        }
        StateMachine<SType> mainState;
        StateMachine<SType> combatState;
        StateMachine<SType> huntState;
        public ObjectMover Mover { get => _character.mover; }

        float lastCollideTime = float.MinValue;

        Vector2Int TargetPos { get => Mover.TargetPosInt; set { Mover.MoveToPos(value); } }

        float _roundDanceRadius;
        bool _rightHandDir;
        Vector2Int _targetPos;
        [SerializeField] int _meatNeeded = 10;

        [SerializeField] float _attackDistance = 1f;
        [SerializeField] Transform _attackTarget;
        int _diceRollResult = -1;
        float _distToTarget = float.MaxValue;

        Task _currentTask = null;
        [SerializeField] Item _meat;
        void Start()
        {
            _character = GetComponent<CharacterFacade>();

            mainState = new();
            State<SType> idle = new
                (
                );
            State<SType> moving = new(
            onEnter: (s) =>
            {
                if (Mover.IsActive == false)
                {
                    TargetPos = GameManager.ChunkManager.GetRandomPos();
                }
            },
            onLogic: (s) =>
            {
                //coordText.text = Mover.CurrentPosInt.ToString();
            },
            onExit: (s) => { _character.row.publicTargetPos = Vector2Int.one * int.MaxValue; Mover.Stop(); }
            );
            huntState = new StateMachine<SType>();


            combatState = new StateMachine<SType>();

            combatState.AddState(SType.combatEntry, onEnter: (s) => { _character.FixLookAtCharacter(_attackTarget.GetComponent<CharacterFacade>()); SubscribeTargetDeath(_attackTarget); _combatEnds = false; });
            combatState.AddState(SType.waitBeforeAttack, onEnter: (s) => { _diceRollResult = Random.Range(0, 6); Debug.Log("Dice roll result: " + _diceRollResult); });
            combatState.AddState(SType.chase, onLogic: (s) => TargetPos = Vector2Int.FloorToInt(_attackTarget.position));
            combatState.AddState(SType.stepBack, onEnter: (s) => Mover.MoveToPos(-Vector2Int.FloorToInt((_attackTarget.position - transform.position) * 100)),
                onExit: (s) => Mover.Stop());
            combatState.AddState(SType.strafe, onEnter: (s) => Mover.MoveAroundPosition(Vector2Int.FloorToInt(_attackTarget.position), Vector2.Distance(_attackTarget.position, transform.position), true),
                onExit: (s) => Mover.Stop());
            combatState.AddState(SType.attack, onEnter: (s) => { _character.combat.StartAttack(); Mover.Stop(); });
            combatState.AddState(SType.combatExit, onEnter: (s) => { _character.StopLookAt(); _combatEnds = true; });

            combatState.AddTransition(SType.combatEntry, SType.waitBeforeAttack);
            combatState.AddTriggerTransitionFromAny("AttackEnds", new Transition<SType>(default, SType.waitBeforeAttack));
            combatState.AddTransition(new Transition<SType>(SType.waitBeforeAttack, SType.attack, (t) => _distToTarget < _attackDistance && _diceRollResult <= 3));
            combatState.AddTransition(new Transition<SType>(SType.waitBeforeAttack, SType.stepBack, (t) => _distToTarget < _attackDistance * 5 && _diceRollResult == 4));
            combatState.AddTransition(new Transition<SType>(SType.waitBeforeAttack, SType.strafe, (t) => _distToTarget < _attackDistance * 5 && _diceRollResult == 5));

            combatState.AddTransition(new Transition<SType>(SType.waitBeforeAttack, SType.chase, (t) => _distToTarget > _attackDistance));
            combatState.AddTransition(new TransitionAfterDynamic<SType>(SType.chase, SType.waitBeforeAttack, (t) => Random.Range(3f, 8f)));
            combatState.AddTransition(new Transition<SType>(SType.chase, SType.attack,
                (t) =>
                _distToTarget < _attackDistance));
            combatState.AddTransition(new TransitionAfter<SType>(SType.stepBack, SType.waitBeforeAttack, 1.5f));
            combatState.AddTransition(new TransitionAfter<SType>(SType.strafe, SType.waitBeforeAttack, 1.7f));

            combatState.AddTriggerTransitionFromAny("TargetDead", SType.combatExit);
            combatState.AddExitTransition(SType.combatExit);

            combatState.SetStartState(SType.combatEntry);

            huntState.AddState(SType.seekRabbits, moving);
            huntState.AddState(SType.attackRabbit, combatState);
            huntState.AddState(SType.lootCorpses);

            huntState.AddTransition(SType.seekRabbits, SType.attackRabbit, (t) => SeeRabbit() && _rabbitTarget.row.isAlive.Value == false, (t) => _attackTarget = _rabbitTarget.transform);
            huntState.AddTransition(SType.seekRabbits, SType.lootCorpses, (t) => { return SeeRabbit() && _rabbitTarget.row.isAlive.Value == false && InCombat() == false; });

            mainState.AddState(SType.idle, idle);
            mainState.AddState(SType.moving, moving);
            mainState.AddState(SType.movingAround, onExit: (s) => Mover.Stop());
            mainState.AddState(SType.combatMain, combatState);
            mainState.AddState(SType.hunt, combatState);
            //mainState.AddState(SType.executeTasks, onLogic: (s) => _currentTask.Execute());



            mainState.AddTriggerTransitionFromAny("GoTo", SType.moving);
            mainState.AddTriggerTransitionFromAny("MoveAround", SType.movingAround);
            mainState.AddTriggerTransitionFromAny("Stop", SType.idle);
            mainState.AddTriggerTransitionFromAny("Fight", SType.combatMain);
            mainState.AddTriggerTransitionFromAny("Hunt", SType.hunt);

            mainState.AddTransition(new Transition<SType>(SType.combatMain, SType.idle, (t) => _combatEnds));
            mainState.AddTransition(new Transition<SType>(SType.hunt, SType.idle, (t) => _character.inventory.CountItems(_meat) >= _meatNeeded));

            _character.combat.OnAttackEnds.AddListener(
                () => 
            { if (mainState.ActiveStateName == SType.combatMain)
                {
                    combatState.Trigger("AttackEnds");
                }
            });

            mainState.SetStartState(SType.idle);
            mainState.Init();
        }
        bool _combatEnds = false;
        bool Collide()
        {
            return Mover.Collide();
        }

        void Update()
        {
            Look();
            _distToTarget = Vector2.Distance(transform.position, _attackTarget == null ? Vector2.one * float.MaxValue : _attackTarget.position);
            mainState.OnLogic();
            Debug.Log(mainState.GetActiveHierarchyPath());
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            //_dialogController.Display("Fuck!");
            //mainState.Trigger("OnCollide");
        }
        [SerializeField] List<CharacterFacade> _charsInSight = new();
        [SerializeField] List<CharacterFacade> _charsToHate = new();

        void Look()
        {
            _charsInSight.Clear();
            _charsToHate.Clear();
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 4);
            foreach (var item in hits)
            {
                CharacterFacade c;
                if (item.TryGetComponent<CharacterFacade>(out c))
                {
                    _charsInSight.Add(c);
                    if (c.name.StartsWith("Rabbit") && _charsToHate.Contains(c) == false)
                    {
                        _charsToHate.Add(c);
                    }
                }
            }
        }
        CharacterFacade _rabbitTarget;
        bool SeeRabbit()
        {
            if (_charsToHate.Count == 0)
            {
                return false;
            }
            _rabbitTarget = _charsToHate[0];
            return true;
        }
        bool InCombat()
        {
            return _charsToHate.Count == 0;
        }
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position, 4);
        }
        void SubscribeTargetDeath(Transform t)
        {
            CharacterFacade c;
            if (t.TryGetComponent(out c))
            {
                c.row.isAlive.onChange += OnTargetAliveStateChange;
            }

        }
        void UnsubscribeTargetDeath()
        {
            _attackTarget.GetComponent<CharacterFacade>().row.isAlive.onChange -= OnTargetAliveStateChange;
        }
        void OnTargetAliveStateChange(RowBase tableCell, bool value)
        {
            if (value == false)
            {
                combatState.Trigger("TargetDead");
            }
        }
        public void TriggerTransition(string triggerName)
        {
            mainState.Trigger(triggerName);
        }
        public void ApplyCommand(string command, params object[] p)
        {
            switch (command)
            {
                case "goto":
                    Vector2Int pos = (Vector2Int)p[0];
                    CharacterFacade sender;
                    if (p.Length >= 2 && p.GetType() == typeof(CharacterFacade))
                    {
                        sender = (CharacterFacade)p[1];
                    }
                    TargetPos = pos;
                    mainState.Trigger("GoTo");
                    break;
                case "line up":
                    Vector2Int targetPos;
                    if ((string)p[1] == "fast")
                    {
                        Vector2Int[] possiblePositions = new Vector2Int[GameManager.instance.group.Count];
                        Vector2Int closest = possiblePositions[0];
                        float minDist = float.MaxValue;
                        for (int i = 0; i < possiblePositions.Length; i++)
                        {
                            possiblePositions[i] = ((Vector2Int)p[0]) + Vector2Int.right * i;
                        }
                        foreach (var position in possiblePositions)
                        {
                            bool taken = false;
                            float tempDist = Vector2Int.Distance(Vector2Int.FloorToInt(transform.position), position);

                            foreach (var member in GameManager.instance.group)
                            {
                                if (member.row.publicTargetPos == position)
                                {
                                    taken = true;
                                    break;
                                }
                            }
                            if (taken == false && tempDist < minDist)
                            {
                                minDist = tempDist;
                                closest = position;
                            }
                        }
                        _character.row.publicTargetPos = closest;
                        targetPos = closest;
                    }
                    else
                    {
                        targetPos = ((Vector2Int)p[0]) + Vector2Int.right * _character.row.numInGroup;
                    }
                    TargetPos = targetPos;
                    break;
                case "round dance":
                    _targetPos = (Vector2Int)p[0];
                    _roundDanceRadius = float.Parse((string)p[1]);
                    _rightHandDir = (string)p[2] == "right";
                    mainState.Trigger("MoveAround");
                    Mover.MoveAroundPosition(_targetPos, _roundDanceRadius, _rightHandDir);
                    break;
                case "fight":
                    CharacterFacade character = (CharacterFacade)p[0];
                    _attackTarget = character.transform;
                    mainState.Trigger("Fight");
                    break;
                case "stop":
                    mainState.Trigger("Stop");
                    break;
                default:
                    break;
            }
        }
    }
    public class Task
    {
        public CharacterFacade character;
        public Brain Brain { get => character.brain; }
        public virtual bool TryCondition()
        {
            return true;
        }
        public virtual void Execute()
        {

        }
        public virtual void OnTaskCompleted()
        {

        }
    }
    public class HuntUntilEnoughMeat : Task
    {
        public int meatThreshold = 10;
        public override bool TryCondition()
        {
            return false; 
            //character.inventory.CountItems(GameManager.ItemDatabase.GetItem(ItemType.meat)) > meatThreshold;
        }
    }
}