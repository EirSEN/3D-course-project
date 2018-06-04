using System.Collections;
using System.Collections.Generic;
using Unity3DCourse.Models;
using UnityEngine;
using UnityEngine.AI;

namespace Unity3DCourse.Controllers
{
    public class AllyController : BaseController
    {
        private AllyModel _ally;

        private void Awake()
        {
            _ally = FindObjectOfType<AllyModel>();
        }

        // Метод, добавляющий союзника (может быть вызыван при появлении нового союзника)
        public void Initialize(AllyModel allyModel)
        {
            _ally = allyModel;
        }

        public void SetDestinationPoint()
        {
            if (!_ally) return;

            RaycastHit hit;

            // В перспективе есть возможность добавить LayerMask, чтобы луч проходил через определенные объекты
            if (Physics.Raycast(Main.Instance.PlayerCamera.transform.position, Main.Instance.PlayerCamera.transform.forward, out hit, 20f))
            {
                if (hit.transform.gameObject.tag == "Ground")
                {
                    _ally.AddDestinationPoint(hit.point);
                }
            }
        }
    }
}