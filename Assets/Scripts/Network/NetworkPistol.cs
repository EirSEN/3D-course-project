//using System.Collections;
//using System.Collections.Generic;
//using Unity3DCourse.View;
//using UnityEngine;
//using UnityEngine.Networking;

//namespace Unity3DCourse
//{
//    public class NetworkPistol : Weapon
//    {
//        [SerializeField]
//        private GameObject _bulletPrefab;
//        [SerializeField]
//        private Transform _firePoint;
//        [SerializeField]
//        private float _timeout = 0.3f;
//        private float _lastShotTime;
//        private ParticleSystem[] _muzzle;
//        [SerializeField]
//        private Animator _anim;
//        [SerializeField]
//        private GameObject _bulletFXPrefab;

//        [Header("Reload Section")]
//        private const float _maxClipSize = 12f; // максимальная емкость обоймы
//        private float _currentClipSize; // текущая емкость обоймы
//        private bool _reloading;
//        private float _reloadTime = 1.2f;
//        private AmmoView _ammoView;
//        private bool _isSelectedWeapon;

//        public float CurrentClipSize
//        {
//            get { return _currentClipSize; }
//            private set
//            {
//                _currentClipSize = value;
//                if (_isSelectedWeapon)
//                {
//                    _ammoView.DisplayCurrentAmmo(_currentClipSize);
//                }
//            }
//        }

//        public float MaxClipSize
//        {
//            get { return _maxClipSize; }
//        }

//        protected override void Awake()
//        {
//            //if (!isLocalPlayer) return;
//            base.Awake();

//            _muzzle = GetComponentsInChildren<ParticleSystem>();
//        }

//        void Start()
//        {
//            if (!isLocalPlayer) return;

//            _isSelectedWeapon = true;
//            _ammoView = FindObjectOfType<AmmoView>();
//            CurrentClipSize = _maxClipSize;
//            _ammoView.DisplayMaxAmmo(_maxClipSize);
//        }

//        public override void AlternativeFire()
//        {
//            return;
//        }

//        public override void Fire()
//        {
//            if (_reloading) return;

//            if (Time.time - _lastShotTime < _timeout)
//                return;

//            CmdCreateBullet();

//            _anim.SetTrigger("Fire");
//            foreach (var effect in _muzzle)
//            {
//                effect.Play(true);
//            }
//            CurrentClipSize--;
//            if (CurrentClipSize <= 0)
//            {
//                StartCoroutine(Reload());
//            }


//            _lastShotTime = Time.time;
//        }

//        [Command]
//        private void CmdCreateBullet()
//        {
//            GameObject bullet = Instantiate(_bulletPrefab, _firePoint.position, Quaternion.identity);
//            bullet.transform.forward = _firePoint.forward;
//            NetworkServer.Spawn(bullet);
//        }

//        public override void ReloadWeapon()
//        {
//            if (_currentClipSize < _maxClipSize && !_reloading)
//            {
//                StartCoroutine(Reload());
//            }
//        }

//        public override void SelectWeapon()
//        {
//            _isSelectedWeapon = true;
//            _ammoView.DisplayCurrentAmmo(_currentClipSize);
//            _ammoView.DisplayMaxAmmo(_maxClipSize);
//            if (CurrentClipSize <= 0)
//            {
//                StartCoroutine(Reload());
//            }
//        }

//        public override void DeselectWeapon()
//        {
//            _isSelectedWeapon = false;
//            StopAllCoroutines();
//            _reloading = false;
//            _ammoView.CancelDisplayReload();
//        }

//        private IEnumerator Reload()
//        {
//            yield return new WaitForSeconds(0.15f);
//            StartCoroutine(_ammoView.DisplayReload(_reloadTime));
//            _reloading = true;
//            yield return new WaitForSeconds(_reloadTime);
//            CurrentClipSize = _maxClipSize;
//            _reloading = false;
//        }

//    }
//}