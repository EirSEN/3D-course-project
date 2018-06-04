using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace Unity3DCourse
{
    public class WayPointManager : MonoBehaviour
    {
        private List<WayPoint> _wayPointsInScene = new List<WayPoint>();
        [SerializeField]
        private float _patrolRange = 25f;

        private void Awake()
        {
            _wayPointsInScene = GetComponentsInChildren<WayPoint>().ToList();
        }

        public List<WayPoint> GetWayPoints(Vector3 botPosition)
        {
            var points = from WayPoint point in _wayPointsInScene
                         where Vector3.Distance(point.transform.position, botPosition) < _patrolRange
                         select point;

            return points.ToList();
        }
    }
}