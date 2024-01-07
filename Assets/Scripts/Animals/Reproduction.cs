using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Animals
{
    public class Reproduction : MonoBehaviour
    {
        #region Configuration
        [SerializeField] private float                     _reproductionTime = 100f;
        [SerializeField] private float                     _remainingTime;
        [SerializeField] private AnimalBehaviourController _animalBehaviour;
        [SerializeField] private Mutation                  _mutationController;
        #endregion

        #region Private Members
        private EcosystemManager _ecosystemManager;
        #endregion

        #region Unity Methods
        private void Start()
        {
            _ecosystemManager = GameObject.FindGameObjectWithTag("EcosystemManager").GetComponent<EcosystemManager>();
            _remainingTime = _reproductionTime;
        }
        private void Update()
        {
            _remainingTime -= Time.deltaTime;

            if (_remainingTime < .5f && _animalBehaviour.Hunger > 75)
            {
                _remainingTime = _reproductionTime;

                for (int i = 0; i < _animalBehaviour.LitterCount; i++)
                {
                    if (this.gameObject.tag.Equals("Prey"))
                    {
                        GameObject child = Instantiate(_ecosystemManager.PreyPrefab, 
                            this.transform.position, Quaternion.identity, _ecosystemManager.PreyParentObject.transform);
                        InheritGenes(child, this.transform.gameObject);
                        AttempMutations(child);
                        _ecosystemManager.SensoryRadiusOfEntities.Add(child, child.GetComponent<SensoryReference>().SensoryRadiusObj);
                        _animalBehaviour.AddAnimalToManager(child);
                        _ecosystemManager.TotalPrey++;

                    }
                    if (this.gameObject.tag.Equals("Predator"))
                    {
                        GameObject child = Instantiate(_ecosystemManager.PredatorPrefab, 
                            this.transform.position, Quaternion.identity, _ecosystemManager.PredatorparentObject.transform);
                        _mutationController.AttemptSensoryMutation(child);
                        _ecosystemManager.SensoryRadiusOfEntities.Add(child, child.GetComponent<SensoryReference>().SensoryRadiusObj);
                        _animalBehaviour.AddAnimalToManager(child);
                        _ecosystemManager.TotalPredators++;
                    }
                }
            }
        }
        #endregion

        #region Local Methods
        private void AttempMutations(GameObject child)
        {
            _mutationController.AttemptSensoryMutation(child);
            //_mutationController.AttempSpeedMutation(child);
            //_mutationController.AttempLitterCountMutation(child);
            //_mutationController.AttemptHungerMutation(child);
        }

        private void InheritGenes(GameObject child, GameObject parent)
        {
            child.GetComponent<SensoryReference>().SensoryRadiusObj.AnimalSearchRadius = _ecosystemManager.SensoryRadiusOfEntities[parent].AnimalSearchRadius;
        }
        #endregion
    }
}