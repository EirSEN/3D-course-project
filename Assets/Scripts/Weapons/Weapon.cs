using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unity3DCourse
{
    public abstract class Weapon : BaseObjectScene
    {
        public abstract void Fire();
        public abstract void AlternativeFire();
        public abstract void SelectWeapon();
        public abstract void DeselectWeapon();
        public abstract void ReloadWeapon();
    }
}