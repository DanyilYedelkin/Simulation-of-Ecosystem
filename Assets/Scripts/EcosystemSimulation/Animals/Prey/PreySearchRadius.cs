using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Animals.Prey
{
    public class PreySearchRadius : SearchRadius
    {
        #region Configuration
        [SerializeField] private int                    _predatorsInRadius = 0;
        [SerializeField] private List<GameObject>       _predators;
        [SerializeField] private PreyController         _preyController;
        [SerializeField] private ReproductionController _reproductionSystem;
        #endregion

        #region Private Members
        private Collider[]                _hitColliders;
        #endregion

        #region Unity Methods
        protected override void Update()
        {
            base.Update();

            _predators.Clear();
            _hitColliders = Physics.OverlapSphere(this.transform.position, AnimalSearchRadius);
            _predatorsInRadius = 0;

            foreach (var hitCollider in _hitColliders)
            {
                if (hitCollider.gameObject.tag.Equals("Predator"))
                {
                    _predatorsInRadius++;
                    _predators.Add(hitCollider.gameObject);
                }

                if (hitCollider.gameObject.tag.Equals("Prey"))
                {
                    _reproductionSystem.MateWasFound(hitCollider.gameObject);
                }

                if (hitCollider.gameObject.tag.Equals("Plant"))
                {
                    _preyController.FoodWasSeen(hitCollider.gameObject);
                }

                if (hitCollider.gameObject.tag.Equals("Water"))
                {
                    _preyController.WaterWasSeen(hitCollider.gameObject);
                }
            }

            if (_predatorsInRadius > 0)
            {
                _preyController.PredatorWasSeen(_predators);
            }
            else if (_preyController.CurrentState == AnimalState.Fleeing)
            {
                _preyController.PredatorWasLost();
            }
        }
        #endregion
    }
}