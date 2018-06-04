using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity3DCourse.Data;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Unity3DCourse
{
    public class HealthPackManager : MonoBehaviour
    {
        private List<HealthPack> _healthPackList = new List<HealthPack>();
        private static DataManager _dataManager = new DataManager();
        private GameObject parentGameObject;

        private void Start()
        {
            _healthPackList.AddRange(FindObjectsOfType<HealthPack>());

            //parentGameObject = new GameObject("MedKits");

            //_dataManager.SetData<JsonData>();
            //_dataManager.SetOptions(Application.dataPath);

            //LoadHealthPackData();
        }

        public void AddHealthPack(HealthPack pack)
        {
            if (!_healthPackList.Contains(pack))
            {
                _healthPackList.Add(pack);
            }
        }

        public void RemoveHealthPack(HealthPack pack)
        {
            if (_healthPackList.Contains(pack))
            {
                _healthPackList.Remove(pack);
            }
        }

        public HealthPack GetClosestHealthPack(Transform point)
        {
            if (_healthPackList.Count == 0)
            {
                return null;
            }

            float minDistance = float.PositiveInfinity;
            HealthPack returnable = null;

            foreach (var pack in _healthPackList)
            {
                if (Vector3.Distance(pack.transform.position, point.position) < minDistance)
                {
                    minDistance = Vector3.Distance(pack.transform.position, point.position);
                    returnable = pack;
                }
            }

            return returnable;
        }

        // В случае необходимости можно доработать этот метод для расстановки аптечек в эдиторе и сохранении в Json до игры
        #if UNITY_EDITOR
        [MenuItem("Homework/Save Health Pack Data", false, 0)]
        #endif
        public static void SaveHealthPacksData()
        {
            List<HealthPack> allHealthPacks = FindObjectsOfType<HealthPack>().ToList();
            _dataManager.Save(new CurrentGameState { MedKits = allHealthPacks });
        }

        // Метод может быть приватным в случае вызова его при старте игры. В тестовых целях он публичный
        public void LoadHealthPackData()
        {
            List<HealthPack> loaded = _dataManager.Load().MedKits;

            if (loaded != null && loaded.Count > 0)
            {
                foreach (var obj in loaded)
                {
                    GameObject pack = Instantiate(Resources.Load("HealthPack"), obj.transform.position, obj.transform.rotation, parentGameObject.transform) as GameObject;
                    _healthPackList.Add(pack.GetComponent<HealthPack>());
                }
            }
        }

        public void DestroyHealthPacks()
        {
            foreach (var item in _healthPackList)
            {
                Destroy(item.gameObject);
            }
            _healthPackList.Clear();
        }
    }
}