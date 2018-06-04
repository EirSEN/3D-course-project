using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Unity3DCourse
{
    public class ObjectPoolManager : MonoBehaviour
    {
        private GameObject ProjectileParent { get; set; }
        private GameObject BulletFXParent { get; set; }

        private Dictionary<string, List<Ammunition>> _pullCollection = new Dictionary<string, List<Ammunition>>();
        private Dictionary<string, List<BulletEffect>> _bulletFXPull = new Dictionary<string, List<BulletEffect>>();

        private void Awake()
        {
            ProjectileParent = new GameObject("Projectiles");
            BulletFXParent = new GameObject("BulletFX");
        }

        // Инициализируем обобщенным способом коллекцию снарядов
        public void InitializePool<T>(GameObject prefab, float weaponTimeout, float bulletDistractionTime) where T : Ammunition
        {
            int capacity = Mathf.CeilToInt(bulletDistractionTime / weaponTimeout) + 2;
            List<Ammunition> currentAmmo = new List<Ammunition>();
            for (int i = 0; i < capacity; i++)
            {
                currentAmmo.Add(Instantiate(prefab, ProjectileParent.transform).GetComponent<T>());
                currentAmmo[i].gameObject.SetActive(false);
            }

            List<Ammunition> currentAmmoList = new List<Ammunition>();

            if (_pullCollection.TryGetValue(typeof(T).Name, out currentAmmoList))
            {
                _pullCollection[typeof(T).Name].AddRange(currentAmmo);
            }
            else
            {
                _pullCollection.Add(typeof(T).Name, currentAmmo);
            }
        }

        // Получаем первый неактивный снаряд из пула объектов
        public T GetInactiveAmmunition<T>() where T : Ammunition
        {
            List <Ammunition> temp = new List<Ammunition>();

            if (_pullCollection.TryGetValue(typeof(T).Name, out temp))
            {
                if (temp != null)
                {
                    foreach (var item in temp)
                    {
                        if (!item.gameObject.activeSelf)
                        {
                            return item as T;
                        }
                    }
                }
            }

            return null;
        }

        public void InitializeBulletFXPool<T>(GameObject prefab, float fireRate, float bulletDistractionTime) where T : BulletEffect 
        {
            int capacity = Mathf.CeilToInt(bulletDistractionTime / fireRate) + 2;

            List<BulletEffect> currentFX = new List<BulletEffect>();
            for (int i = 0; i < capacity; i++)
            {
                currentFX.Add(Instantiate(prefab, BulletFXParent.transform).GetComponent<T>());
                currentFX[i].gameObject.SetActive(false);
            }

            List<BulletEffect> currentBulletFX = new List<BulletEffect>();

            if (_bulletFXPull.TryGetValue(typeof(T).Name, out currentBulletFX))
            {
                _bulletFXPull[typeof(T).Name].AddRange(currentFX);
            }
            else
            {
                _bulletFXPull.Add(typeof(T).Name, currentFX);
            }
        }

        public T GetInactiveBulletFX<T>() where T : BulletEffect
        {
            List<BulletEffect> temp = new List<BulletEffect>();

            if (_bulletFXPull.TryGetValue(typeof(T).Name, out temp))
            {
                if (temp != null)
                {
                    foreach (var item in temp)
                    {
                        if (!item.gameObject.activeSelf)
                        {
                            return item as T;
                        }
                    }
                }
            }

            return null;
        }
    }
}