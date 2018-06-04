using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unity3DCourse.Controllers
{
    public abstract class BaseController : MonoBehaviour
    {
        public bool IsEnabled { get; protected set; }

        public virtual void On()
        {
            IsEnabled = true;
        }

        public virtual void Off()
        {
            IsEnabled = false;
        }
    }
}
