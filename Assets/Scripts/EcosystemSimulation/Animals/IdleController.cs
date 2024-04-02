using UnityEngine;
using UnityEngine.AI;
using System.Collections;

namespace Animals
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class idleController : MonoBehaviour
    {
        #region Configuration
        [Tooltip("The lower bound value used for a numerical range or limit.")]
        [SerializeField] private float _lowerBound;
        [Tooltip("The upper bound value used for a numerical range or limit.")]
        [SerializeField] private float _upperBound;
        #endregion

        #region Private Members
        private NavMeshAgent              _agent;
        [SerializeField]  private Vector3                   _movePosition;
        private AnimalBehaviourController _animalBehaviour;
        #endregion

        #region Unity Methods
        private void Start()
        {
            _agent = GetComponent<NavMeshAgent>();
            _animalBehaviour = GetComponent<AnimalBehaviourController>();
            _movePosition = _agent.transform.position;
        }

        private void Update()
        {
            if (_agent.velocity.magnitude < 0.15f && _animalBehaviour.CurrentState != AnimalState.Fleeing &&
                _animalBehaviour.CurrentState != AnimalState.ChasingPrey && !_agent.pathPending)
            {
                _movePosition = GetRandomPointOnNavMesh(Random.Range(_lowerBound, _upperBound));
                _agent.SetDestination(_movePosition);
            }
        }
        #endregion

        #region Local Methods
        /// <summary>
        /// Get a random point on the NavMesh within a specified radius from the current object's position.
        /// </summary>
        /// <param name="radius">The maximum distance from the object's position to sample a random point.</param>
        /// <returns>A random point on the NavMesh within the specified radius.</returns>
        private Vector3 GetRandomPointOnNavMesh(float radius)
        {
            Vector3 randomDirection = Random.insideUnitSphere * radius;
            randomDirection.y = transform.position.y;

            NavMeshHit navHit;
            if (NavMesh.SamplePosition(randomDirection + this.transform.position, out navHit, radius, -1))
            {
                return new Vector3(navHit.position.x, transform.position.y, navHit.position.z);
            }

            return this.transform.position;
        }
        #endregion
    }
}