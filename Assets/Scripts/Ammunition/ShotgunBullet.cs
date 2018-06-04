using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unity3DCourse
{
    public class ShotgunBullet : Ammunition
    {
        [SerializeField]
        private int _gritCount = 10;
        [SerializeField]
        private GameObject _gritPrefab;

        public void Initialize(Vector3 position, Vector3 force, float damageMult = 1f)
        {
            Awake();

            for (int i = 0; i < _gritCount; i++)
            {
                Vector2 randomPos = Random.insideUnitCircle / 10;
                Vector3 randomPosV3 = new Vector3(randomPos.x, randomPos.y, 0);

                ShotgunGrit grit = Instantiate(_gritPrefab, position + randomPosV3, Quaternion.identity).GetComponent<ShotgunGrit>();

                //float randomX = Random.Range(-1, 1);
                //float randomY = Random.Range(-1, 1);

                grit.Initialize(force);
            }

            Destroy(gameObject, 5f);
        }
    }
}