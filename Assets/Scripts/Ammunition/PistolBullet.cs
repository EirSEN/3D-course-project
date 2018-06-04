using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unity3DCourse
{
    public class PistolBullet : Ammunition
    {
        [SerializeField]
        private float _timeToDestruct = 3f;
        [SerializeField]
        private readonly float _defaultStartDamage = 15f;

        private float _currentDamage;
        private float _speed;
        private TrailRenderer _trailRenderer;
        private bool _isHitted;
        [SerializeField]
        private LayerMask _bulletLayerMask;

        public float TimeToDestruct
        {
            get
            {
                return _timeToDestruct;
            }
        }

        public void Initialize(Vector3 force, float damageMult = 1f)
        {
            gameObject.SetActive(true);
            Awake();
            if (!_trailRenderer)
            {
                _trailRenderer = GetComponent<TrailRenderer>();
            }

            CancelInvoke();
            _currentDamage = _defaultStartDamage * damageMult;
            _isHitted = false;

            transform.forward = force;
            _speed = force.magnitude;

            Invoke("DisableObject", _timeToDestruct);
            StartCoroutine(EnableTrailRenderer());
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
            PistolBulletExplEffect effect = Main.Instance.ObjectPoolManager.GetInactiveBulletFX<PistolBulletExplEffect>();
            effect.transform.position = transform.position;
            effect.transform.forward = -transform.forward;
            effect.gameObject.SetActive(true);
        }
    }
}