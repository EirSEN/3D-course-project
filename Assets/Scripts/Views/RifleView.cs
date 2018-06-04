using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Unity3DCourse.View
{
    public class RifleView : MonoBehaviour
    {
        private Image _scopeImage;
        private Camera _mainCamera;
        private float _normalFOV;
        [SerializeField]
        private float _scopeFOV = 15f;

        private void Awake()
        {
            _scopeImage = GetComponent<Image>();
            _scopeImage.enabled = false;
        }

        private void Start()
        {
            _mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
            _normalFOV = _mainCamera.fieldOfView;
        }

        public void TurnScope(bool value)
        {
            _scopeImage.enabled = value;
            if (value)
            {
                _mainCamera.fieldOfView = _scopeFOV;
            }
            else
            {
                _mainCamera.fieldOfView = _normalFOV;
            }
        }
    }
}