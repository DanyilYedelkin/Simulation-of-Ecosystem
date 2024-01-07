using UnityEngine;

namespace Animals.Predator
{
    public class PredatorSearchRadius : SearchRadius
    {
        #region Configuration
        [SerializeField] private PredatorController _predatorController;
        #endregion

        #region Unity Methods
        private void OnTriggerEnter(Collider other)
        {
            // _predatorController.CurrentState != AnimalState.ChasingPrey
            if (other.gameObject.tag.Equals("Prey"))
            {
                FindClosestPrey();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.tag.Equals("Prey"))
            {
                _predatorController.PreyWasLost();
            }
        }
        #endregion

        #region Local Methods
        private void FindClosestPrey()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, AnimalSearchRadius);

            Transform closestPrey = null;
            float closestDistance = Mathf.Infinity;

            foreach (var collider in colliders)
            {
                if (collider.gameObject.tag.Equals("Prey"))
                {
                    float distance = Vector3.Distance(transform.position, collider.transform.position);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestPrey = collider.transform;
                    }
                }
            }

            if (closestPrey != null)
            {
                _predatorController.PreyWasSeen(closestPrey.gameObject);
            }
        }
        #endregion
    }
}