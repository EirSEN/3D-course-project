using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unity3DCourse.Models
{
    public class LiftableModel : BaseObjectScene, IInteractable
    {
        private bool _isLifted = false;
        private Transform _baseParentTransform;
        [SerializeField] // В последствии можно реализовать броски объектов
        private float _dropForce = 1000f;

        protected override void Awake()
        {
            base.Awake();
            _baseParentTransform = transform.parent;
        }

        public void Interact()
        {
            if (!_isLifted)
            {
                LiftObject();
            }
            else
            {
                DropObject();
            }
        }

        private void LiftObject()
        {
            Rigidbody.isKinematic = true;
            transform.parent = Main.Instance.PlayerCamera.transform;
            _isLifted = true;
            Main.Instance.InteractController.IsSomethingLifted = true;
        }

        private void DropObject()
        {
            Rigidbody.isKinematic = false;
            transform.parent = _baseParentTransform;
            _isLifted = false;
            Main.Instance.InteractController.IsSomethingLifted = false;
        }

        private void OnDestroy()
        {
            Main.Instance.InteractController.IsSomethingLifted = false;
        }

        private void OnDisable()
        {
            Main.Instance.InteractController.IsSomethingLifted = false;
        }

        private void OnTriggerEnter(Collider collision)
        {
            if ((collision.gameObject.tag == "Wall" || collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Player") && Main.Instance.InteractController.IsSomethingLifted)
            {
                DropObject();
            }
        }
    }
}
