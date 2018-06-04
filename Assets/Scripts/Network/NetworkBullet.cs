using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unity3DCourse {
    public class NetworkBullet : MonoBehaviour
    {
        [SerializeField]
        private float _timeToDestruct = 3f;
        [SerializeField]
        private readonly float _defaultStartDamage = 15f;

        private float _currentDamage;
        private float _speed = 100f;
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

        private void Awake()
        {
            Destroy(gameObject, _timeToDestruct);
            _currentDamage = _defaultStartDamage;
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
                SetDamage(hit.transform.gameObject.GetComponent<IDamagable>());
                Destroy(gameObject, 0.1f);
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
    }
}