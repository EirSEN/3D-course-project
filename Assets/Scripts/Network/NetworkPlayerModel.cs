using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityStandardAssets.Characters.FirstPerson;

namespace Unity3DCourse
{
    public class NetworkPlayerModel : NetworkBehaviour, IDamagable
    {
        private PlayerView _playerView;
        //private RigidbodyFirstPersonController _FPSController;
        [SyncVar(hook = "OnHealthChanged")]
        private float _currentHealth;
        private float _maxHealth = 100f;
        private Weapon _pistol;
        //private bool _isDead = false;

        private void Start()
        {
            if (!isLocalPlayer) return;

            _playerView = FindObjectOfType<PlayerView>();
            _pistol = GetComponentInChildren<Weapon>();
            //_FPSController = GetComponent<RigidbodyFirstPersonController>();

            _currentHealth = _maxHealth;
            _playerView.SetMaxHealth(_maxHealth);
            _playerView.DisplayHealth(_currentHealth);
            //_FPSController.enabled = true;
        }

        public override void OnStartClient()
        {
            base.OnStartClient();

            GetComponent<Renderer>().enabled = true;
        }

        public override void OnStartLocalPlayer()
        {
            base.OnStartLocalPlayer();

            GetComponent<Renderer>().enabled = false;
        }

        public void GetDamage(float damage, Vector3 direction)
        {
            if (!isServer) return;

            _currentHealth -= damage;

            if (_currentHealth > _maxHealth)
            {
                _currentHealth = _maxHealth;
            }
            
            if (_currentHealth <= 0)
            {
                RpcRespawn();
            }
        }

        private void OnHealthChanged(float health)
        {
            _currentHealth = health;

            if (isLocalPlayer)
            {
                _playerView.DisplayHealth(_currentHealth);
            }
        }

        private void Update()
        {
            if (!isLocalPlayer) return;

            if (Input.GetMouseButton(0))
                _pistol.Fire();
        }


        [ClientRpc]
        private void RpcRespawn()
        {

            _currentHealth = _maxHealth;
            transform.position = Vector3.up;
        }
    }
}