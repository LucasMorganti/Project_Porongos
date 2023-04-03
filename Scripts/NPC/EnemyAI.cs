using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private EnemyView enemyView;
    [SerializeField] private ObjectThrow obj;
    [SerializeField] private Ability ab;

    public enum NpcLevel
    {
        Easy, 
        Normal,
        Hard
    }
    
    public enum FsmStates
    {
        Idle,
        Patrol,
        Chase,
        Attack,
        Search
    }

    [Header("NPC Level")] 
    public NpcLevel npcLevel = NpcLevel.Easy;

    [Header("States")] 
    public FsmStates currentState = FsmStates.Patrol;

    [Header("Visibility Check")] 
    [SerializeField] private bool searching;
    [SerializeField] private bool spotted;

    [Header("Player Detection")] 
    [SerializeField] private Image detectionBar;
    [SerializeField] private GameObject apple;
    private float _barValue;
    private bool _detecting;

    [Header("General Values")] 
    [SerializeField] private Transform target;
    [SerializeField] private float maxDetectionValue = 20;
    [SerializeField] private float rotSpeed = 10;
    private Transform _myTransform;
    private Vector3 _direction;
    private float _distance;
    private float _timer;

    [Header("Patrol Values")] 
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private float changeToChase = 10;
    private NavMeshAgent _enemyAgent;
    private int _currentWaypoint;
    private float _timerLook;

    [Header("Chase Values")] 
    [SerializeField] private float patrolToChase = 15;
    [SerializeField] private float startAttacking = 5;

    [Header("Attack Values")] 
    [SerializeField] private Transform muzzle;
    [SerializeField] private Rigidbody bullet;
    [SerializeField] private float initialBulletForce = 1000;
    [SerializeField] private float shotFrequency = 5;
    [SerializeField] private int maxShots = 3;
    private int _numberOfShots;

    [Header("Search Values")] 
    [SerializeField] private float toSearch;
    private Vector3 _objDirection;
    private float _objDistance;
    private float _searchTimer;

    private Animator _anim;

    public void Start()
    {
        _enemyAgent = GetComponent<NavMeshAgent>();
        _enemyAgent.speed = 2;
        _currentWaypoint = 0;
        _enemyAgent.SetDestination(waypoints[_currentWaypoint].position);

        _timer = 0;
        _distance = 0;
        _myTransform = GetComponent<Transform>();

        if (npcLevel == NpcLevel.Easy)
        {
            maxDetectionValue = 20;
            
        }
        else if (npcLevel == NpcLevel.Normal)
        {
            maxDetectionValue = 15;
        }
        else if (npcLevel == NpcLevel.Hard)
        {
            maxDetectionValue = 10;
        }

        _barValue = 0;
        detectionBar.fillAmount = _barValue / maxDetectionValue;

        _anim = GetComponent<Animator>();
    }

    public void Update()
    {
        if (GameObject.FindWithTag("Apple"))
        {
            obj = GameObject.FindWithTag("Apple").GetComponent<ObjectThrow>();
        }

        var position = _myTransform.position;

        _direction = target.position - position;
        _distance = _direction.magnitude;

        _objDirection = obj.newWaypoint.position - position;
        _objDistance = Vector3.Distance(position, obj.contact.point);
        
        if (enemyView.canSeePlayer)
        {
            if (npcLevel == NpcLevel.Easy)
            {
                _barValue += Time.deltaTime;
                if (_barValue >= maxDetectionValue)
                {
                    _barValue = maxDetectionValue;
                    spotted = true;
                    _detecting = true;
                    
                    
                }
            }
            else if (npcLevel == NpcLevel.Normal)
            {
                _barValue += Time.deltaTime * 1.5f;
                if (_barValue >= maxDetectionValue)
                {
                    _barValue = maxDetectionValue;
                    spotted = true;
                    _detecting = true;
                    
                    
                }
            }
            else if (npcLevel == NpcLevel.Hard)
            {
                _barValue += Time.deltaTime * 2f;
                if (_barValue >= maxDetectionValue)
                {
                    _barValue = maxDetectionValue;
                    spotted = true;
                    _detecting = true;
                    
                    
                }
            }

            detectionBar.fillAmount = _barValue / maxDetectionValue;
        }
        else
        {
            _barValue -= Time.deltaTime;
            if (_detecting && _barValue < maxDetectionValue)
            {
                spotted = false;
                //searching = true;
            }
            /*else
            {
                searching = false;
            }*/

            if (_barValue <= 0)
            {
                _barValue = 0;
                _detecting = false;
                spotted = false;
            }

            detectionBar.fillAmount = _barValue / maxDetectionValue;
        }

        /*if (player.amountOfNoiseMade >= 1 && _distance <= 30)
        {
            transform.LookAt(target);
            _enemyAgent.isStopped = true;

            _timer += Time.deltaTime;
            if (_timer >= 5 && !spotted)
            {
                transform.LookAt(waypoints[_currentWaypoint]);
                _enemyAgent.isStopped = false;

                _timer = 0;
            }
        }*/

        if (_enemyAgent.isStopped)
        {
            _anim.Play("Idle");
        }
        else
        {
            _anim.Play("Walk");
        }

        if (npcLevel == NpcLevel.Easy)
        {
            _enemyAgent.speed = 3.5f;
        }
        else if (npcLevel == NpcLevel.Normal)
        {
            _enemyAgent.speed = 4f;
        }
        else if (npcLevel == NpcLevel.Hard)
        {
            _enemyAgent.speed = 4.5f;
        }

        #region StatesUpdate

        if (currentState == FsmStates.Idle)
        {
            IdleState();
        }
        else if (currentState == FsmStates.Patrol)
        {
            PatrolState();
        }
        else if (currentState == FsmStates.Search)
        {
            SearchState();
        }
        else if (currentState == FsmStates.Chase)
        {
            ChaseState();
        }
        else if (currentState == FsmStates.Attack)
        {
            AttackState();
        }

        #endregion
    }

    private void IdleState()
    {
        currentState = FsmStates.Idle;

        _enemyAgent.enabled = false;
        _enemyAgent.isStopped = true;
    }

    private void PatrolState()
    {
        if (spotted)
        {
            currentState = FsmStates.Chase;
            _enemyAgent.SetDestination(target.position);
            
            return;
        }

        if (obj.hitGround && _objDistance <= 20)
        {
            currentState = FsmStates.Search;
            _enemyAgent.SetDestination(obj.contact.point);

            return;
        }

        if (_enemyAgent.remainingDistance != 0) return;
        _enemyAgent.isStopped = true;

        _timerLook += Time.deltaTime;
        if (npcLevel == NpcLevel.Easy)
        {
            if (_timerLook >= 5)
            {
                _timerLook = 0;
                _enemyAgent.isStopped = false;
                _currentWaypoint++;
            }
        }
        else if (npcLevel == NpcLevel.Normal)
        {
            if (_timerLook >= 3)
            {
                _timerLook = 0;
                _enemyAgent.isStopped = false;
                _currentWaypoint++;
            }
        }
        else if (npcLevel == NpcLevel.Hard)
        {
            if (_timerLook >= 1)
            {
                _timerLook = 0;
                _enemyAgent.isStopped = false;
                _currentWaypoint++;
            }
        }

        if (_currentWaypoint >= waypoints.Length)
        {
            _currentWaypoint = 0;
        }

        _enemyAgent.SetDestination(waypoints[_currentWaypoint].position);
    }

    private void SearchState()
    {
        _enemyAgent.isStopped = false;

        if (_objDistance <= 2)
        {
            _enemyAgent.isStopped = true;
            
            _searchTimer += Time.deltaTime;
            if (npcLevel == NpcLevel.Easy)
            {
                if (_searchTimer >= 5)
                {
                    currentState = FsmStates.Patrol;
                    _enemyAgent.isStopped = false;
                    _currentWaypoint++;

                    _searchTimer = 0;
                    obj.hitGround = false;
                
                    _enemyAgent.SetDestination(waypoints[_currentWaypoint].position);

                    return;
                }
            }
            else if (npcLevel == NpcLevel.Normal)
            {
                if (_searchTimer >= 3)
                {
                    currentState = FsmStates.Patrol;
                    _enemyAgent.isStopped = false;
                    _currentWaypoint++;

                    _searchTimer = 0;
                    obj.hitGround = false;
                
                    _enemyAgent.SetDestination(waypoints[_currentWaypoint].position);

                    return;
                }
            }
            else if (npcLevel == NpcLevel.Hard)
            {
                if (_searchTimer >= 2.5f)
                {
                    currentState = FsmStates.Patrol;
                    _enemyAgent.isStopped = false;
                    _currentWaypoint++;

                    _searchTimer = 0;
                    obj.hitGround = false;
                
                    _enemyAgent.SetDestination(waypoints[_currentWaypoint].position);

                    return;
                }
            }
        }

        _enemyAgent.SetDestination(obj.contact.point);
    }

    private void ChaseState()
    {
        if (!spotted && _distance >= 10 || ab.usingAbility)
        {
            currentState = FsmStates.Patrol;
            _enemyAgent.SetDestination(waypoints[_currentWaypoint].position);
            
            return;
        }

        /*if (_distance <= 2)
        {
            print("you are dead");
        }*/

        if (spotted && _distance <= 6)
        {
            currentState = FsmStates.Attack;

            _enemyAgent.isStopped = true;
            _enemyAgent.enabled = false;

            _timer = 0;
            _numberOfShots = 0;
            
            return;
        }

        _enemyAgent.SetDestination(target.position);
    }

    private void AttackState()
    {
        _myTransform.rotation = Quaternion.Slerp(_myTransform.rotation, Quaternion.LookRotation(_direction), 
            rotSpeed * Time.deltaTime);
        _myTransform.eulerAngles = new Vector3(0, _myTransform.eulerAngles.y, 0);

        _timer += Time.deltaTime;
        if (_timer >= shotFrequency)
        {
            _timer = 0;

            Rigidbody b = Instantiate(bullet, muzzle.position, muzzle.rotation);
            b.AddForce(muzzle.forward * initialBulletForce);
            Destroy(b.gameObject, 2);

            _numberOfShots++;
            if (_numberOfShots < maxShots) return;
            if (_distance <= startAttacking)
            {
                _numberOfShots = 0;
                return;
            }

            if (_distance > 6)
            {
                currentState = FsmStates.Chase;

                _enemyAgent.isStopped = false;
                _enemyAgent.enabled = true;
                _enemyAgent.SetDestination(target.position);

                _timer = 0;
                
                return;
            }

            if (enemyView.canSeePlayer == false && !spotted || ab.usingAbility)
            {
                currentState = FsmStates.Patrol;

                _enemyAgent.isStopped = false;
                _enemyAgent.enabled = true;

                _enemyAgent.SetDestination(waypoints[_currentWaypoint].position);
            }
        }
    }
}
