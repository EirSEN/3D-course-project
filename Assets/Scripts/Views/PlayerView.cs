using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Unity3DCourse
{
    public class PlayerView : MonoBehaviour
    {
        [SerializeField]
        private Slider _health;
        [SerializeField]
        private Image _fader;

        private void Start()
        {
            _health.minValue = 0;
            _fader.color = new Color(0, 0, 0, 0);

        }

        public void DisplayHealth(float health)
        {
            _health.value = health / 100;
        }

        public void SetMaxHealth(float maxHealth)
        {
            _health.maxValue = maxHealth / 100;
        }

        public IEnumerator Fade()
        {
            while (_fader.color.a < 1)
            {
                _fader.color += new Color(0, 0, 0, 0.05f);
                yield return new WaitForSeconds(0.1f);
            }
        }
    }
}