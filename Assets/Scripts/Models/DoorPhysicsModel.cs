using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unity3DCourse.Models
{
    public class DoorPhysicsModel : BaseObjectScene, IInteractable
    {
        private HingeJoint _hingleJoint;
        [SerializeField]
        private float _torque = 100f;
        [SerializeField]
        private float _openAngle; // Угол от положения спокойствия, до которого будет применена сила для открытия
        [SerializeField]
        private bool _rightDoor;
        private Vector3 _torqueVector;


        protected override void Awake()
        {
            base.Awake();

            _hingleJoint = GetComponent<HingeJoint>();
            _torqueVector = new Vector3(_torque, 0, _torque);
        }

        private void Update()
        {
            //Debug.LogError(_hingleJoint.angle);
        }

        private void AddOpenForce()
        {
            if (_rightDoor)
            {
                if (_hingleJoint.angle > _hingleJoint.limits.max + _openAngle)
                {
                    Rigidbody.AddRelativeTorque(-_torqueVector, ForceMode.Impulse); // Вектор с минусом - открытие
                }
                else
                {
                    Rigidbody.AddRelativeTorque(_torqueVector, ForceMode.Impulse); // Вектор с плюсом - закрытие
                }
            }
            else
            {
                if (_hingleJoint.angle < _hingleJoint.limits.min + _openAngle)
                {
                    Rigidbody.AddRelativeTorque(-_torqueVector, ForceMode.Impulse); // Вектор с минусом - открытие
                }
                else
                {
                    Rigidbody.AddRelativeTorque(_torqueVector, ForceMode.Impulse); // Вектор с плюсом - закрытие
                }
            }

        }

        public void Interact()
        {
            AddOpenForce();
        }
    }
}