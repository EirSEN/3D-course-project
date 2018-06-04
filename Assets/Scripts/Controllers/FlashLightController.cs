using System.Collections;
using System.Collections.Generic;
using Unity3DCourse.Models;
using UnityEngine;

namespace Unity3DCourse.Controllers
{
    public class FlashLightController : BaseController
    {
        private FlashLightModel _flashLight;

        private void Awake()
        {
            _flashLight = FindObjectOfType<FlashLightModel>();
        }

        private void Start()
        {
            Off();
        }

        public override void On()
        {
            _flashLight.On();

            base.On();

        }

        public override void Off()
        {
            base.Off();

            if (_flashLight)
                _flashLight.Off();
        }

        public void Switch()
        {
            _flashLight.Switcher();
            IsEnabled = _flashLight.enabled;
        }
    }
}
