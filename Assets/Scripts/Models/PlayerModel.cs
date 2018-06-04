using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.Characters.FirstPerson;

namespace Unity3DCourse.Models
{
    public class PlayerModel : BaseObjectScene, IDamagable
    {
        private PlayerView _playerView;
        private RigidbodyFirstPersonController _FPSController; 
        private float _currentHealth;
        private float _maxHealth = 300f;
        private bool _isDead = false;

        private void Start()
        {
            _playerView = FindObjectOfType<PlayerView>();
            _FPSController = GetComponent<RigidbodyFirstPersonController>();

            _currentHealth = _maxHealth;
            _playerView.SetMaxHealth(_maxHealth);
            _playerView.DisplayHealth(_currentHealth);
            _FPSController.enabled = true;
        }

        public void GetDamage(float damage, Vector3 direction)
        {
            if (_isDead) return;

            _currentHealth -= damage;

            if (_currentHealth > _maxHealth)
            {
                _currentHealth = _maxHealth;
            }

            _playerView.DisplayHealth(_currentHealth);

            if (_currentHealth <= 0)
            {
                Death();
            }

            _playerView.DisplayHealth(_currentHealth);
        }

        private void Death()
        {
            _isDead = true;
            _FPSController.enabled = false;
            StartCoroutine(_playerView.Fade());
            Invoke("RestartLevel", 3f);
        }

        private void RestartLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }


    }
}