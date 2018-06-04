using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unity3DCourse
{
    public class Mine : BaseObjectScene
    {
        [SerializeField]
        private float _damageAmmount = 100f;
        [SerializeField]
        private float _explosionForce = 400f;
        [SerializeField]
        private float _explosionRadius = 1.5f;

        private void OnTriggerEnter(Collider other)
        {
            IDamagable unit = other.gameObject.GetComponent<IDamagable>();

            if (unit == null)
                return;

            unit.GetDamage(_damageAmmount, Vector3.zero);

            if (other.gameObject.GetComponent<Rigidbody>())
            {
                other.gameObject.GetComponent<Rigidbody>().AddExplosionForce(_explosionForce, transform.position, _explosionRadius, 2f, ForceMode.Impulse);
            }

            Destroy(gameObject);
        }
    }
}