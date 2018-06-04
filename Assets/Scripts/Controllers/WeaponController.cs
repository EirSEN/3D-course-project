using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Unity3DCourse.Controllers
{
    public class WeaponController : BaseController
    {
        private int _currentWeaponId;
        private int _currentAmmoId;

        private void Start()
        {
            for (int i = 1; i < Main.Instance.ObjectManager.Weapons.Length; i++)
            {
                Main.Instance.ObjectManager.Weapons[i].IsVisible = false;
            }
        }

        public void NextWeapon()
        {
            Main.Instance.ObjectManager.Weapons[_currentWeaponId].IsVisible = false;
            Main.Instance.ObjectManager.Weapons[_currentWeaponId].DeselectWeapon();

            _currentWeaponId++;
            if (_currentWeaponId >= Main.Instance.ObjectManager.Weapons.Length)
                _currentWeaponId = 0;

            Main.Instance.ObjectManager.Weapons[_currentWeaponId].IsVisible = true;
            Main.Instance.ObjectManager.Weapons[_currentWeaponId].SelectWeapon();
        }

        public void PreviousWeapon()
        {
            Main.Instance.ObjectManager.Weapons[_currentWeaponId].IsVisible = false;
            Main.Instance.ObjectManager.Weapons[_currentWeaponId].DeselectWeapon();

            _currentWeaponId--;
            if (_currentWeaponId < 0)
            {
                _currentWeaponId = Main.Instance.ObjectManager.Weapons.Length - 1;
            }

            Main.Instance.ObjectManager.Weapons[_currentWeaponId].IsVisible = true;
            Main.Instance.ObjectManager.Weapons[_currentWeaponId].SelectWeapon();

        }

        public void Fire()
        {
            if (Main.Instance.InteractController.IsSomethingLifted) return;

            Main.Instance.ObjectManager.Weapons[_currentWeaponId].Fire();
        }

        public void AltFire()
        {
            if (Main.Instance.InteractController.IsSomethingLifted) return;

            Main.Instance.ObjectManager.Weapons[_currentWeaponId].AlternativeFire();
        }

        public void Reload()
        {
            if (Main.Instance.InteractController.IsSomethingLifted) return;

            Main.Instance.ObjectManager.Weapons[_currentWeaponId].ReloadWeapon();
        }
    }
}