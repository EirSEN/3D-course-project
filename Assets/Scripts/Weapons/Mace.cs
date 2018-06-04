using System.Collections;
using System.Collections.Generic;
using Unity3DCourse.View;
using UnityEngine;

namespace Unity3DCourse
{
    public class Mace : Weapon
    {
        [SerializeField]
        private float _timeout = 1f;
        [SerializeField]
        private float _damage = 30f;
        [SerializeField]
        private LayerMask _layerMask;
        [SerializeField]
        private Transform _overlapPosition1;
        [SerializeField]
        private Transform _overlapPosition2;
        private float _capsuleRadius = 0.5f;
        private float _lastShotTime;
        private AmmoView _ammoView;
        private Animator _anim;

        private void Start()
        {
            _ammoView = FindObjectOfType<AmmoView>();
            _anim = GetComponent<Animator>();

        }

        public override void Fire()
        {
            if (Time.time - _lastShotTime < _timeout)
                return;

            _anim.SetTrigger("Attack");

            _lastShotTime = Time.time;
        }

        public void DealDamage()
        {
            var cols = Physics.OverlapCapsule(_overlapPosition1.position, _overlapPosition2.position, _capsuleRadius, _layerMask);

            foreach (var enemyCollider in cols)
            {
                IDamagable damagable = enemyCollider.gameObject.GetComponent<IDamagable>();

                if (damagable != null)
                {
                    damagable.GetDamage(_damage, Vector3.forward);
                }
            }
        }

        public override void ReloadWeapon()
        {
            return;
        }

        public override void SelectWeapon()
        {
            _ammoView.DisplayCurrentAmmo(0);
            _ammoView.DisplayMaxAmmo(0);
        }
        public override void DeselectWeapon()
        {
            return;
        }

        public override void AlternativeFire()
        {
            return;
        }
    }
}