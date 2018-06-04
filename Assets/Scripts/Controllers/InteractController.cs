using System.Collections;
using System.Collections.Generic;
using Unity3DCourse.Controllers;
using Unity3DCourse.Models;
using UnityEngine;

namespace Unity3DCourse.Controllers
{
    public class InteractController : BaseController
    {
        // Немного небезопасно. В идеале сделать событие и подписывать на него поднятый объект.
        public bool IsSomethingLifted { get; set; }

        public void Interact()
        {
            RaycastHit hit;
            
            if (Physics.Raycast(Main.Instance.PlayerCamera.transform.position, Main.Instance.PlayerCamera.transform.forward, out hit, 2f, LayerMask.GetMask("Interactable")))
            {
                IInteractable _interactableObject = hit.transform.gameObject.GetComponent<IInteractable>();
                if (_interactableObject != null)
                {
                    _interactableObject.Interact();
                }
            }
        }
    }
}