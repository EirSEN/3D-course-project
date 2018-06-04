using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unity3DCourse
{
    public class PistolBulletExplEffect : BulletEffect
    {
        private void OnEnable()
        {
            Invoke("DisableEffect", _timeToDisable);
        }


        private void DisableEffect()
        {
            gameObject.SetActive(false);
        }
    }
}