using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Unity3DCourse
{
    public abstract class BaseObjectScene : MonoBehaviour
    {
        protected int _layer;
        protected Renderer _renderer;
        protected Material _material;
        protected Color _color;
        protected Rigidbody _rigidbody;
        protected Collider _collider;
        protected string _name;
        protected bool _isVisible;

        [Header("Physics properties")]

        [SerializeField] protected bool _isMovable;
        [SerializeField] protected bool _isDragable;
        [SerializeField] protected bool _isLiftable;
        [SerializeField] protected bool _isThroughWalkable;

        #region Properties

        public int Layer
        {
            get
            {
                return _layer;
            }
            set
            {
                _layer = value;
                if (gameObject)
                {
                    SetLayers(transform, _layer);
                }
            }
        }

        public Renderer Renderer
        {
            get
            {
                return _renderer;
            }

            private set
            {
                _renderer = value;
            }
        }

        public Material Material
        {
            get
            {
                return _material;
            }

            set
            {
                _material = value;
                if (_renderer)
                {
                    _renderer.material = _material;
                }
            }
        }

        public Color Color
        {
            get
            {
                return _color;
            }

            set
            {
                _color = value;
                _material.color = _color;
            }
        }

        public string Name
        {
            get
            {
                return _name;
            }

            set
            {
                _name = value;
                if (gameObject)
                {
                    gameObject.name = _name;
                }
            }
        }

        public bool IsVisible
        {
            get
            {
                return CheckVisibility(transform);
            }
            set
            {
                _isVisible = value;
                SetVisibility(transform, _isVisible);
            }
        }

        public Rigidbody Rigidbody
        {
            get
            {
                return _rigidbody;
            }

            private set
            {
                _rigidbody = value;
            }
        }

        public Collider Collider
        {
            get
            {
                return _collider;
            }
            private set
            {
                _collider = value;
            }
        }

        public bool IsMovable
        {
            get
            {
                return _isMovable;
            }
            private set
            {
                _isMovable = value;
                if (_isMovable)
                {
                    SetMovableObject();
                }
                else
                {
                    SetDefaultPhysicsObject();
                }
            }
        }

        public bool IsDragable
        {
            get
            {
                return _isDragable;
            }
            private set
            {
                _isDragable = value;
                if (_isDragable)
                {
                    SetDragableObject();
                }
                else
                {
                    SetDefaultPhysicsObject();
                }
            }
        }

        public bool IsLiftable
        {
            get
            {
                return _isLiftable;
            }
            private set
            {
                _isDragable = value;
                if (_isDragable)
                {
                    SetLiftableObject();
                }
                else
                {
                    SetDefaultPhysicsObject();
                }
            }
        }

        public bool IsThroughWalkable
        {
            get
            {
                return _isThroughWalkable;
            }
            private set
            {
                _isThroughWalkable = value;
                if (_isThroughWalkable)
                {
                    SetThroughWalkableObject();
                }
                else
                {
                    SetDefaultPhysicsObject();
                }
            }
        }

        #endregion

        protected virtual void Awake()
        {
            _layer = gameObject.layer;
            Name = gameObject.name;
            Renderer = GetComponent<Renderer>();
            if (Renderer)
            {
                Material = Renderer.material;
                if (Material)
                {
                    //Color = Material.color;
                }
            }
            _rigidbody = GetComponent<Rigidbody>();
            _collider = GetComponent<Collider>();
        }

        protected void SetLayers(Transform obj, int layer)
        {
            obj.gameObject.layer = layer;
            if (obj.childCount > 0)
            {
                foreach (Transform child in obj)
                {
                    SetLayers(child, layer);
                }
            }
        }

        protected virtual void SetMovableObject()
        {
            Rigidbody.isKinematic = false;
            Rigidbody.drag = 0f;
            Collider.isTrigger = false;
        }

        protected virtual void SetDragableObject()
        {
            Rigidbody.freezeRotation = true;
            Rigidbody.drag = 0f;
            Rigidbody.isKinematic = false;
            Collider.isTrigger = false;
        }

        protected virtual void SetLiftableObject()
        {
            Rigidbody.freezeRotation = true;
            Rigidbody.constraints = RigidbodyConstraints.FreezePositionX & RigidbodyConstraints.FreezePositionZ;
            Rigidbody.isKinematic = false;
            Collider.isTrigger = false;
        }

        protected virtual void SetThroughWalkableObject()
        {
            Rigidbody.isKinematic = true;
            Collider.enabled = false;
        }

        protected virtual void SetDefaultPhysicsObject()
        {
            Rigidbody.constraints = RigidbodyConstraints.None;
            Rigidbody.isKinematic = false;
            Collider.enabled = true;
            Collider.isTrigger = false;
        }

        private void SetVisibility(Transform objTransform, bool visible)
        {
            var rend = objTransform.GetComponent<Renderer>();
            if (rend)
                rend.enabled = visible;

            foreach (var r in GetComponentsInChildren<Renderer>(true))
                r.enabled = visible;
        }

        private bool CheckVisibility(Transform objTransform)
        {
            var rend = objTransform.GetComponent<Renderer>();

            if (rend)
            {
                return rend.enabled;
            }
            else
            {
                foreach (var r in GetComponentsInChildren<Renderer>(true))
                {
                    if (!r.enabled) return false;
                }
                return true;
            }
        }
    }
}