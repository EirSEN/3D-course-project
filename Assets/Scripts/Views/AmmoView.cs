using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Unity3DCourse.View
{
    public class AmmoView : MonoBehaviour
    {

        [SerializeField]
        private Text _currentAmmo;
        [SerializeField]
        private Text _maxAmmo;
        [SerializeField]
        private Image _reloadImage;
        [SerializeField]
        private Text _reloadText;
        [SerializeField]
        private float _fillAmountRate = 0.035f;


        public void DisplayCurrentAmmo(float ammo)
        {
            _currentAmmo.text = ammo.ToString();
        }

        public void DisplayMaxAmmo(float maxAmmo)
        {
            _maxAmmo.text = maxAmmo.ToString();
        }

        // Возможно, стоит вынести в WeaponView
        public IEnumerator DisplayReload(float reloadTime)
        {
            _reloadImage.enabled = true;
            _reloadText.enabled = true;
            _reloadImage.fillAmount = 0;
            float rate = reloadTime / 30;
            while (_reloadImage.fillAmount < 1)
            {
                _reloadImage.fillAmount += _fillAmountRate;
                yield return new WaitForSeconds(rate);
            }
            _reloadImage.enabled = false;
            _reloadText.enabled = false;
        }

        public void CancelDisplayReload()
        {
            StopAllCoroutines();
            _reloadImage.enabled = false;
            _reloadText.enabled = false;
            _reloadImage.fillAmount = 0;
        }
    }

}