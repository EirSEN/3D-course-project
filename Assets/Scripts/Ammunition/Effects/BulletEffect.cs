using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unity3DCourse
{
    public abstract class BulletEffect : BaseObjectScene
    {
        [SerializeField]
        protected float _timeToDisable;
    }
}