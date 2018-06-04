using System.Collections;
using System.Collections.Generic;
using Unity3DCourse.View;
using UnityEngine;

namespace Unity3DCourse.Models
{
    public class FlashLightModel : MonoBehaviour
    {
        private Light _light;
        private FlashLightView _flashLightView;
        private float _maxLightIntensity = 4f;
        private float _midLightIntensity = 2f;
        private float _lowLightIntensity = 1.5f;

        public float CurrenBatteryCharge
        {
            get
            {
                return _currentBatteryCharge;
            }
            private set
            {
                _currentBatteryCharge = value;
                _flashLightView.SetFillAmount(CurrenBatteryCharge);
                if (_currentBatteryCharge > 50f)
                {
                    _light.intensity = _maxLightIntensity;
                }
                else if (_currentBatteryCharge >= 25f && _currentBatteryCharge <= 50f)
                {
                    _light.intensity = _midLightIntensity;
                }
                else if (_currentBatteryCharge < 25f && _currentBatteryCharge > 0f)
                {
                    _light.intensity = _lowLightIntensity;
                    RandomFlash();
                }
                else if (_currentBatteryCharge <= 0f)
                {
                    Main.Instance.FlashlightController.Off();
                }
            }
        }

        private float _currentBatteryCharge; // текущий заряд аккумулятора
        private float _maxBatteryCharge; // максимальный заряд аккумулятора
        [SerializeField] private float _chargeRate; // скорость разрядки/зарядки аккумулятора (за один тик)
        private WaitForSeconds _dischargeDelay = new WaitForSeconds(0.5f); // как часто происходит тик разрядки аккумулятора
        private WaitForSeconds _rechargeeDelay = new WaitForSeconds(0.25f); // как часто происходит тик зарядки аккумулятора
        private WaitForSeconds _dischargeEffectDelay = new WaitForSeconds(0.08f); // как часто происходит эффект разрядки аккумулятора

        private void Awake()
        {
            _light = GetComponent<Light>();
            _light.enabled = false;
            _maxBatteryCharge = 100f;
            _chargeRate = 4f; // большое значение только для тестирования
        }

        private void Start()
        {
            _flashLightView = FindObjectOfType<FlashLightView>();
            CurrenBatteryCharge = _maxBatteryCharge;
        }

        public bool On()
        {
            if (CurrenBatteryCharge < 10f)
            {
                return false;
            }


            _light.enabled = true;
            StopAllCoroutines();
            StartCoroutine(Discharge());
            return true;
        }

        public void Off()
        {
            _light.enabled = false;
            StopAllCoroutines();
            StartCoroutine(Recharge());
        }

        public void Switcher()
        {
            //_light.enabled = !_light.enabled;
            if (_light.enabled)
            {
                Off();
            }
            else
            {
                On();
            }
        }

        private IEnumerator Discharge()
        {
            while (CurrenBatteryCharge > 0)
            {
                CurrenBatteryCharge -= _chargeRate;
                yield return _dischargeDelay;
            }
        }

        private IEnumerator Recharge()
        {
            yield return new WaitForSeconds(1f); // задержка перед перезарядкой фонарика
            while (CurrenBatteryCharge < _maxBatteryCharge)
            {
                CurrenBatteryCharge += _chargeRate;
                yield return _rechargeeDelay;
            }
        }

        private IEnumerator DischargeEffect()
        {
            _light.intensity = 0.3f;
            yield return _dischargeEffectDelay;
            _light.intensity = 0.8f;
            yield return _dischargeEffectDelay;
            _light.intensity = 0.1f;
            yield return _dischargeEffectDelay;
            _light.intensity = 0.5f;
            yield return _dischargeEffectDelay;
            _light.intensity = 0.2f;
            yield return _dischargeEffectDelay;
            _light.intensity = 0.7f;

        }

        private void RandomFlash()
        {
            if (!_light.enabled) return;

            if (Random.Range(0, 10) < 8) return;

            StartCoroutine(DischargeEffect());
        }
    }
}