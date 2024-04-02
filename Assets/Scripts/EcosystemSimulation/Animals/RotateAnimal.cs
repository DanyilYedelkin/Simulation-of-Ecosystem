using UnityEngine;
using UnityEngine.AI;

namespace Animals
{
    public class RotateAnimal : MonoBehaviour
    {
        #region Private Members
        private NavMeshAgent _navMeshAgent;
        #endregion

        #region Unity Methods
        private void Start()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            if (_navMeshAgent == null)
            {
                Debug.LogError("NavMeshAgent component is missing!");
            }
        }

        private void Update()
        {
            if (_navMeshAgent.velocity.magnitude > 0)
            {
                Vector3 movementDirection = _navMeshAgent.velocity.normalized;
                Vector3 targetPosition = transform.position + movementDirection;

                Quaternion targetRotation = Quaternion.LookRotation(movementDirection);
                transform.rotation = Quaternion.Euler(0, targetRotation.eulerAngles.y - 90, 0);
            }
        }
        #endregion
    }
}