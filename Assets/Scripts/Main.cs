using System.Collections;
using System.Collections.Generic;
using Unity3DCourse.Controllers;
using UnityEngine;
using UnityEngine.Networking;
using UnityStandardAssets.Characters.FirstPerson;

namespace Unity3DCourse
{
    [DefaultExecutionOrder(-1)]
    public class Main : MonoBehaviour
    {
        public static Main Instance { get; private set; }

        public InputController InputController { get; private set; }
        public FlashLightController FlashlightController { get; private set; }
        public InteractController InteractController { get; private set; }
        public WeaponController WeaponController { get; private set; }
        public AllyController AllyController { get; private set; }

        public HeadBob PlayerCamera { get; private set; }
        public RigidbodyFirstPersonController PlayerController { get; private set; }

        public ObjectManager ObjectManager { get; private set; }
        public ObjectPoolManager ObjectPoolManager { get; private set; }
        public WayPointManager WayPointManager { get; private set; }
        public HealthPackManager HealthPackManager { get; private set; }
        public VFXManager VFXManager { get; private set; }
        public EnemyAIManager EnemyAIManager { get; private set; }


        private void Awake()
        {
            if (Instance)
            {
                DestroyImmediate(gameObject);
            }
            else
            {
                Instance = this;
            }

            InputController = gameObject.AddComponent<InputController>();
            FlashlightController = gameObject.AddComponent<FlashLightController>();
            InteractController = gameObject.AddComponent<InteractController>();
            WeaponController = gameObject.AddComponent<WeaponController>();
            AllyController = gameObject.AddComponent<AllyController>();
            HealthPackManager = gameObject.AddComponent<HealthPackManager>();


            ObjectManager = GetComponent<ObjectManager>();
            ObjectPoolManager = GetComponent<ObjectPoolManager>();
            VFXManager = GetComponent<VFXManager>();
            EnemyAIManager = GetComponent<EnemyAIManager>();
        }

        private void Start()
        {
            PlayerCamera = FindObjectOfType<HeadBob>();
            PlayerController = FindObjectOfType<RigidbodyFirstPersonController>();
            WayPointManager = FindObjectOfType<WayPointManager>();            
        }
    }
}
