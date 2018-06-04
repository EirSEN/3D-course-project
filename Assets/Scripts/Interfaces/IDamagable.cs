using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Unity3DCourse
{
    public interface IDamagable
    {
        //void GetDamage(float damage);
        void GetDamage(float damage, Vector3 direction);
    }
}