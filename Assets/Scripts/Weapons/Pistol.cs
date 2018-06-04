using System.Collections;
using System.Collections.Generic;
using Unity3DCourse.View;
using UnityEngine;

namespace Unity3DCourse
{
    public class Pistol : Weapon
    {
        [SerializeField]
        private float _force = 150f;
        [SerializeField]
        private GameObject _bulletPrefab;
        [SerializeField]
        private Transform _firePoint;
        [SerializeField]
        private float _timeout = 0.3f;
        private float _lastShotTime;

        private ParticleSystem[] _muzzle;
        private Animator _anim;
        [SerializeField]
        private GameObject _bulletFXPrefab;

        [Header("Reload Section")]
        private const float _maxClipSize = 12f; // максимальная емкость обоймы
        private float _currentClipSize; // текущая емкость обоймы
        private bool _reloading;
        private float _reloadTime = 1.2f;
        private AmmoView _ammoView;
        private bool _isSelectedWeapon;

        [SerializeField]
        private bool _isEnemyWeapon = false; // Этот пистолет противника или игрока


        public float CurrentClipSize
        {
            get { return _currentClipSize; }
            private set
            {
                _currentClipSize = value;
                if (_isSelectedWeapon)
                {
                    _ammoView.DisplayCurrentAmmo(_currentClipSize);
                }
            }
        }

        public float MaxClipSize
        {
            get { return _maxClipSize; }
        }

        protected override void Awake()
        {
            base.Awake();

            _muzzle = GetComponentsInChildren<ParticleSystem>();
            _anim = GetComponent<Animator>();
        }

        private void Start()
        {
            Main.Instance.ObjectPoolManager.InitializePool<PistolBullet>(_bulletPrefab, _timeout, _bulletPrefab.GetComponent<PistolBullet>().TimeToDestruct);
            Main.Instance.ObjectPoolManager.InitializeBulletFXPool<PistolBulletExplEffect>(_bulletFXPrefab, _timeout, _bulletPrefab.GetComponent<PistolBullet>().TimeToDestruct);

            if (_isEnemyWeapon) return;

            _isSelectedWeapon = true;
            _ammoView = FindObjectOfType<AmmoView>();
            CurrentClipSize = _maxClipSize;
            _ammoView.DisplayMaxAmmo(_maxClipSize); // Только для пистолета (первое оружие)
        }

        public override void Fire()
        {
            if (_reloading) return;

            if (Time.time - _lastShotTime < _timeout)
                return;

            
            PistolBullet t = Main.Instance.ObjectPoolManager.GetInactiveAmmunition<PistolBullet>();
            t.transform.position = _firePoint.position;
            t.transform.rotation = _firePoint.rotation;
            t.Initialize(_firePoint.forward * _force);

            if (!_isEnemyWeapon)
            {
                _anim.SetTrigger("Fire");
                foreach (var effect in _muzzle)
                {
                    effect.Play(true);
                }
                CurrentClipSize--;
                if (CurrentClipSize <= 0)
                {
                    StartCoroutine(Reload());
                }
            }

            _lastShotTime = Time.time;
        }

        public override void SelectWeapon()
        {
            if (_isEnemyWeapon) return;

            _isSelectedWeapon = true;
            _ammoView.DisplayCurrentAmmo(_currentClipSize);
            _ammoView.DisplayMaxAmmo(_maxClipSize);
            if (CurrentClipSize <= 0)
            {
                StartCoroutine(Reload());
            }
        }

        public override void DeselectWeapon()
        {
            if (_isEnemyWeapon) return;

            _isSelectedWeapon = false;
            StopAllCoroutines();
            _reloading = false;
            _ammoView.CancelDisplayReload();
        }

        public override void ReloadWeapon()
        {
            if (_isEnemyWeapon) return;

            if (_currentClipSize < _maxClipSize && !_reloading)
            {
                StartCoroutine(Reload());
            }
        } 

        private IEnumerator Reload()
        {
            if (_isEnemyWeapon) yield break;

            yield return new WaitForSeconds(0.15f);
            StartCoroutine(_ammoView.DisplayReload(_reloadTime));
            _reloading = true;
            yield return new WaitForSeconds(_reloadTime);
            CurrentClipSize = _maxClipSize;
            _reloading = false;
        }

        public override void AlternativeFire()
        {
            return;
        }
    }
}