using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unity3DCourse.View
{
    public class RobotSparks : MonoBehaviour
    {
        private Light _light;
        private float _baseLightIntensity = 30f;

        private void Awake()
        {
            _light = GetComponent<Light>();
        }

        private void OnEnable()
        {
            _light.intensity = _baseLightIntensity;
        }

        private void Update()
        {
            _light.intensity = Mathf.Lerp(_light.intensity, 0, 0.1f);
        }
    }
}