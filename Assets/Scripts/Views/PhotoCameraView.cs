using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Unity3DCourse.View
{
    public class PhotoCameraView : MonoBehaviour
    {
        private Image _flash;
        [SerializeField]
        private Image _blackTop;
        [SerializeField]
        private Image _blackBotoom;

        private Color _flashColor = new Color(1, 1, 1, 0.8f);
        private Color _beforeFlashColor = new Color(1, 1, 1, 0);

        private void Awake()
        {
            _flash = GetComponent<Image>();
        }

        public void PhotoEffect()
        {
            StartCoroutine(TopBlackSlide());
            StartCoroutine(BottomBlackSlide());
        }

        private IEnumerator TopBlackSlide()
        {
            while (_blackTop.rectTransform.localPosition.y > 0)
            {
                _blackTop.rectTransform.Translate(Vector2.down * 50f);
                yield return null;
            }
            while (_blackTop.rectTransform.localPosition.y < 540)
            {
                _blackTop.rectTransform.Translate(Vector2.up * 50f);
                yield return null;
            }
        }

        private IEnumerator BottomBlackSlide()
        {
            while (_blackBotoom.rectTransform.localPosition.y < 0)
            {
                _blackBotoom.rectTransform.Translate(Vector2.up * 50f);
                yield return null;
            }
            StartCoroutine(PhotoFlash());
            while (_blackBotoom.rectTransform.localPosition.y > -540)
            {
                _blackBotoom.rectTransform.Translate(Vector2.down * 50f);
                yield return null;
            }
        }

        private IEnumerator PhotoFlash()
        {
            while (_flash.color.a <= 0.7f)
            {
                _flash.color = Color.Lerp(_flash.color, _flashColor, 0.15f);
                yield return null;
            }

            while (_flash.color.a >= 0.02f)
            {
                _flash.color = Color.Lerp(_flash.color, _beforeFlashColor, 0.1f);
                yield return null;
            }

            _flash.color = _beforeFlashColor;
        }
    }
}