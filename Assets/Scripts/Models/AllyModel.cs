using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

namespace Unity3DCourse.Models
{
    public class AllyModel : BaseObjectScene
    {
        private Queue<GameObject> _destinations = new Queue<GameObject>();
        private NavMeshAgent _agent;
        private Transform _target;
        private ThirdPersonCharacter _character;
        private WaitForSeconds _updateAI;
        private float _updateRate = 0.25f;

        [SerializeField]
        private GameObject _pointPrefab;

        protected override void Awake()
        {
            base.Awake();
            _agent = GetComponent<NavMeshAgent>();
            _character = GetComponent<ThirdPersonCharacter>();

            _agent.updateRotation = true;
            _agent.updatePosition = true;
            _agent.updateUpAxis = true;

            _updateAI = new WaitForSeconds(_updateRate);
        }

        private void Start()
        {
            StartCoroutine(MoveToTarget());
        }

        private IEnumerator MoveToTarget()
        {
            while (true)
            {
                if (!_target)
                {
                    SetNextTarget();
                }

                if (_target != null)
                    _agent.SetDestination(_target.position);

                if (_agent.remainingDistance > _agent.stoppingDistance)
                    _character.Move(_agent.desiredVelocity, false, false);
                else
                    _character.Move(Vector3.zero, false, false);

                yield return _updateAI;
            }
        }

        public void AddDestinationPoint(Vector3 point)
        {
            NavMeshHit hit;
            if (NavMesh.SamplePosition(point, out hit, 1f, NavMesh.AllAreas))
            {
                GameObject newTarget = Instantiate(_pointPrefab, hit.position, Quaternion.identity);
                _destinations.Enqueue(newTarget);
            }
        }


        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "PointSphere")
            {
                if (_agent.destination.x == other.gameObject.transform.position.x && _agent.destination.z == other.gameObject.transform.position.z)
                {
                    _target = null;
                    Destroy(other.gameObject);
                    Destroy(other.gameObject.transform.parent.gameObject);
                }
            }
        }

        private void SetNextTarget()
        {
            if (_destinations.Count > 0)
            {
                NavMeshPath path = new NavMeshPath();
                GameObject testTarget = _destinations.Dequeue();
                NavMesh.CalculatePath(transform.position, testTarget.transform.position, 1 << LayerMask.NameToLayer("Default"), path);
                if (path.status == NavMeshPathStatus.PathComplete || path.status == NavMeshPathStatus.PathPartial)
                {
                    _target = testTarget.transform;
                }
                else
                {
                    _target = null;
                    Destroy(testTarget);
                    Destroy(testTarget.GetComponentInParent<Transform>());
                }
            }
        }

    }
}

#region Garbage 
//private void SetNextTarget()
//{
//    if (_destinations.Count > 0)
//    {
//        NavMeshPath path = new NavMeshPath();
//        GameObject testTarget = _destinations.Dequeue();
//        NavMesh.CalculatePath(transform.position, testTarget.transform.position, 1 << LayerMask.NameToLayer("Default"), path);
//        if (path.status == NavMeshPathStatus.PathComplete)
//        {
//            NavMeshHit raycatHit;
//            if (!NavMesh.Raycast(path.corners[path.corners.Length - 2], path.corners[path.corners.Length - 1], out raycatHit, NavMesh.AllAreas))
//            {
//                _target = testTarget.transform;
//            }
//            else
//            {
//                NavMeshHit NVHit;
//                if (NavMesh.FindClosestEdge(testTarget.transform.position, out NVHit, NavMesh.AllAreas))
//                {
//                    testTarget.transform.position = NVHit.position;
//                    _target = testTarget.transform;
//                }
//            }

//        }
//        else
//        {
//            _target = null;
//            Destroy(testTarget);
//            Destroy(testTarget.GetComponentInParent<Transform>());
//        }
//    }
//}
#endregion