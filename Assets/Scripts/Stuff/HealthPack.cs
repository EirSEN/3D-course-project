using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unity3DCourse
{
    public class HealthPack : BaseObjectScene
    {
        // Отрицательное значение установлено, чтобы через метод GetDamage() восполнять здоровье
        [SerializeField]
        private float _healthRestoreAmmount = -100f;

        private void OnTriggerEnter(Collider other)
        {
            IDamagable unit = other.gameObject.GetComponent<IDamagable>();

            if (unit == null)
                return;

            unit.GetDamage(_healthRestoreAmmount, Vector3.zero);
            Main.Instance.HealthPackManager.RemoveHealthPack(this);
            Destroy(gameObject);
        }

        public override string ToString()
        {
            return gameObject.transform.position + " " + _healthRestoreAmmount;
        }
    }
}