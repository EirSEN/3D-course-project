using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Unity3DCourse.Controllers
{
    public class InputController : BaseController
    {
        void Update()
        {
            if (Input.GetButtonDown("Flashlight"))
            {
                Main.Instance.FlashlightController.Switch();
            }

            if (Input.GetButtonDown("Action"))
            {
                Main.Instance.InteractController.Interact();
            }

            if (Input.GetButton("Fire1"))
            {
                Main.Instance.WeaponController.Fire();
            }

            if (Input.GetButtonDown("Fire2"))
            {
                Main.Instance.WeaponController.AltFire();
            }

            if (Input.GetButtonDown("WeaponChange"))
            {
                Main.Instance.WeaponController.NextWeapon();
            }

            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                Main.Instance.WeaponController.NextWeapon();
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                Main.Instance.WeaponController.PreviousWeapon();
            }

            if (Input.GetButtonDown("AllyCommand"))
            {
                Main.Instance.AllyController.SetDestinationPoint();
            }

            if (Input.GetButtonDown("Reload"))
            {
                Main.Instance.WeaponController.Reload();
            }

            //if (Input.GetButtonDown("LoadData"))
            //{
            //    Main.Instance.HealthPackManager.LoadHealthPackData();
            //}

            //if (Input.GetButtonDown("DestroyObjects"))
            //{
            //    Main.Instance.HealthPackManager.DestroyHealthPacks();
            //}
        }
    }
}