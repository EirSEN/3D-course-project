using System.Collections;
using System.Collections.Generic;
using Unity3DCourse.Helpers;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace Unity3DCourse.Models
{
    public class EnemyMeleeModel : BaseObjectScene, IDamagable
    {
        private float _curTimeout;
        private List<WayPoint> _wayPoints;
        private int _currentWayPointId;

        [SerializeField]
        private float _currentHealth = 100f;
        [SerializeField]
        private float _dangerHealthLevel = 30f;

        private bool _isDead;
        private NavMeshAgent _agent;
        private Animator _anim;

        [SerializeField]
        private Transform _eyesTransform;
        [SerializeField]
        private float _viewDistance;
        [SerializeField]
        private float _attackDistance;
        [SerializeField]
        private float _viewAngle = 90f;

        public float AttackDistance
        {
            get { return _attackDistance; }
        }

        private GameObject _target;
        private Transform _targetHead;
        private bool _isInFOV;
        private bool _isBlocked;
        private AI_ENEMY_STATE _currentState;
        private EnemyRangeStates _rangeState;
        private float _minTimeInAttackState = 0.5f;
        private float _rotationSpeedInAttackState = 5f;
        private float _chaseTimeOut = 4f;
        private bool _canSeeTarget = false;
        private float _patrolSpeed = 2f;
        private float _chaseSpeed = 5f;
        private float _seekHealthSpeed = 6f;

        [SerializeField]
        private Transform _sparkPos1;
        [SerializeField]
        private Transform _sparkPos2;
        [SerializeField]
        private Transform _sparkPos3;

        private Weapon _weapon;

        protected override void Awake()
        {
            base.Awake();
            _isDead = false;
            _agent = GetComponent<NavMeshAgent>();
            _anim = GetComponent<Animator>();
            _weapon = GetComponentInChildren<Weapon>();
            _currentState = AI_ENEMY_STATE.IDLE;
        }

        private void Start()
        {
            _wayPoints = Main.Instance.WayPointManager.GetWayPoints(transform.position);
            _target = Main.Instance.PlayerController.gameObject;
            _targetHead = Main.Instance.PlayerCamera.transform;
            _currentWayPointId = 0;

            StartCoroutine(State_Idle());
        }

        private void Update()
        {
            if (_isDead || !_target)
                return;
            
            _canSeeTarget = IsTargetInLineOfSight();
        }

        // Аккуратно, если будет много врагов и мало вейпоинтов, может быть бесконечный цикл
        private void SetNextDestination()
        {
            int previousWayPointID = _currentWayPointId;
            do
            {
                _currentWayPointId = Random.Range(0, _wayPoints.Count);
            }
            while (_wayPoints[_currentWayPointId].CurrentEnemy);

            _wayPoints[previousWayPointID].CurrentEnemy = null;

            _agent.SetDestination(_wayPoints[_currentWayPointId].transform.position);
            _wayPoints[_currentWayPointId].CurrentEnemy = gameObject;
        }

        // Общая проверка на видимость цели
        private bool IsTargetInLineOfSight()
        {
            if (Vector3.Distance(transform.position, _target.transform.position) > _viewDistance) return false;

            if (!InFieldOfView()) return false;

            if (IsBlocked()) return false;

            return true;
        }

        // Проверка на поле зрения
        private bool InFieldOfView()
        {
            Vector3 direction = (_target.transform.position - transform.position).normalized;
            direction.y = 0;

            if (direction == Vector3.zero)
            {
                direction = transform.forward;
            }

            return Mathf.Abs(Vector3.Angle(transform.forward, direction)) < _viewAngle;
        }

        // Назначаем новую цель
        public void SetTarget(GameObject target)
        {
            _target = target;
        }

        // Проверка на видимость (заблокированность)
        private bool IsBlocked()
        {
            RaycastHit hit;

            Debug.DrawLine(_eyesTransform.position, _target.transform.position, Color.red);
            Debug.DrawLine(_eyesTransform.position, _targetHead.position, Color.red);

            if (Physics.Linecast(_eyesTransform.position, _target.transform.position, out hit))
            {
                if (hit.transform == _target.transform)
                    return false;
            }
            else if (Physics.Linecast(_eyesTransform.position, _targetHead.position, out hit))
            {
                if (hit.transform == _targetHead)
                    return false;
            }

            return true;
        }

        public void GetDamage(float damage, Vector3 dir)
        {
            if (_isDead)
                return;

            _currentHealth -= damage;

            if (_currentHealth <= 0)
            {
                StopAllCoroutines();
                StartCoroutine(State_Death());
                return;
            }

            if (_currentHealth < _dangerHealthLevel)
            {
                StopAllCoroutines();
                StartCoroutine(State_SeekHealth());
            }

            if (_currentState == AI_ENEMY_STATE.IDLE || _currentState == AI_ENEMY_STATE.PATROL)
            {
                StopAllCoroutines();
                StartCoroutine(State_Chase());
            }
        }

        // Покой
        private IEnumerator State_Idle()
        {
            _currentState = AI_ENEMY_STATE.IDLE;

            _anim.SetTrigger("Idle");
            _anim.SetFloat("Speed", 0);

            _agent.isStopped = true;

            while (_currentState == AI_ENEMY_STATE.IDLE)
            {
                if (_canSeeTarget)
                {
                    StartCoroutine(State_Chase());
                    _curTimeout = 0;

                    yield break;
                }

                _curTimeout += Time.deltaTime * (int)_rangeState;

                if (_curTimeout > _wayPoints[_currentWayPointId].WaitTime)
                {
                    _curTimeout = 0;
                    StartCoroutine(State_Patrol());
                    yield break;
                }

                yield return Extensions.WaitForFrames((int)_rangeState);
            }
        }

        // Патрулирование
        private IEnumerator State_Patrol()
        {
            _currentState = AI_ENEMY_STATE.PATROL;

            _agent.isStopped = false;
            _agent.speed = _patrolSpeed;

            SetNextDestination();
            _anim.SetTrigger("Patrol");

            while (_currentState == AI_ENEMY_STATE.PATROL)
            {
                _anim.SetFloat("Speed", _agent.velocity.magnitude);
                if (_canSeeTarget)
                {
                    StartCoroutine(State_Chase());
                    yield break;
                }

                if (Vector3.Distance(transform.position, _wayPoints[_currentWayPointId].transform.position) <= _agent.stoppingDistance)
                {
                    StartCoroutine(State_Idle());
                    yield break;
                }

                yield return Extensions.WaitForFrames((int)_rangeState);
            }
        }

        // Преследование
        private IEnumerator State_Chase()
        {
            _currentState = AI_ENEMY_STATE.CHASE;

            _agent.isStopped = false;
            _agent.speed = _chaseSpeed;



            if (!_anim.GetCurrentAnimatorStateInfo(0).IsName("Chase") && !_anim.IsInTransition(0))
            {
                _anim.SetTrigger("Chase");
            }

            while (_currentState == AI_ENEMY_STATE.CHASE)
            {
                _anim.SetFloat("Speed", _agent.velocity.magnitude);
                _agent.SetDestination(_target.transform.position);

                if (!_canSeeTarget)
                {
                    float ElapsedTime = 0f;

                    while (true)
                    {
                        ElapsedTime += Time.deltaTime * (int)EnemyRangeStates.COMBAT;

                        _agent.SetDestination(_target.transform.position);
                        _anim.SetFloat("Speed", _agent.velocity.magnitude);

                        yield return Extensions.WaitForFrames((int)EnemyRangeStates.COMBAT);

                        if (_canSeeTarget)
                        {
                            break;
                        }

                        if (ElapsedTime >= _chaseTimeOut && !_canSeeTarget)
                        {
                            StartCoroutine(State_Idle());
                            yield break;

                        }
                    }
                }


                if (Vector3.Distance(transform.position, _target.transform.position) <= _attackDistance)
                {
                    StartCoroutine(State_Attack());
                    yield break;
                }

                yield return Extensions.WaitForFrames((int)EnemyRangeStates.COMBAT);
            }
        }

        // Атака
        private IEnumerator State_Attack()
        {
            _currentState = AI_ENEMY_STATE.ATTACK;

            _agent.isStopped = true;
            float ElapsedTime = 0f;
            _anim.ResetTrigger("Chase");
            while (_currentState == AI_ENEMY_STATE.ATTACK)
            {
                ElapsedTime += Time.deltaTime;
                _anim.SetFloat("Speed", 0);
                if ((!_canSeeTarget || Vector3.Distance(transform.position, _target.transform.position) > _attackDistance) 
                    && ElapsedTime > _minTimeInAttackState)
                {
                    StartCoroutine(State_Chase());
                    yield break;
                }

                Quaternion targetRotation = Quaternion.LookRotation(_target.transform.position - transform.position);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeedInAttackState * Time.deltaTime);
                _weapon.Fire();

                yield return Extensions.WaitForFrames((int)EnemyRangeStates.COMBAT); ;
            }
        }

        // Используется для вызова из анимации
        public void LaunchAttack()
        {
            Fist fist = _weapon as Fist;
            if (fist)
            {
                fist.LaunchAttack();
            }
        }

        //Поиск аптечки
        private IEnumerator State_SeekHealth()
        {
            _currentState = AI_ENEMY_STATE.SEEKHEALTH;

            _agent.isStopped = false;
            _agent.speed = _seekHealthSpeed;
            _anim.ResetTrigger("Attack");
            _anim.ResetTrigger("Chase");
            _anim.SetTrigger("Seek Health");

            HealthPack HR = null;

            while (_currentState == AI_ENEMY_STATE.SEEKHEALTH)
            {
                if (HR == null)
                    HR = Main.Instance.HealthPackManager.GetClosestHealthPack(transform);
                if (HR != null)
                    _agent.SetDestination(HR.transform.position);

                _anim.SetFloat("Speed", _agent.velocity.magnitude);

                if (HR == null || _currentHealth > _dangerHealthLevel)
                {
                    StartCoroutine(State_Idle());
                    yield break;
                }
                yield return Extensions.WaitForFrames((int)_rangeState);
            }
        }

        private IEnumerator State_Death()
        {
            _currentState = AI_ENEMY_STATE.DEATH;
            _isDead = true;
            _agent.enabled = false;
            _wayPoints[_currentWayPointId].CurrentEnemy = null;
            _anim.ResetTrigger("Seek Health");
            _anim.SetTrigger("Death");
            _anim.SetFloat("Speed", 0f);
            Main.Instance.EnemyAIManager.RemoveEnemy(this);
            
            Destroy(gameObject, 3.33f);

            Main.Instance.VFXManager.PlayRobotSparksEffect(_sparkPos1.position, _sparkPos1.forward);
            yield return new WaitForSeconds(1.1f);
            Main.Instance.VFXManager.PlayRobotSparksEffect(_sparkPos2.position, _sparkPos2.forward);
            yield return new WaitForSeconds(1.1f);
            Main.Instance.VFXManager.PlayRobotSparksEffect(_sparkPos3.position, _sparkPos3.forward);
            yield return new WaitForSeconds(0.8f);

            Main.Instance.VFXManager.PlayRobotExplosionEffect(transform.position + Vector3.up);
            
        }

        public void SetRangeState(EnemyRangeStates state)
        {
            _rangeState = state;
        }

    }
}

#region OldVersion
//float dist = Vector3.Distance(transform.position, _target.transform.position);

//bool inViewDistance = dist < _viewDistance;

//if (inViewDistance)
//{
//    _isInFOV = InFieldOfView();
//    if (_isInFOV)
//    {
//        _isBlocked = IsBlocked();
//        if (!_isBlocked && dist < _attackDistance)
//        {
//            _weapon.Fire();
//            _agent.SetDestination(_target.transform.position); // Убрать для отключения стрельбы на ходу
//        }
//        else if (!_isBlocked)
//        {
//            _agent.SetDestination(_target.transform.position);
//        }
//    }
//}

//if ((!inViewDistance || !_isInFOV || _isBlocked) && _wayPoints.Count >= 2)
//{
//    _agent.SetDestination(_wayPoints[_currentWayPointId].transform.position);

//    if (_agent.remainingDistance <= _agent.stoppingDistance)
//    {
//        _curTimeout += Time.deltaTime;
//        if (_curTimeout > _wayPoints[_currentWayPointId].WaitTime)
//        {
//            _curTimeout = 0;
//            _wayPoints[_currentWayPointId].CurrentEnemy = null;
//            SetNextDestination();
//        }
//    }
//}
#endregion