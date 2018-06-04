using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unity3DCourse
{
    public class Fist : Weapon
    {
        //private Transform _firePoint;
        [SerializeField]
        private float _timeout = 1f;
        [SerializeField]
        private float _damage = 20f; 
        private float _lastShotTime;
        [SerializeField]
        private Transform _attackPoint;
        private Animator _anim;

        protected override void Awake()
        {
            base.Awake();
            _anim = GetComponentInParent<Animator>();
        }


        public override void Fire()
        {
            if (Time.time - _lastShotTime < _timeout)
                return;

            _anim.SetTrigger("Attack");

            _lastShotTime = Time.time;
        }

        // Вызывается в качестве ивента в анимации
        public void LaunchAttack()
        {
            var colliders = Physics.OverlapSphere(_attackPoint.transform.position, 0.4f);
            
            foreach (var col in colliders)
            {
                IDamagable damagable = col.gameObject.GetComponent<IDamagable>();
                if (damagable != null)
                {
                    damagable.GetDamage(_damage, Vector3.back);
                    return; // Наносим урон только один раз.
                }
            }
        }





        #region Unnecessary for melee weapon
        public override void ReloadWeapon()
        {
            return;
        }

        public override void SelectWeapon()
        {
            return;
        }
        public override void DeselectWeapon()
        {
            return;
        }

        public override void AlternativeFire()
        {
            return;
        }
        #endregion
    }
}