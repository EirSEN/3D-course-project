using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Unity3DCourse.View
{
    public class FlashLightView : MonoBehaviour
    {
        private Image _flashlightBar;

        private void Awake()
        {
            _flashlightBar = GetComponent<Image>();
        }

        public void SetFillAmount(float amount)
        {
            if (_flashlightBar)
            {
                _flashlightBar.fillAmount = amount/100;
            }
        }
    }
}