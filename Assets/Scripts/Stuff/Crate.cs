using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unity3DCourse
{
    public class Crate : BaseObjectScene, IDamagable
    {
        [SerializeField]
        private float _maxHealth = 100f;
        private float _currentHealth;

        public float Health
        {
            get
            {
                return _currentHealth;
            }
            private set
            {
                if (_currentHealth < 0) return;

                _currentHealth = value;
                if (_currentHealth > _maxHealth)
                {
                    _currentHealth = _maxHealth;
                }

                if (_currentHealth <= 0)
                    Death();
            }
        }

        protected override void Awake()
        {
            base.Awake();
            Health = _maxHealth;
        }

        public void GetDamage(float damage, Vector3 dir)
        {
            Health -= damage;
        }

        private void Death()
        {
            Color = Color.white;
            Collider.enabled = false;
            Destroy(gameObject, 3f);
        }
    }
}