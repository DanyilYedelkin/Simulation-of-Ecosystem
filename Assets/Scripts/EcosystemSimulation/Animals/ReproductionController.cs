using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Animals
{
    public class ReproductionController : MonoBehaviour
    {
        #region Configuration
        [SerializeField] private int                       _inheritanceCount = 1;
        [SerializeField] private AnimalBehaviourController _currentAnimal;
        [SerializeField] private MutationController        _mutationController;
        [SerializeField] private GeneStructure             _geneStructure;
        #endregion

        #region Private Members
        private EcosystemManager   _ecosystemManager = null;
        private NavMeshAgent       _currentAgent     = null;
        #endregion

        #region Unity Methods
        private void Start()
        {
            if (!_currentAnimal)
            {
                _currentAnimal = GetComponent<AnimalBehaviourController>();
            }
            _ecosystemManager = GameObject.FindGameObjectWithTag("EcosystemManager").GetComponent<EcosystemManager>();
            _currentAgent = GetComponent<NavMeshAgent>();
        }

        private void Update()
        {
            if (_currentAnimal.MateAnimal && _currentAnimal.CurrentState == AnimalState.LookingForMate)
            {
                GoToTheMate();
            }
        }
        #endregion

        #region API
        public void MateWasFound(GameObject mateAnimal)
        {
            if (_currentAnimal.gameObject.transform.position == mateAnimal.transform.position)
            {
                return;
            }

            AnimalBehaviourController mateAnimalController = mateAnimal.GetComponent<AnimalBehaviourController>();

            if (_currentAnimal.ReproductiveNeed > 30 && _currentAnimal.Hunger < 70 &&
                mateAnimalController && mateAnimalController.ReproductiveNeed > 30 &&
                mateAnimalController.Hunger < 70 && mateAnimalController.CurrentState != AnimalState.LookingForMate &&
                mateAnimal != _currentAnimal.gameObject && _currentAnimal.AgeController.CanReproduce() &&
                _currentAnimal.Sex != mateAnimalController.Sex && mateAnimalController.AgeController.CanReproduce())
            {
                _currentAnimal.MateAnimal = mateAnimal;
                mateAnimalController.MateAnimal = _currentAnimal.gameObject;

                _currentAnimal.CurrentState = AnimalState.LookingForMate;
                mateAnimalController.CurrentState = AnimalState.LookingForMate;
            }
        }
        #endregion

        #region Local Methods
        private void GoToTheMate()
        {
            _currentAgent.SetDestination(_currentAnimal.MateAnimal.transform.position);

            Collider selfCollider = _currentAnimal.gameObject.GetComponent<Collider>();
            Collider targetCollider = _currentAnimal.MateAnimal.GetComponent<Collider>();

            if (selfCollider != null && targetCollider != null && selfCollider != targetCollider
                && selfCollider.bounds.Intersects(targetCollider.bounds))
            {
                // if this animal is female
                if (!_currentAnimal.Sex)
                {
                    for (int i = 0; i < _inheritanceCount; i++)
                    {
                        Procreate(_currentAnimal.MateAnimal.GetComponent<AnimalBehaviourController>());
                    }
                }
                _currentAnimal.ReproductiveNeed = 0;
                _currentAnimal.CurrentState = AnimalState.Idle;
            }
        }

        private void Procreate(AnimalBehaviourController mateAnimal)
        {
            if (mateAnimal.Sex != _currentAnimal.Sex)
            {
                if (_currentAnimal.gameObject.tag.Equals("Prey"))
                {
                    GameObject child = Instantiate(_ecosystemManager.PreyPrefab,
                        this.transform.position, Quaternion.identity, _ecosystemManager.PreyParentObject.transform);

                    AnimalBehaviourController childController = child.GetComponent<AnimalBehaviourController>();
                    GeneStructure childGene = child.GetComponent<GeneStructure>();
                    MutationController childMutation = child.GetComponent<MutationController>();
                    if (childController && childGene && childMutation && _ecosystemManager.GeneticEvolutionOfPrey)
                    {
                        childController.Gene = childGene.MendelianInheritance(mateAnimal.Gene, _currentAnimal.Gene);
                        childController.Gene = childMutation.TryMutateGene(childController.Gene);
                        child.GetComponent<SensoryReference>().SensoryRadiusObj.AnimalSearchRadius = childController.Gene.FirstGeneValue;
                    }
                    //else
                    //{
                    //Debug.Log("Error in child's components!");
                    //}
                    child.GetComponent<AgeController>().CurrentAge = 0;
                    _currentAnimal.AddAnimalToManager(child);
                    _ecosystemManager.TotalPrey++;

                }
                if (_currentAnimal.gameObject.tag.Equals("Predator"))
                {
                    GameObject child = Instantiate(_ecosystemManager.PredatorPrefab,
                        this.transform.position, Quaternion.identity, _ecosystemManager.PredatorparentObject.transform);

                    AnimalBehaviourController childController = child.GetComponent<AnimalBehaviourController>();
                    GeneStructure childGene = child.GetComponent<GeneStructure>();
                    MutationController childMutation = child.GetComponent<MutationController>();
                    if (childController && childGene && childMutation && _ecosystemManager.GeneticEvolutionOfPredators)
                    {
                        childController.Gene = childGene.MendelianInheritance(mateAnimal.Gene, _currentAnimal.Gene);
                        childController.Gene = childMutation.TryMutateGene(childController.Gene);
                        child.GetComponent<SensoryReference>().SensoryRadiusObj.AnimalSearchRadius = childController.Gene.FirstGeneValue;
                        childController.FleeingSpeed = childController.Gene.FirstGeneValue;
                    }
                    else
                    {
                        Debug.Log("Error in child's components!");
                    }

                    _currentAnimal.AddAnimalToManager(child);
                    _ecosystemManager.TotalPredators++;
                }
            }
        }
        #endregion
    }
}