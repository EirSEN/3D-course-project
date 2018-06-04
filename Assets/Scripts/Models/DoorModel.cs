using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unity3DCourse.Models
{
    public class DoorModel : BaseObjectScene, IInteractable
    {
        public bool IsOpened { get; private set; }
        private Animator _animator;

        protected override void Awake()
        {
            base.Awake();
            _animator = GetComponent<Animator>();
            IsOpened = false;
        }

        private void Open()
        {
            _animator.SetTrigger("Open");
            IsOpened = true;
        }

        private void Close()
        {
            _animator.SetTrigger("Close");
            IsOpened = false;
        }

        public void Interact()
        {
            if (IsOpened)
            {
                Close();
            }
            else
            {
                Open();
            }
        }
    }
}