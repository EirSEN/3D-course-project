using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unity3DCourse
{
    public class Barrel : BaseObjectScene, IDamagable
    {
        [SerializeField]
        private float _hitsToDie = 7f;
        private float _hitsToDieLeft;

        public float Health
        {
            get
            {
                return _hitsToDieLeft;
            }
            private set
            {
                if (_hitsToDieLeft < 0) return;

                _hitsToDieLeft = value;
                if (_hitsToDieLeft > _hitsToDie)
                {
                    _hitsToDieLeft = _hitsToDie;
                }
                
                if (_hitsToDieLeft <= 0)
                    Death();

            }
        }

        protected override void Awake()
        {
            base.Awake();
            Health = _hitsToDie;
        }

        public void GetDamage(float damage, Vector3 t)
        {
            Health--;
        }

        private void Death()
        {
            Color = Color.red;
            Rigidbody.AddForce(50, 200, 50, ForceMode.Impulse);
            Rigidbody.AddTorque(60, 20, 70);
            Destroy(gameObject, 3f);
        }
    }
}
