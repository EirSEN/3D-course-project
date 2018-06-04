using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Unity3DCourse
{
    public class RifleBullet : Ammunition
    {
        [Header("Base section")]
        [SerializeField]
        private float _timeToDestruct = 5f;
        [SerializeField]
        private float _defaultStartDamage = 50f;
        private float _maxDamage; // Значение, больше которого не может упать урон пули.

        [Header("Multiplier section")]
        [SerializeField]
        private float _damageOverTimeMultiplier = 0.25f;
        [SerializeField]
        private float _multilpyEverySec = 0.25f;
        private WaitForSeconds _damageMultiplierRate;
        private float _currentDamage;
        [SerializeField]
        private LayerMask _bulletLayerMask;

        private bool _isHitted;
        private float _speed;
        private TrailRenderer _trailRenderer;

        public float TimeToDestruct
        {
            get
            {
                return _timeToDestruct;
            }
        }

        public void Initialize(Vector3 force, float damageMult = 1f)
        {
            gameObject.SetActive(true); // Для пула объектов
            Awake();

            if (!_trailRenderer)
                _trailRenderer = GetComponent<TrailRenderer>();

            CancelInvoke();
            _damageMultiplierRate = new WaitForSeconds(_multilpyEverySec);

            _currentDamage = _defaultStartDamage * damageMult;
            _maxDamage = _currentDamage * 2;
            _isHitted = false;

            transform.forward = force;
            _speed = force.magnitude;

            Invoke("DisableObject", _timeToDestruct);
            StartCoroutine(EnableTrailRenderer());
            StartCoroutine(DamageIncrease());
        }

        private void FixedUpdate()
        {
            if (_isHitted) return;

            Vector3 endPositionOfThisUpdate = transform.position + transform.forward * _speed * Time.fixedDeltaTime;

            RaycastHit hit;
            if (Physics.Linecast(transform.position, endPositionOfThisUpdate, out hit, _bulletLayerMask))
            {
                _isHitted = true;
                transform.position = hit.point;
                StartExplosionEffect();
                SetDamage(hit.transform.gameObject.GetComponent<IDamagable>());
                Invoke("DisableObject", 0.3f);
            }
            else
            {
                transform.position = endPositionOfThisUpdate;
            }
        }

        private void SetDamage(IDamagable target)
        {
            if (target == null)
                return;

            target.GetDamage(_currentDamage, transform.forward);
        }

        private void DisableObject()
        {
            _trailRenderer.time = 0;
            StopAllCoroutines();
            gameObject.SetActive(false);
        }

        // Увеличение урона со временем.
        // Можно реализовать через дистанцию, но мне кажется, что так будет дороже по ресурсам
        // проверять каждый раз расстояние от места выстрела до текущей позиции
        private IEnumerator DamageIncrease()
        {
            yield return _damageMultiplierRate; // Задержка перед увеличение урона
            while (_currentDamage < _maxDamage)
            {
                _currentDamage += _defaultStartDamage * _damageOverTimeMultiplier;
                yield return _damageMultiplierRate;
            }
        }

        private IEnumerator EnableTrailRenderer()
        {
            yield return null;
            _trailRenderer.time = 0.12f;
        }

        private void StartExplosionEffect()
        {
            RifleBulletExplEffect effect = Main.Instance.ObjectPoolManager.GetInactiveBulletFX<RifleBulletExplEffect>();
            effect.transform.position = transform.position;
            effect.transform.forward = -transform.forward;
            effect.gameObject.SetActive(true);
        }
    }
}