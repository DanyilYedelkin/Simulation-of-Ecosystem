using System.Collections.Generic;
using UnityEngine;

namespace Animals.Predator
{
    public class PredatorSearchRadius : SearchRadius
    {
        #region Configuration
        [SerializeField] private int                    _preyInRadius = 0;
        [SerializeField] private List<GameObject>       _prey;
        [SerializeField] private PredatorController     _predatorController;
        [SerializeField] private ReproductionController _reproductionSystem;
        #endregion

        #region Private Members
        private Collider[] _hitColliders;
        #endregion

        #region Unity Methods
        protected override void Update()
        {
            base.Update();

            _prey.Clear();
            _hitColliders = Physics.OverlapSphere(this.transform.position, AnimalSearchRadius);
            _preyInRadius = 0;

            foreach (var hitCollider in _hitColliders)
            {
                if (hitCollider.gameObject.tag.Equals("Prey"))
                {
                    _preyInRadius++;
                    _prey.Add(hitCollider.gameObject);
                }

                if (hitCollider.gameObject.tag.Equals("Predator"))
                {
                    _reproductionSystem.MateWasFound(hitCollider.gameObject);
                }

                if (hitCollider.gameObject.tag.Equals("Water"))
                {
                    _predatorController.WaterWasSeen(hitCollider.gameObject);
                }
            }

            if (_preyInRadius > 0)
            {
                FindClosestPrey();
            }
            else if (_predatorController.CurrentState == AnimalState.ChasingPrey)
            {
                _predatorController.PreyWasLost();
            }
        }

        /*private void OnTriggerEnter(Collider other)
        {
            // _predatorController.CurrentState != AnimalState.ChasingPrey
            if (other.gameObject.tag.Equals("Prey"))
            {
                FindClosestPrey();
            }
            
            if (other.gameObject.tag.Equals("Predator"))
            {
                _reproductionSystem.MateWasFound(other.gameObject);
            }

            if (other.gameObject.tag.Equals("Water"))
            {
                _predatorController.WaterWasSeen(other.gameObject);
            }
        }*/

        /*private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.tag.Equals("Prey"))
            {
                _predatorController.PreyWasLost();
            }
        }*/
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