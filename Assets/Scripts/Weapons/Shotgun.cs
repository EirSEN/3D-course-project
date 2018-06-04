using System.Collections;
using System.Collections.Generic;
using Unity3DCourse.View;
using UnityEngine;

namespace Unity3DCourse
{
    public class Shotgun : Weapon
    {
        [SerializeField]
        private float _force = 300f;
        [SerializeField]
        private GameObject _bulletPrefab;
        [SerializeField]
        private Transform _firePoint;
        [SerializeField]
        private float _timeout = 1.2f;
        private float _lastShotTime;
        [SerializeField]
        private int _gritCount = 10;

        [Header("Reload Section")]
        private const float _maxClipSize = 7f; // максимальная емкость обоймы
        private float _currentClipSize; // текущая емкость обоймы
        private bool _reloading;
        private float _reloadTime = 2.5f;
        private AmmoView _ammoView;
        private bool _isSelectedWeapon;

        private Animator _anim;
        [SerializeField]
        private GameObject _muzzleFlash;
        [SerializeField]
        private GameObject _bulletFXPrefab;


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

            _anim = GetComponent<Animator>();
        }

        private void Start()
        {
            Main.Instance.ObjectPoolManager.InitializePool<ShotgunGrit>(_bulletPrefab, _timeout, _bulletPrefab.GetComponent<ShotgunGrit>().TimeToDestruct * _gritCount * 1.5f);
            Main.Instance.ObjectPoolManager.InitializeBulletFXPool<ShotgunGritExplEffect>(_bulletFXPrefab, _timeout, _bulletPrefab.GetComponent<ShotgunGrit>().TimeToDestruct * _gritCount * 1.5f);

            _isSelectedWeapon = false;
            _ammoView = FindObjectOfType<AmmoView>();
            CurrentClipSize = _maxClipSize;
        }

        public override void Fire()
        {
            if (_reloading) return;

            if (Time.time - _lastShotTime < _timeout)
                return;

            StartCoroutine(MuzzleFlash());
            _anim.SetTrigger("Fire");

            CurrentClipSize--;
            for (int i = 0; i < _gritCount; i++)
            {
                Vector3 randomStartPosition = Random.insideUnitCircle / 5;

                float randomX = Random.Range(-0.05f, 0.05f);
                float randomY = Random.Range(-0.05f, 0.05f);
                float randomZ = Random.Range(-0.05f, 0.05f);

                Vector3 randomDirection = new Vector3(_firePoint.forward.x + randomX, _firePoint.forward.y + randomY, _firePoint.forward.z + randomZ);

                ShotgunGrit t = Main.Instance.ObjectPoolManager.GetInactiveAmmunition<ShotgunGrit>();
                t.transform.position = _firePoint.position + randomStartPosition;
                t.transform.rotation = _firePoint.rotation;
                t.Initialize(randomDirection * _force);
            }
            if (CurrentClipSize <= 0)
            {
                StartCoroutine(Reload());
            }

            _lastShotTime = Time.time;
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
            _reloading = false;
            _ammoView.CancelDisplayReload();
        }

        public override void ReloadWeapon()
        {
            if (_currentClipSize < _maxClipSize && !_reloading)
            {
                StartCoroutine(Reload());
            }
        }

        private IEnumerator Reload()
        {
            _reloading = true;
            StartCoroutine(_ammoView.DisplayReload(_reloadTime));
            yield return new WaitForSeconds(_reloadTime);
            CurrentClipSize = _maxClipSize;
            _reloading = false;
        }

        private IEnumerator MuzzleFlash()
        {
            _muzzleFlash.SetActive(true);
            yield return new WaitForSeconds(0.6f);
            _muzzleFlash.SetActive(false);
        }

        public override void AlternativeFire()
        {
            return;
        }
    }
}