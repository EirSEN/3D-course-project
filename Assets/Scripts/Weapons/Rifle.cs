using System.Collections;
using System.Collections.Generic;
using Unity3DCourse.View;
using UnityEngine;
using UnityEngine.UI;

namespace Unity3DCourse
{
    public class Rifle : Weapon
    {
        [SerializeField]
        private float _force = 350f;
        [SerializeField]
        private GameObject _bulletPrefab;
        [SerializeField]
        private Transform _firePoint;
        [SerializeField]
        private float _timeout = 1f;
        private float _lastShotTime;

        [SerializeField]
        private GameObject _muzzleFlash;
        [SerializeField]
        private GameObject _bulletFXPrefab;

        [Header("Reload Section")]
        private const float _maxClipSize = 10f; // максимальная емкость обоймы
        private float _currentClipSize; // текущая емкость обоймы
        private bool _reloading;
        private float _reloadTime = 2f;
        private AmmoView _ammoView;
        private bool _isSelectedWeapon;

        private Animator _anim;
        private bool _isScoped = false;
        private RifleView _rifleView;


        protected override void Awake()
        {
            base.Awake();

            _anim = GetComponent<Animator>();
        }

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

        private void Start()
        {
            Main.Instance.ObjectPoolManager.InitializePool<RifleBullet>(_bulletPrefab, _timeout, _bulletPrefab.GetComponent<RifleBullet>().TimeToDestruct);
            Main.Instance.ObjectPoolManager.InitializeBulletFXPool<RifleBulletExplEffect>(_bulletFXPrefab, _timeout, _bulletPrefab.GetComponent<RifleBullet>().TimeToDestruct);

            _isSelectedWeapon = false;
            _ammoView = FindObjectOfType<AmmoView>();
            _rifleView = FindObjectOfType<RifleView>();
            CurrentClipSize = _maxClipSize;
        }

        public override void SelectWeapon()
        {
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
            _isSelectedWeapon = false;
            StopAllCoroutines();

            if (_isScoped)
            {
                AlternativeFire();
            }
            IsVisible = false;
            _reloading = false;
            _ammoView.CancelDisplayReload();
        }

        public override void Fire()
        {
            if (_reloading) return;

            if (Time.time - _lastShotTime < _timeout)
                return;

            FireWithObjectPool();

            _lastShotTime = Time.time;
        }

        // Стрельба с использование пула объектов
        private void FireWithObjectPool()
        {
            CurrentClipSize--;
            RifleBullet t = Main.Instance.ObjectPoolManager.GetInactiveAmmunition<RifleBullet>();
            t.transform.position = _firePoint.position;
            t.transform.rotation = _firePoint.rotation;
            t.Initialize(_firePoint.forward * _force);

            if (!_isScoped)
            {
                _anim.SetTrigger("Fire");
            }

            StartCoroutine(MuzzleFlash());

            if (CurrentClipSize <= 0)
            {
                StartCoroutine(Reload());
            }
        }

        public override void ReloadWeapon()
        {
            if (_currentClipSize < _maxClipSize && !_reloading)
            {
                StartCoroutine(Reload());
            }
        }

        private IEnumerator MuzzleFlash()
        {
            _muzzleFlash.SetActive(true);
            yield return new WaitForSeconds(0.6f);
            _muzzleFlash.SetActive(false);
        }

        private IEnumerator Reload()
        {
            if (_isScoped)
            {
                AlternativeFire();
            }
            
            _reloading = true;
            StartCoroutine(_ammoView.DisplayReload(_reloadTime));
            yield return new WaitForSeconds(_reloadTime);
            CurrentClipSize = _maxClipSize;
            _reloading = false;
        }

        public override void AlternativeFire()
        {
            if (_reloading) return;

            _isScoped = !_isScoped;
            _anim.SetBool("Scoped", _isScoped);

            if (_isScoped)
            {
                StartCoroutine(Scoping());
            }
            else
            {
                UnScoping();
            }

        }

        private IEnumerator Scoping()
        {
            yield return new WaitForSeconds(0.15f);
            IsVisible = !_isScoped;
            _rifleView.TurnScope(_isScoped);
        }

        private void UnScoping()
        {
            _rifleView.TurnScope(_isScoped);
            IsVisible = !_isScoped;
        }
    }
}