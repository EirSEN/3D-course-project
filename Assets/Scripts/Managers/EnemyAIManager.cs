using System.Collections;
using System.Collections.Generic;
using Unity3DCourse.Models;
using UnityEngine;

namespace Unity3DCourse
{
    public class EnemyAIManager : MonoBehaviour
    {
        private List<EnemyMeleeModel> _closeEnemies = new List<EnemyMeleeModel>();
        private List<EnemyMeleeModel> _farEnemies = new List<EnemyMeleeModel>();
        [SerializeField]
        private float _closeRangeDistance = 25f;
        [SerializeField]
        [Range(5, 20)]
        private int _checkEveryFrame = 10;
        private int _counter = 0;

        void Start()
        {
            EnemyMeleeModel[] allEnemies = FindObjectsOfType<EnemyMeleeModel>();

            foreach (var enemy in allEnemies)
            {
                if (Vector3.Distance(enemy.transform.position, Main.Instance.PlayerController.transform.position) < _closeRangeDistance)
                {
                    _closeEnemies.Add(enemy);
                    enemy.SetRangeState(EnemyRangeStates.CLOSE);
                }
                else
                {
                    _farEnemies.Add(enemy);
                    enemy.SetRangeState(EnemyRangeStates.FAR);
                }
            }

        }

        void Update()
        {
            if (_counter < _checkEveryFrame)
            {
                _counter++;
                return;
            }

            _counter = 0;

            foreach (var enemy in _closeEnemies)
            {
                if (Vector3.Distance(enemy.transform.position, Main.Instance.PlayerController.transform.position) > _closeRangeDistance)
                {
                    _farEnemies.Add(enemy);
                    enemy.SetRangeState(EnemyRangeStates.FAR);
                    _closeEnemies.Remove(enemy);
                    break; // При модификации коллекции вынужден прервать цикл foreach, чтобы итератор не выдал исключение
                    // Можно создать промежуточный массив и перемещать противников туда, если есть острая необходимость в точной проверке
                }
            }

            foreach (var enemy in _farEnemies)
            {
                if (Vector3.Distance(enemy.transform.position, Main.Instance.PlayerController.transform.position) < _closeRangeDistance)
                {
                    _closeEnemies.Add(enemy);
                    enemy.SetRangeState(EnemyRangeStates.CLOSE);
                    _farEnemies.Remove(enemy);
                    break;
                }
            }
        }

        public void RemoveEnemy(EnemyMeleeModel enemy)
        {
            if (_farEnemies.Contains(enemy))
            {
                _farEnemies.Remove(enemy);
            }
            else if (_closeEnemies.Contains(enemy))
            {
                _closeEnemies.Remove(enemy);
            }
        }
    }
}