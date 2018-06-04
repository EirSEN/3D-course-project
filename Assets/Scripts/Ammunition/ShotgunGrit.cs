using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unity3DCourse
{
    public class ShotgunGrit : Ammunition
    {
        [SerializeField]
        private float _timeToDestruct = 2f;
        [SerializeField]
        private float _defaultStartDamage = 5f;

        [Header("Multiplier section")]
        [SerializeField]
        private float _damageOverTimeDivider = 0.15f; // Насколько будет снинижаться урон от начального
        [SerializeField]
        private float _decreaseEverySec = 0.15f; // Через какой промежуток времени будет снижаться урон
        private WaitForSeconds _damageDecreaseRate; // Параметр для корутины
        private float _currentDamage; // Текущий урон (изменяется со временем)
        private float _minDamage; // Значение, меньше которого не может упать урон пули.

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
            _damageDecreaseRate = new WaitForSeconds(_decreaseEverySec);

            _currentDamage = _defaultStartDamage * damageMult;
            _minDamage = _currentDamage / 2;
            _isHitted = false;

            transform.forward = force;
            _speed = force.magnitude;

            Invoke("DisableObject", _timeToDestruct);

            StartCoroutine(EnableTrailRenderer());
            StartCoroutine(DecreaseDamage());
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
                Invoke("DisableObject", 0.2f);
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

        // Уменьшение урона со временем.
        // Можно реализовать через дистанцию, но мне кажется, что так будет дороже по ресурсам
        // проверять каждый раз расстояние от места выстрела до текущей позиции
        private IEnumerator DecreaseDamage()
        {
            yield return _damageDecreaseRate; // задержка перед падением урона
            while (_currentDamage > _minDamage)
            {
                _currentDamage -= _defaultStartDamage * _damageOverTimeDivider;
                yield return _damageDecreaseRate;
            }
        }

        private void DisableObject()
        {
            _trailRenderer.time = 0;
            StopAllCoroutines();
            gameObject.SetActive(false);
        }

        private IEnumerator EnableTrailRenderer()
        {
            yield return null;
            _trailRenderer.time = 0.12f;
        }

        private void StartExplosionEffect()
        {
            ShotgunGritExplEffect effect = Main.Instance.ObjectPoolManager.GetInactiveBulletFX<ShotgunGritExplEffect>();
            effect.transform.position = transform.position;
            effect.transform.forward = -transform.forward;
            effect.gameObject.SetActive(true);
        }
    }
}